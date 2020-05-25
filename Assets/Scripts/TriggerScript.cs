using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool dontFinishTheGame = true;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;
    public GameObject spawnPoint4;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (gameObject.name == "Trigger (2nd stage)")
            {
                spawnPoint1.SetActive(true);
                spawnPoint2.SetActive(true);
                foreach (GameObject enemy in GameObject.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    if (enemy.name == "Enemy (2nd stage)")
                    {                         
                        enemy.SetActive(true);
                    }
                }
            }
            if (gameObject.name == "Trigger (3rd stage)")
            {
                spawnPoint3.SetActive(true);
                spawnPoint4.SetActive(true);
                foreach (GameObject enemy in GameObject.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    if (enemy.name == "Enemy (3rd stage)")
                        enemy.SetActive(true);
                }
            }
            if (gameObject.name == "Trigger (Administration)")
            {
                foreach (GameObject enemy in GameObject.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    if (enemy.name == "Enemy (Administration)")
                        enemy.SetActive(true);
                }
                dontFinishTheGame = false;
            }
        }
    }
}