using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float reloadTime = 1f;
    public float fireRate = 0.15f;
    public int magSize = 20;
    public float recoilDistance = 0.1f;
    public float recoilSpeed = 15f;


    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public GameObject weaponFlash;

    private int currentAmmo;
    private bool isReloading = false;
    private float nextTimeOfFire = 0f;
    private Quaternion initialrotation;
    private Vector3 initialPosition;
    private Vector3 reloadRotationOffset = new Vector3(66,50,50);

    void Start()
    {
        currentAmmo = magSize;
        initialPosition = transform.localPosition;
        initialrotation = transform.localRotation;   
    }

    public void Shoot()
    {
        if (isReloading) return;
        if (Time.time < nextTimeOfFire) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        nextTimeOfFire = Time.time + fireRate;
        currentAmmo--;

        Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Instantiate(weaponFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        StopCoroutine(nameof(Recoil));
        StartCoroutine(nameof(Recoil));
    }

    IEnumerator Reload()
    {
        isReloading = true;

        Quaternion targetRotation = Quaternion.Euler(initialrotation.eulerAngles + reloadRotationOffset);
        float halfReload = reloadTime / 2f;
        float t = 0f;

        while (t < halfReload)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(initialrotation, targetRotation, t/halfReload);
            yield return null;
        }

        t = 0f;

        while (t < halfReload)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(targetRotation, initialrotation, t/halfReload);
            yield return null;
        }

        currentAmmo = magSize;
        isReloading = false;
    }

    public void TryReload()
    {
        if (isReloading) return;
        if (currentAmmo == magSize) return;

        StartCoroutine(Reload());
    }

    private IEnumerator Recoil()
    {
        Vector3 recoilTarget = initialPosition + new Vector3(recoilDistance, 0, 0);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * recoilSpeed;
            transform.localPosition = Vector3.Slerp(initialPosition, recoilTarget, t);
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * recoilSpeed;
            transform.localPosition = Vector3.Slerp(recoilTarget, initialPosition, t);
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}
