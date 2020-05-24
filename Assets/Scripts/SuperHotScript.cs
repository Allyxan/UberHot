using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Audio;

public class SuperHotScript : MonoBehaviour
{
    bool testAnim = false;
    float LastTime;
    public static SuperHotScript instance;
    public bool canShoot = true;
    public bool action;
    public GameObject bullet;
    public Transform bulletSpawner;
    public float forPitch;
    public AudioMixer master;

    [Header("Weapon")]
    public WeaponScript weapon;
    public Transform weaponHolder;
    public LayerMask weaponLayer;


    [Space]
    [Header("UI")]
    public Image indicator;

    [Space]
    [Header("Prefabs")]
    public GameObject hitParticlePrefab;
    public GameObject bulletPrefab;


    private void Awake()
    {
        instance = this;
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weapon = weaponHolder.GetComponentInChildren<WeaponScript>();
    }


    void Update()
    {
        LastTime = Time.timeScale;
        if (Cursor.visible == false)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                BreakResDoorScript.breakingdoor = false;
                BreakAdmDoorScript.breakingdoor = false;
                testAnim = false;
            }
            if (testAnim == false)
            {
                if ((BreakResDoorScript.breakingdoor == false) & (BreakAdmDoorScript.breakingdoor == false))
                {

                    if (canShoot)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (HandScript.doing == true)
                            {
                                if (Controller.bb == false)
                                {
                                    if (weapon != null && weapon.reloading == false)
                                    {
                                        StopCoroutine(ActionE(.03f));
                                        StartCoroutine(ActionE(.03f));
                                        weapon.Shoot(SpawnPos() + Camera.main.transform.forward * .5f, Camera.main.transform.rotation, false);
                                    }
                                } else
                                {
                                    Controller.Instance.BBB();
                                }
                            }
                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            if (HandScript.doing == true)
                            {
                                if (weapon != null && weapon.reloading == false)
                                {
                                    StopCoroutine(ActionE(.4f));
                                    StartCoroutine(ActionE(.4f));
                                    weapon.Throw();
                                    weapon = null;
                                }
                            }
                        }
                    }


                    /* if (Input.GetMouseButtonDown(1))
                     {
                         StopCoroutine(ActionE(.4f));
                         StartCoroutine(ActionE(.4f));

                         if (weapon != null)
                         if (Input.GetMouseButtonDown(1))
                         {
                         }

                     }*/

                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3, weaponLayer))
                    {
                        if (Input.GetMouseButtonDown(0) && weapon == null)

                        {
                            if (HandScript.doing == true)
                            {
                                hit.transform.GetComponent<WeaponScript>().Pickup();
                            }
                        }
                    }

                    float x = Input.GetAxisRaw("Horizontal");
                    float y = Input.GetAxisRaw("Vertical");

                    //float time = (x != 0 || y != 0) ? 1f : .01f;
                    //float lerpTime = (x != 0 || y != 0) ? .05f : .5f;
                    float time = (x != 0 || y != 0) ? 1f : .05f;
                    float lerpTime = (x != 0 || y != 0) ? .05f : .8f;

                    time = action ? 1 : time;
                    lerpTime = action ? .1f : lerpTime;

                    Time.timeScale = Mathf.Lerp(LastTime, time, lerpTime);
                    forPitch = Time.timeScale;
                    if (forPitch < 0.2f) forPitch = 0.2f;
                    master.SetFloat("masterPitch", forPitch);

                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        LastTime = Time.timeScale;
                        testAnim = true;
                        Time.timeScale = 1;
                        StartCoroutine(PlaySecretKey());

                    }
                }
                else
                {
                    Time.timeScale = 1;
                }
            }
        }
    }

    IEnumerator ActionE(float time)
    {
        action = true;
        yield return new WaitForSecondsRealtime(.06f);
        action = false;
    }

    public void ReloadUI(float timee)
    {
        indicator.transform.eulerAngles = new Vector3(0, 0, 45);
        //сделала по тупому, прошу прощения
        indicator.transform.DORotate(new Vector3(0, 0, 90), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(() => indicator.transform.DOPunchScale(Vector3.one / 3, .2f, 10, 1).SetUpdate(true));
    }
    Vector3 SpawnPos()
    {
        return Camera.main.transform.position + (Camera.main.transform.forward * .5f) + (Camera.main.transform.up * -.02f);
    }

    IEnumerator PlaySecretKey()
    {
        yield return new WaitForSecondsRealtime(5f);
        testAnim = false;
        Time.timeScale = LastTime;
    }
}