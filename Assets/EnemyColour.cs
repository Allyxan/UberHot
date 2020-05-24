using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColour : MonoBehaviour
{
    public EnemyColour instance;
    public Material material0;
    public Material material1;

    public GameObject bodyPartPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeMaterial(int lifes)
    {
        // this.GetComponentsInChildren<Renderer>().material = material1;
        SkinnedMeshRenderer[] clr = GetComponentsInChildren<SkinnedMeshRenderer>();
        //clr[1].material = material1;
        foreach (SkinnedMeshRenderer r in clr)
        {

            if (lifes == 1)
            {
                Debug.Log("fd");
                r.material.color =  Color.grey;
            }
            if (lifes == 0)
            {
                r.material.color = Color.black;
            }
        }
    }
}
