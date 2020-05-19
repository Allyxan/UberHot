using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartScript : MonoBehaviour
{
<<<<<<< HEAD
    public Rigidbody rb;
    public EnemyScript enemy;
    public Renderer bodyPartRenderer;
    public GameObject bodyPartPrefab;
    public bool replaced; //bool StopUpdate = true;
=======
    public Rigidbody rb; //твердое тело
    public EnemyScript enemy; //противник
    public Renderer bodyPartRenderer; // доступ к телу
    public GameObject bodyPartPrefab; // объект
    public bool replaced; 


    public int maxHealth = 90;
    public int _curHealth = 90;


    // Start is called before the first frame update
>>>>>>> Pola
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (maxHealth < 1)
            maxHealth = 1;
        _curHealth = maxHealth;
    }
<<<<<<< HEAD
    void Update()
    {
        /*if(StopUpdate && replaced)
        {
            StartCoroutine(FadeAway());
            transform.parent.Translate(Vector3.down * Time.deltaTime, Space.World);
        }*/
=======

    void Update()
    {
        AddjustCurrentHealth(_curHealth);
>>>>>>> Pola
    }
    public void HidePartAndReplace()
    {
        if (replaced)
            return;

        if (bodyPartRenderer != null)
            bodyPartRenderer.enabled = false;

        GameObject part = new GameObject();
        if (bodyPartPrefab != null)
            part = Instantiate(bodyPartPrefab, transform.position, transform.rotation);
        part.transform.parent = transform;

        Rigidbody[] rbs = part.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody r in rbs)
        {
            r.interpolation = RigidbodyInterpolation.Interpolate;
            r.AddExplosionForce(15, transform.position, 5);
        }

        rb.AddExplosionForce(15, transform.position, 5);

        this.enabled = false;
        replaced = true;

    }
<<<<<<< HEAD
    /*IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(3f);
        StopUpdate = false;
        Destroy(transform.parent);
    }*/
=======
    public void AddjustCurrentHealth(int adj)
    {
        _curHealth = adj;
        if (_curHealth < 0)
            _curHealth = 0;
        if (_curHealth > maxHealth)
            _curHealth = maxHealth;
    }
>>>>>>> Pola
}
// part какойто новый объект
// rb твердое тело 
//