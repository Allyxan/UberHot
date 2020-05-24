using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class EnemyScript : MonoBehaviour
{


    bool testAnim = false;
    Animator anim;
    public bool dead;
    public static EnemyScript instance;
    public Transform weaponHolder;
    GameObject Camera;
    public float distance = 0.05f;
    bool readyToShoot;
    NavMeshAgent navMeshAgent;
    int layerMask;
    bool borrow = false;
    public int Probnay;
    public GameObject DrugoiObiekt;
    private PlayerAttack playerAttack;

    public AudioClip ZvukSmert;
    public AudioSource audio;

    public int lifes;

    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        anim = GetComponent<Animator>();
        StartCoroutine(RandomAnimation());
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weaponHolder.GetComponentInChildren<WeaponScript>().active = false;
        layerMask = 1 << 15;
        //GameObject go = GameObject.Find("Enemy (Resseption)");
        //PlayerAttack playerAttack = go.GetComponent<PlayerAttack>();
        //int current = playerAttack.BB;
        //Probnay = current;
        audio = GetComponent<AudioSource>();
        lifes = 2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ///anim.SetBool("Secter key", true); заменить на включение чит кода
            //добавьте в аниматор противника assets/ new animation / test_sectret key; из any state мейби
            testAnim = true;
            navMeshAgent.enabled = false;
            StartCoroutine(PlaySecretKey());
        }
        //if (testAnim == false) // anim.SetBool("Test_anim", false); заменить на выключение чит кода

        if (testAnim == false)
        {
            if ((BreakResDoorScript.breakingdoor == false) & (BreakAdmDoorScript.breakingdoor == false))
            {

                if (!dead && BreakResDoorScript.instance.breakResseptionDoor == true)
                {

                    /*if((gameObject.name == "Enemy (1st stage)" && BreakResDoorScript.instance.breakResseptionDoor == true)
                        || (gameObject.name == "Enemy (2nd stage)" && TriggerScript.instance.trigger2ndPassed == true)
                        || (gameObject.name == "Enemy (3rd stage)" && TriggerScript.instance.trigger3rdPassed == true)
                        || (gameObject.name == "Enemy (Administration)" TriggerScript.instance.triggerAdminPassed == true))
                    {
                        gameObject.SetActive(true);
                        navMeshAgent.enabled = true;
                        transform.LookAt(new Vector3(Camera.transform.position.x, Camera.transform.position.y-1.5f, Camera.transform.position.z));
                        float NewDistance = Vector3.Distance(transform.position, Camera.transform.position);
                        if (NewDistance > distance)
                        {
                            navMeshAgent.SetDestination(Camera.transform.position);
                        }
                        else
                        {
                            Vector3 direction = transform.position - Camera.transform.position;
                            Vector3 FallBack = transform.position + direction;
                            navMeshAgent.SetDestination(FallBack);
                        }
                    }*/
                    navMeshAgent.enabled = true;
                    transform.LookAt(new Vector3(Camera.transform.position.x, Camera.transform.position.y - 1.5f, Camera.transform.position.z));
                    float NewDistance = Vector3.Distance(transform.position, Camera.transform.position);
                    if (NewDistance > distance)
                    {
                        navMeshAgent.SetDestination(Camera.transform.position);
                    }
                    else
                    {
                       Vector3 direction = transform.position - Camera.transform.position;
                       Vector3 FallBack = transform.position + direction;
                       navMeshAgent.SetDestination(FallBack);
                    }
                }
                else
                {
                    navMeshAgent.enabled = false;
                    navMeshAgent.velocity = Vector3.zero;

                }
                RaycastHit wallCheck; RaycastHit enemyCheck;
                if (!dead)
                {
                    if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
                    {
                        Debug.DrawRay(weaponHolder.GetComponentInChildren<WeaponScript>().transform.position, transform.forward * 5, Color.green);
                        //if (Physics.Raycast(weaponHolder.GetComponentInChildren<WeaponScript>().transform.position, transform.forward, out enemyCheck, 100, layerMask)) //Staraya versiya; Nova robit lu4she
                        if (Physics.SphereCast(weaponHolder.GetComponentInChildren<WeaponScript>().transform.position, 0.1f, transform.forward, out enemyCheck, 100, layerMask))
                        {
                            readyToShoot = false;
                        }
                        else
                        {
                            if (Physics.SphereCast(weaponHolder.GetComponentInChildren<WeaponScript>().transform.position, 0.1f, transform.forward, out wallCheck, 100))

                            {
                                if (wallCheck.transform.CompareTag("Wall"))
                                {
                                    readyToShoot = false;
                                }
                                if (wallCheck.transform.CompareTag("Player"))
                                {
                                    readyToShoot = true;
                                }
                            }
                        }
                    }
                    /*else
                    {

                    }*/
                }
                if (dead && borrow)
                {
                    transform.Translate(Vector3.down * 0.3f * Time.deltaTime, Space.World);
                }
            } else
            {
                navMeshAgent.enabled = false;
            }
        }
    }
    public void Ragdoll()
    {
        tag = "DeadEnemy";
        anim.enabled = false;
        BodyPartScript[] parts = GetComponentsInChildren<BodyPartScript>();
        foreach (BodyPartScript bp in parts)
        {
            bp.tag = "DeadEnemy";
            bp.rb.isKinematic = false;
            bp.rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        dead = true;

        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
        {
            WeaponScript w = weaponHolder.GetComponentInChildren<WeaponScript>();
            w.Release();
        }
        audio.PlayOneShot(ZvukSmert, 0.7F);
        if (gameObject.name != "Enemy (Administration)")
            StartCoroutine(WaitAndBury());
        //WeaponRelease();
    }
    //public void Smert()
    //{
    //    //GameObject go = GameObject.Find("Enemy (Resseption)");
    //    //PlayerAttack playerAttack = go.GetComponent<PlayerAttack>();
    //    //int current = playerAttack.BB;
    //        tag = "DeadEnemy";
    //        anim.enabled = false;
    //       if (Probnay == 1)
    //        dead = true;

    //    if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
    //    {
    //        WeaponScript w = weaponHolder.GetComponentInChildren<WeaponScript>();
    //        w.Release();
    //    }
    //}
    public void WeaponRelease()
    {
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
        {
            WeaponScript w = weaponHolder.GetComponentInChildren<WeaponScript>();
            w.Release();
        }
    }
    public void Shoot()
    {
        if (dead)
            return;
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null && readyToShoot == true)
        {
            weaponHolder.GetComponentInChildren<WeaponScript>().Shoot(GetComponentInChildren<ParticleSystem>().transform.position,
                transform.rotation, true);
        }
    }

    IEnumerator RandomAnimation()
    {
        anim.enabled = false;
        yield return new WaitForSecondsRealtime(Random.Range(.1f, .5f));
        anim.enabled = true;
    }

    IEnumerator WaitAndBury()
    {
        yield return new WaitForSeconds(10f);
        GameObject skeleton = gameObject.transform.Find("LowManSkeleton").gameObject;
        skeleton.SetActive(false);
        borrow = true;
        yield return new WaitForSeconds(5f);
        borrow = false;
        Destroy(gameObject);
    }
    IEnumerator PlaySecretKey()
    {
        yield return new WaitForSecondsRealtime(5f);
        testAnim = false;
        navMeshAgent.enabled = true;
    }
    public void ChangeMaterial(int lifes)
    {
        // this.GetComponentsInChildren<Renderer>().material = material1;
        SkinnedMeshRenderer[] clr = GetComponentsInChildren<SkinnedMeshRenderer>();
        //clr[1].material = material1;
        foreach (SkinnedMeshRenderer r in clr)
        {

            if (lifes == 1)
            {
                Debug.Log("fd");
                r.material.color = Color.grey;
            }
            if (lifes == 0)
            {
                r.material.color = Color.black;
            }
        }
    }
}