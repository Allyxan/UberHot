using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartScript : MonoBehaviour
{
    public Rigidbody rb;
    public EnemyScript enemy;
    public Renderer bodyPartRenderer;
    public GameObject bodyPartPrefab;
    public bool replaced; //bool StopUpdate = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        /*if(StopUpdate && replaced)
        {
            StartCoroutine(FadeAway());
            transform.parent.Translate(Vector3.down * Time.deltaTime, Space.World);
        }*/
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
    /*IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(3f);
        StopUpdate = false;
        Destroy(transform.parent);
    }*/
}
