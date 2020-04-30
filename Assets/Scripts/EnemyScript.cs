using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class EnemyScript : MonoBehaviour
{
    Animator anim;
    public bool dead;
    public Transform weaponHolder;
    GameObject Camera;
    public float distance = 3.0f;
    bool readyToShoot;
    NavMeshAgent navMeshAgent;
    int layerMask;
    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        anim = GetComponent<Animator>();
        StartCoroutine(RandomAnimation());
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weaponHolder.GetComponentInChildren<WeaponScript>().active = false;
        layerMask = 1 << 15;
    }

    void Update()
    {
        if (!dead & BreakResDoorScript.instance.breakResseptionDoor == true)
        {
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
        }
        else
        {
            navMeshAgent.enabled = false;
            navMeshAgent.velocity = Vector3.zero;
        }
        RaycastHit wallCheck; RaycastHit enemyCheck;
        if (!dead)
        {
            if(weaponHolder.GetComponentInChildren<WeaponScript>() != null)
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
    }
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
}