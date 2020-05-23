using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class WeaponScript : MonoBehaviour
{
    [Header("Bools")]
    public bool active = true;
    public bool reloading;

    private Rigidbody rb;
    private Collider collider;
    private Renderer renderer;
    Transform weaponHolder;
    public static WeaponScript instance;

    [Space]
    [Header("Weapon Settings")]
    public float reloadTime = .7f;
    public int bulletAmount = 6;
    Vector3 recoil = new Vector3(-30f, 0, 0);

    public AudioClip ZvukVistrela;
    public AudioClip ZvukPustogoPistoleta;
    public AudioClip Brosit;
    public AudioClip Podniat;
    public AudioSource audio;

    void Start()
    {
        if (instance == null)
            instance = this;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        audio = GetComponent<AudioSource>();


        ChangeSettings();
    }
    void ChangeSettings()
    {
        if (transform.parent != null)
            return;

        rb.isKinematic = (SuperHotScript.instance.weapon == this) ? true : false;
        rb.interpolation = (SuperHotScript.instance.weapon == this) ? RigidbodyInterpolation.None : RigidbodyInterpolation.Interpolate;
        collider.isTrigger = (SuperHotScript.instance.weapon == this);
    }
    public void Shoot(Vector3 pos, Quaternion rot, bool isEnemy)
    {
        if (reloading)
            return;

        if (bulletAmount <= 0)
        {
            audio.PlayOneShot(ZvukPustogoPistoleta, 0.7F);
            return;
        }

        if (SuperHotScript.instance.weapon == this)
        {
            GunScript.shoot = true;
        }

        //if (bulletAmount <= 0)
        //   return;

        if (SuperHotScript.instance.weapon == this)
        {
            audio.PlayOneShot(ZvukVistrela, 0.7F);
            bulletAmount--;
        }


        StartCoroutine(shoot2(pos, rot, isEnemy));


        if (GetComponentInChildren<ParticleSystem>() != null)
        {
            GetComponentInChildren<ParticleSystem>().Play();
            audio.PlayOneShot(ZvukVistrela, 0.7F);
        }

        if (SuperHotScript.instance.weapon == this)
            StartCoroutine(Reload());

        Camera.main.transform.DOComplete();


    }

    public void Throw()
    {
        StartCoroutine(Throw2());
    }

    public void Pickup()
    {
        if (!active)
            return;

        SuperHotScript.instance.weapon = this;
        GunScript.pickup = true;
        GunScript.pickup = true;
        ChangeSettings();
        transform.parent = SuperHotScript.instance.weaponHolder;

        transform.DOLocalMove(Vector3.zero, 0.33f).SetEase(Ease.OutBack);
        transform.DOLocalRotate(Vector3.zero, 0.16f);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        audio.PlayOneShot(Podniat, 0.7F);
    }

    public void Release()
    {
        active = true;
        transform.parent = null;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        collider.isTrigger = false;

        rb.AddForce((Camera.main.transform.position - transform.position) * 2, ForceMode.Impulse);
        rb.AddForce(Vector3.up * 2, ForceMode.Impulse);
    }
    IEnumerator Reload()
    {
        if (SuperHotScript.instance.weapon != this)
            yield break;
        SuperHotScript.instance.ReloadUI(reloadTime);
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.relativeVelocity.magnitude > 8)
        {
            BodyPartScript bp = collision.gameObject.GetComponent<BodyPartScript>();
            if (!bp.enemy.dead)
                Instantiate(SuperHotScript.instance.hitParticlePrefab, transform.position, transform.rotation);
            //bp.enemy.Release();
            //bp.HidePartAndReplace();
            //bp.enemy.Ragdoll();
            bp.enemy.WeaponRelease();
        }
    }
    IEnumerator Throw2()
    {
        float speed = 3;
        float original_delay = 1.15f;
        float delay = original_delay / speed + 0.05f;
        GunScript.throww = true;
        yield return new WaitForSeconds(delay);
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMove(transform.position - transform.forward * 0.5f, .01f)).SetUpdate(true);
        s.AppendCallback(() => transform.parent = null);
        s.AppendCallback(() => transform.position = Camera.main.transform.position +
        (Camera.main.transform.right * .1f) + (Camera.main.transform.up * -.1f) + (Camera.main.transform.forward * .3f));
        s.AppendCallback(() => ChangeSettings());
        s.AppendCallback(() => rb.AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse));
        s.AppendCallback(() => rb.AddTorque(transform.transform.up * 20, ForceMode.Impulse));
        audio.PlayOneShot(Brosit, 0.7F);
    }
    IEnumerator shoot2(Vector3 pos, Quaternion rot, bool isEnemy)
    {
        yield return new WaitForSeconds(.0f); //хелп, там хотелось бы видеть .5f
        GameObject bullet = Instantiate(SuperHotScript.instance.bulletPrefab, pos, rot);
        if (isEnemy == false)
        {
            bullet.tag = "PlayerBullet";
        }
    }

}

