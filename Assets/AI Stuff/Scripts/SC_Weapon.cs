using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class SC_Weapon : MonoBehaviour
{
    public bool singleFire = false;
    public float fireRate = 0.1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int currentMagazineSize = 30;
    public float timeToReload = 1.5f;
    public float weaponDamage = 15;
    public AudioClip fireAudio;
    public AudioClip reloadAudio;

    public Text ammoText;

    [HideInInspector]
    public SC_WeaponManager manager;

    float nextFireTime = 0;
    bool canFire = true;
    int maxMagazineSize = 0;
    AudioSource audioSource;

    private void Start()
    {
        maxMagazineSize = currentMagazineSize;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && singleFire)
        {
            Fire();
        }
        if(Input.GetMouseButton(0) && !singleFire)
        {
            Fire();
        }
        if(Input.GetKeyDown(KeyCode.R) && canFire)
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        if (!canFire)
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }
        
        nextFireTime = Time.time + fireRate;

        if(currentMagazineSize > 0)
        {
            Vector3 firePointPointerPosition = manager.playerCamera.transform.position + manager.playerCamera.transform.forward * 100;
            RaycastHit hit;
            if(Physics.Raycast(manager.playerCamera.transform.position, manager.playerCamera.transform.forward, out hit, 100))
            {
                firePointPointerPosition = hit.point;
            }
            firePoint.LookAt(firePointPointerPosition);
            GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            SC_Bullet bullet = bulletObject.GetComponent<SC_Bullet>();
            bullet.SetDamage(weaponDamage);
            currentMagazineSize--;
            audioSource.clip = fireAudio;
            audioSource.Play();
            ammoText.text = currentMagazineSize.ToString() + " / " + maxMagazineSize.ToString();
        }
        else 
        {
            StartCoroutine(Reload());
        }
        
        
    }

    IEnumerator Reload()
    {
        canFire = false;

        audioSource.clip = reloadAudio;
        audioSource.Play();

        yield return new WaitForSeconds(timeToReload);
        currentMagazineSize = maxMagazineSize;

        canFire = true;
        ammoText.text = currentMagazineSize.ToString() + " / " + maxMagazineSize.ToString();
    }

    public void ActivateWeapon(bool activate)
    {
        StopAllCoroutines();
        canFire = true;
        gameObject.SetActive(activate);
        if (activate)
        {
            ammoText.text = currentMagazineSize.ToString() + " / " + maxMagazineSize.ToString();
        }
    }
}
