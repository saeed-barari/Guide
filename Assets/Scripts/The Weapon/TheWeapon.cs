using BaldrAttributes;
using UnityEngine;
using System.Collections;
public class TheWeapon : MonoBehaviour
{
    [Group("Shooting")]
    public float damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float maxDistance;

    [Group("Ammo & Reloading")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int magSize;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool isReloading;

    [Group("Assignables")]
    [SerializeField] private Camera fpsCam;

    // Invisible Variables
    RaycastHit hit;
    float timeSinceLastShot;

    private bool CanShoot() => !isReloading && timeSinceLastShot > 1f / (fireRate / 60f);

    void Update() {
        // Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Shoot();
        }
        //reload
        if (Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(Reload());
        }
    }

    void Shoot() {
        if (currentAmmo > 0) {
            if (CanShoot()) {
                Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, maxDistance);
                Target target = hit.transform.GetComponent<Target>();
                if (target != null) {
                    target.health = target.health - damage;
                }
                currentAmmo--;
            }
        }
    }

    private IEnumerator Reload() {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magSize;
        isReloading = false;
    }
}
