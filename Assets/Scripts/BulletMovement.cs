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
        if (collision.gameObject.CompareTag("Enemy") && CompareTag("PlayerBullet"))
        {
            BodyPartScript bp = collision.gameObject.GetComponent<BodyPartScript>();
            Instantiate(SuperHotScript.instance.hitParticlePrefab, transform.position, transform.rotation);
            bp.HidePartAndReplace();
            bp.enemy.Ragdoll();
        }
        if(collision.gameObject.CompareTag("Player") && !(CompareTag("PlayerBullet")))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        Destroy(gameObject);
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}