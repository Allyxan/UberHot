using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartScript : MonoBehaviour
{

    public  Material material0;
    public Material material1;
    public Rigidbody rb; //твердое тело
    public EnemyScript enemy; //противник
    public EnemyColour colour;
    public Renderer bodyPartRenderer; // доступ к телу
    public GameObject bodyPartPrefab; // объект
    public bool replaced;



    public int maxHealth = 90;
    public int _curHealth = 90;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (maxHealth < 1)
            maxHealth = 1;
        _curHealth = maxHealth;
    }

    void Update()
    {
        AddjustCurrentHealth(_curHealth);
    }

    public void ChangeMaterial(int lifes)
    {
        enemy.ChangeMaterial(lifes);

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
    public void AddjustCurrentHealth(int adj)
    {
        _curHealth = adj;
        if (_curHealth < 0)
            _curHealth = 0;
        if (_curHealth > maxHealth)
            _curHealth = maxHealth;
    }
}
// part какойто новый объект
// rb твердое тело 
//