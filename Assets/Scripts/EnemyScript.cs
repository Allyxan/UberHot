using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class EnemyScript : MonoBehaviour
{
    public bool testAnim = false;
    public GameObject weaponHolderEnemy;
    public float closeDistance = Mathf.Infinity;
    Animator anim;
    public bool dead;
    public static EnemyScript instance;
    public Transform weaponHolder;
    GameObject Camera;
    public float distance = 3.0f;
    bool readyToShoot;
    NavMeshAgent navMeshAgent;
    int layerMask; int saveIterator;
    bool borrow = false; public bool waited = false; public bool aggro = true;
    public AudioClip EnemyDeath;
    public AudioClip HitByThrowable;
    public AudioSource audio;

    void Start()
    {
        aggro = false;
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        anim = GetComponent<Animator>();
        StartCoroutine(RandomAnimation());
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weaponHolder.GetComponentInChildren<WeaponScript>().active = false;
        layerMask = 1 << 15;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (testAnim == false)
            {
                StopCoroutine(PlaySecretKey());
                StartCoroutine(PlaySecretKey());
            }
        }
        //  if (testAnim == false) 

        if (testAnim == false)
        {
            if ((BreakResDoorScript.breakingdoor == false) & (BreakAdmDoorScript.breakingdoor == false))
            {
                if (!dead && BreakResDoorScript.instance.breakResseptionDoor == true || (!dead
                    && name == "Enemy (Resseption)" && aggro == true))
                {
                    anim.SetTrigger("Start");
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
                        if (Physics.SphereCast(weaponHolder.GetComponentInChildren<WeaponScript>().transform.position, 0.1f, transform.forward, out enemyCheck, 100, layerMask))
                        {
                            readyToShoot = false;
                            anim.SetBool("Shoot+Walk", false);
                        }
                        else
                        {
                            if (Physics.SphereCast(weaponHolder.GetComponentInChildren<WeaponScript>().transform.position, 0.1f, transform.forward, out wallCheck, 100))
                            {
                                if (wallCheck.transform.CompareTag("Wall"))
                                {
                                    readyToShoot = false;
                                    anim.SetBool("Shoot+Walk", false);
                                }
                                if (wallCheck.transform.CompareTag("Player") || wallCheck.transform.CompareTag("MainCamera"))
                                {
                                    readyToShoot = true;
                                    anim.SetBool("Shoot+Walk", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        anim.SetBool("WalkNWTOStayNW", true);
                        GameObject[] closestGuns = GameObject.FindGameObjectsWithTag("EnemyGun");
                        if (name == "Enemy (Resseption)")
                        {
                            navMeshAgent.enabled = true;//С этого места идет агр врага на рессепшне
                            aggro = true;
                        }
                        for (int i = 0; i < closestGuns.Length; i++)
                        {
                            if (Vector3.Distance(transform.position, closestGuns[i].transform.position) <= closeDistance)
                            {
                                //navMeshAgent.SetDestination(closestGuns[i].transform.position);
                                saveIterator = i;
                            }
                        }
                        anim.SetBool("WalkNWTOStayNW", true);
                        navMeshAgent.SetDestination(closestGuns[saveIterator].transform.position);
                        if (Vector3.Distance(transform.position, closestGuns[saveIterator].transform.position) <= 1f)
                        {
                            anim.SetBool("WalkNWTOStayNW", false);
                            StartCoroutine(WaitForPickup());
                            if (waited)
                            {
                                closestGuns[saveIterator].tag = "Untagged";
                                closestGuns[saveIterator].GetComponent<Rigidbody>().isKinematic = true;
                                closestGuns[saveIterator].GetComponent<Rigidbody>().useGravity = false;
                                closestGuns[saveIterator].GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
                                closestGuns[saveIterator].GetComponent<Collider>().isTrigger = false;
                                closestGuns[saveIterator].transform.parent = weaponHolderEnemy.transform;
                                closestGuns[saveIterator].transform.position = weaponHolderEnemy.transform.position;
                                closestGuns[saveIterator].transform.localRotation = Quaternion.Euler(-44.157f, 2.143f, 178.507f);
                                closestGuns[saveIterator].GetComponent<Rigidbody>().useGravity = true;
                            }
                        }
                    }
                }
                if (dead && borrow)
                {
                    transform.Translate(Vector3.down * 0.3f * Time.deltaTime, Space.World);
                }
            }
            else
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
        audio.PlayOneShot(EnemyDeath, 0.15F);
        if (gameObject.name != "Enemy (Administration)")
            StartCoroutine(WaitAndBury());
        //WeaponRelease();
    }
    public void WeaponRelease()
    {
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
        {
            WeaponScript w = weaponHolder.GetComponentInChildren<WeaponScript>();
            anim.SetTrigger("Damage");
            w.Release();
            waited = false;
            audio.PlayOneShot(HitByThrowable, 0.3F);
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
    IEnumerator WaitForPickup()
    {
        anim.SetTrigger("Take");
        yield return new WaitForSeconds(2f);
        waited = true;
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
        anim.SetBool("K", true); //заменить на включение чит кода
        anim.SetTrigger("Kk");
        testAnim = true;
        navMeshAgent.enabled = false;
        yield return new WaitForSecondsRealtime(5f);
        testAnim = false;
        anim.SetBool("K", false);
        navMeshAgent.enabled = true;
    }
}