using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float Distant = 5f;
    //public bool dead;
    //public Transform weaponHolder;
    //bool readyToShoot = false;
    public int BB = 0;
   
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, Distant))
        {
            BB = 1;
        }
        BB = 0;
    }
    //public void Shoot()
    //{
    //    if (dead)
    //        return;
    //    if (weaponHolder.GetComponentInChildren<WeaponScript>() != null && readyToShoot == true)
    //    {
    //        weaponHolder.GetComponentInChildren<WeaponScript>().Shoot(GetComponentInChildren<ParticleSystem>().transform.position,
    //            transform.rotation, true);
    //    }
    //}
}
