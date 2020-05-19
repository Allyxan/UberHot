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

    [Space]
    [Header("Weapon Settings")]
    public float reloadTime = .3f;
    public int bulletAmount = 6;
    Vector3 recoil = new Vector3 (-30f, 0, 0);

    public AudioClip ZvukVistrela;
    public AudioClip ZvukPustogoPistoleta;
    public AudioClip Brosit;
    public AudioClip Podniat;
    public AudioSource audio;


    void Start()
    {
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
    public void Shoot(Vector3 pos,Quaternion rot, bool isEnemy)
    {
        if (reloading || bulletAmount <= 0)
            return;

<<<<<<< HEAD
        //if (bulletAmount <= 0)
        //   return;
=======
        if (bulletAmount <= 0)
        {
            audio.PlayOneShot(ZvukPustogoPistoleta, 0.7F);
            return;
        }
>>>>>>> Pola

        if (SuperHotScript.instance.weapon == this)
        {
            audio.PlayOneShot(ZvukVistrela, 0.7F);
            bulletAmount--;
            
        }

            if (isEnemy == false)
        {
            GameObject bullet = Instantiate(SuperHotScript.instance.bulletPrefab, pos, rot);
            bullet.tag = "PlayerBullet";
        }

        if(isEnemy == true)
        {
            GameObject bullet = Instantiate(SuperHotScript.instance.bulletPrefab, pos, rot);
        }

        if (GetComponentInChildren<ParticleSystem>() != null)
        {
            GetComponentInChildren<ParticleSystem>().Play();
            audio.PlayOneShot(ZvukVistrela, 0.7F);
        }

            if (SuperHotScript.instance.weapon == this)
            StartCoroutine(Reload());

        Camera.main.transform.DOComplete();

       if (SuperHotScript.instance.weapon == this)
        {
            transform.DOLocalRotate(recoil, .2f).OnComplete(() => transform.DOLocalRotate(Vector3.zero, .1f));
            transform.DOLocalMoveZ(-.1f, .2f).OnComplete(() => transform.DOLocalMoveZ(0, .1f));
            transform.DOLocalMoveY(.1f, .2f).OnComplete(() => transform.DOLocalMoveY(0, .1f));
            transform.DOLocalMoveX(-.01f, .2f).OnComplete(() => transform.DOLocalMoveX(0, .1f));
        }
    }

    public void Throw()
    {
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMove(transform.position - transform.forward, .01f)).SetUpdate(true);
        s.AppendCallback(() => transform.parent = null);
        s.AppendCallback(() => transform.position = Camera.main.transform.position + (Camera.main.transform.right * .1f));
        s.AppendCallback(() => ChangeSettings());
        s.AppendCallback(() => rb.AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse));
        s.AppendCallback(() => rb.AddTorque(transform.transform.right + transform.transform.up * 20, ForceMode.Impulse));
        audio.PlayOneShot(Brosit, 0.7F);
    }

    public void Pickup()
    {
        if (!active)
            return;

        SuperHotScript.instance.weapon = this;
        ChangeSettings();

        transform.parent = SuperHotScript.instance.weaponHolder;

        transform.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.OutBack).SetUpdate(true);
        transform.DOLocalRotate(Vector3.zero, .25f).SetUpdate(true);
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
}
