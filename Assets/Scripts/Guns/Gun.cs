using System.Collections;
using UnityEngine;

public enum FireMode
{
   Single,
   SemiAuto,
   FullAuto
}

public class Gun : MonoBehaviour
{
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 5f;

    public FireMode fireMode;
    public AudioSource gunAudio;
    public Camera playerCamera; //Access to Player's Camera

    protected float nextFireTime;

    public Animator animator;

    //Ammo and Reloading
    public int magzineSize = 7;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    public bool isReloading;
    public bool autoReload = true;

    protected void Start()
    {
        currentAmmo = magzineSize;
    }


    protected virtual void Update()
    {
        if(fireMode == FireMode.FullAuto)
        {
            if (Input.GetMouseButton(0))
            {
                TryShoot();
            } 
        }

        else if (fireMode == FireMode.SemiAuto)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryShoot();
            }
        }

        else if(fireMode == FireMode.Single) 
        {
            if(Input.GetMouseButtonDown(0))
            {
                TryShoot();
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
    }

    protected virtual void TryShoot()
    {
        if(isReloading)
            return;

        if (currentAmmo <= 0)
        {
            if (autoReload)
            {
                Reload();
            }

            return;
        }

        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + 1f / fireRate;

        Shoot();
    }


    protected virtual void Shoot()
    {
        currentAmmo--;

        gunAudio.PlayOneShot(gunAudio.clip);

        animator.Play("PistolShoot", 0, 0f);

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if(Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.TryGetComponent<EnemyAI>(out EnemyAI enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    protected virtual void Reload()
    {
        if (isReloading) return;

        if (currentAmmo == magzineSize) return;

        StartCoroutine(ReloadCoroutine());
    }

    protected IEnumerator ReloadCoroutine() 
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magzineSize;

        isReloading = false;
    }
}
