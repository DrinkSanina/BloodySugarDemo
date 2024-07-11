using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_projectile : AWeapon, IWPN_Projectile
{
    [field: SerializeField]
    public GameObject Bullet { get; set; }
    
    [field: SerializeField]
    public float ShootForce { get; set; }
    [field: SerializeField]
    public float UpwardForce { get; set; }

    public override void PrimaryFire(Vector3 rayOrigin, Vector3 rayDirection)
    {
        if (canShoot)
        {
            Debug.Log($"Primary Fire!");

            Vector3 targetPoint;

            RaycastHit hit = CastRay(rayOrigin, rayDirection, out targetPoint);

            ((IWPN_Projectile)this).SpawnProjectile(gunBarrel.position, targetPoint-gunBarrel.position, rayDirection);
        }
    }

    void Start()
    {
        base.Start();
    }

}
