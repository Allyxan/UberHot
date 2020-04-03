using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyScript : MonoBehaviour
{
    Animator anim;
    public bool dead;
    public Transform weaponHolder;
    public GameObject Camera;
    public float EnemyTurnSpeed = 2.0f;
    public float EnemyRunSpeed = 1.0f;
    public float distance = 4.0f;
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(RandomAnimation());

        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weaponHolder.GetComponentInChildren<WeaponScript>().active = false;

    }

    void Update()
    {
        if (!dead)
        {
            float NewDistance = Vector3.Distance(transform.position, Camera.transform.position);
            transform.LookAt(new Vector3(Camera.transform.position.x, 0, Camera.transform.position.z));
            if (NewDistance > distance)
            {
                transform.Translate(0, 0, EnemyRunSpeed * Time.deltaTime);
            }
        }
    }

    public void Ragdoll()
    {
        anim.enabled = false;
        BodyPartScript[] parts = GetComponentsInChildren<BodyPartScript>();
        foreach (BodyPartScript bp in parts)
        {
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

        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weaponHolder.GetComponentInChildren<WeaponScript>().Shoot(GetComponentInChildren<ParticleSystem>().transform.position, transform.rotation, true);
    }

    IEnumerator RandomAnimation()
    {
        anim.enabled = false;
        yield return new WaitForSecondsRealtime(Random.Range(.1f, .5f));
        anim.enabled = true;

    }
}
