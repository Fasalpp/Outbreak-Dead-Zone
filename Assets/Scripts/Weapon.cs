using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;
    public float range = 100f;
    public float damage = 10f;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public GameObject groundHitEffect;
    public float timetoShot = 0.2f;

    public Ammo ammo;
    public AmmoType ammoType;
    bool canShoot = true;
    Animator aR;
    void Start()
    {
        aR = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            StartCoroutine( Shoot());
            
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (aR != null)
            {
                aR.SetTrigger("Reload");
            }
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (ammo.GetCurrentAmmo(ammoType) > 0)
        {
            MuzzleFlash();
            if (aR != null)
            {
                aR.SetTrigger("Shoot");
            }
            ammo.ReduceCurrentAmmo(ammoType);
            Debug.Log("Current Ammo : " + ammo.GetCurrentAmmo(ammoType));
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                //Debug.Log("Hit : " + hit.collider.name);
                //HitImpact(hit);
                EnemyHealth enemyHealth = hit.collider.transform.root.GetComponent<EnemyHealth>();
                EnemyAI enemyAI = hit.transform.GetComponent<EnemyAI>();
                if (enemyHealth != null)
                {
                    if (hit.transform.CompareTag("Head"))
                    {
                        enemyHealth.TakeDamage(damage * 2, "isHeadShot", transform.position);
                    }
                    else
                    {
                        enemyHealth.TakeDamage(damage,"isBodyShot", transform.position);
                    }
                    if (enemyAI != null)
                    {
                        enemyAI.OnDamageTaken(hit.point);
                    }
                }
                else
                {
                    
                }
            }
        }
        yield return new WaitForSeconds(timetoShot);
        canShoot = true;
    }
    void MuzzleFlash()
    {
        muzzleFlash.Play();
    }
    void HitImpact(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Enemy"))
        {
            GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1);
        }
        else
        {
            GameObject impact = Instantiate(groundHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1);
        }
    }
}
