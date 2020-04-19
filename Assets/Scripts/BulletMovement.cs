using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(SelfDestruct());
    }
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BodyPartScript bp = collision.gameObject.GetComponent<BodyPartScript>();
            Instantiate(SuperHotScript.instance.hitParticlePrefab, transform.position, transform.rotation);
            bp.HidePartAndReplace();
            bp.enemy.Ragdoll();
        }
        Destroy(gameObject);
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}