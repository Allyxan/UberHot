using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 90;
    private int _curHealth = 90;
    private float healthBarLenght;
    // Start is called before the first frame update
    void Start()
    {
        if (maxHealth < 1)
            maxHealth = 1;
        _curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        AddjustCurrentHealth(_curHealth);
    }
    public void AddjustCurrentHealth (int adj)
    {
        _curHealth = adj;
        if (_curHealth < 0) 
            _curHealth = 0;
        if (_curHealth > maxHealth)
            _curHealth = maxHealth;
    }
}
