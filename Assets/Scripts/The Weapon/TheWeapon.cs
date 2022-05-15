using BaldrAttributes;
using UnityEngine;
using System.Collections;
using TMPro;
public class TheWeapon : MonoBehaviour
{
    [Group("Shooting")]
    public float damage;
    [Tooltip("Fire Rate in Rounds per Minute")]
    [SerializeField] private float fireRate;
    [SerializeField] private float maxDistance;

    [Group("Ammo & Reloading")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int magSize;
    [SerializeField] private float reloadTime;
    private bool isReloading;

    [Group("Assignables")]
    [SerializeField] private Camera fpsCam;
    [SerializeField] private TextMeshProUGUI ammoText;

    // Invisible Variables
    RaycastHit hit;
    Animator m_anim;
    float timeSinceLastShot;

    private bool CanShoot() => !isReloading && timeSinceLastShot > 1f / (fireRate / 60f);

    void Start() {
        m_anim = GetComponent<Animator>();
    }

    void Update() {
        // Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Shoot();
        }
        //reload
        if (Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(Reload());
        }
        timeSinceLastShot += Time.deltaTime;
        //Debug.Log(timeSinceLastShot.ToString());
        ammoText.text = "Ammo: " + currentAmmo.ToString();
    }

    void Shoot() {
        if (currentAmmo > 0) {
            if (CanShoot()) {
                m_anim.SetTrigger("Shoot");
                Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, maxDistance);
                Target target = hit.transform.GetComponent<Target>();
                if (target != null) {
                    target.health = target.health - damage;
                }
                currentAmmo--;
                timeSinceLastShot = 0f;
            }
        }
    }

    private IEnumerator Reload() {
        isReloading = true;
        m_anim.SetTrigger("Reload");
        yield return new WaitForSecondsRealtime(reloadTime);

        currentAmmo = magSize;
        isReloading = false;
    }
}
