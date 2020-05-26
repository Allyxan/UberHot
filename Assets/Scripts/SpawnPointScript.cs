using UnityEngine;
using System.Collections;
public class SpawnPointScript : MonoBehaviour
{
    Animator animSpawn;
    public Transform SpawnPointPosition;
    public float spawnTime = 0.5f;
    public GameObject enemy;
    private Light forLight;
    void Start()
    {
        animSpawn = GetComponent<Animator>();
        forLight = GetComponent<Light>();
        animSpawn.enabled = false;
        Invoke("Spawn", spawnTime);
    }
    void Spawn()
    {
        animSpawn.enabled = true;
        Instantiate(enemy, transform.position, transform.rotation);
        StartCoroutine(DisableAnimation());
    }
    IEnumerator DisableAnimation()
    {
        yield return new WaitForSeconds(2.0f);
        animSpawn.enabled = false;
        forLight.enabled = false;
    }
}