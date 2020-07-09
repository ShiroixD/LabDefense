using UnityEngine;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    public GunType gunType;
    public float damage = 10.0f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public int maxAmmo = 10;
    private int currentAmmo = -1;
    public float reloadTime = 1;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private TextMeshProUGUI _ammoCounter;

    private float nextTimeToFire = 0f;
    private bool isShooting = false;

    public Animator animator;

    void Start()
    {
        if (currentAmmo == -1)
            currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo = 0;
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && _player.State == PlayerStates.NORMAL)
        {
            isShooting = true;
            if (gunType == GunType.RIFLE)
                GetComponent<AudioSource>().Play();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isShooting = false;
            if (gunType == GunType.RIFLE)
                GetComponent<AudioSource>().Stop();
        }

        if (Input.GetButton("Fire1") & Time.time >= nextTimeToFire && _player.State == PlayerStates.NORMAL)
        {
            nextTimeToFire = Time.time * 1f / fireRate;
            Shoot();
        }

        CalculateAmmoCounter();
    }

    private void CalculateAmmoCounter()
    {
        _ammoCounter.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        _ammoCounter.text = "Reloading";
        Debug.Log("Reloading ...");
        animator.SetBool("Reloading", true);
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(reloadSound);
        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = maxAmmo;
        CalculateAmmoCounter();
        isReloading = false;
    }

    void Shoot()
    {
        if (gunType != GunType.RIFLE)
            GetComponent<AudioSource>().PlayOneShot(fireSound);

        if (!muzzleFlash.isPlaying)
            muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impacttGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impacttGo, 1.5f);
        }
    }
}
