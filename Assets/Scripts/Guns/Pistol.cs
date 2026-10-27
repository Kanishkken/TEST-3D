using UnityEngine;

public class Pistol : Gun
{
    [SerializeField]protected ParticleSystem muzzleFlash;

    private void Awake()
    {
        fireMode = FireMode.SemiAuto;
        damage = 25f;
        fireRate = 7f;
        range = 100f;
    }

    protected override void Shoot()
    {
        base.Shoot();

        muzzleFlash.Play();    
    }
}
