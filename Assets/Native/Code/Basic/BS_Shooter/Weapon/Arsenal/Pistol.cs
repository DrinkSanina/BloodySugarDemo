using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : AWeapon, IWPN_Reloadable, IWPN_HasSecondaryFire
{
    

    [field: SerializeField]
    public int ClipSize { get; set; }
    
    [field: SerializeField]
    public int MaxAmmo { get; set; }

    public override void PrimaryFire(Vector3 rayOrigin, Vector3 rayDirection)
    {
        if(canShoot)
        {
            Vector3 targetPoint;
            RaycastHit hit = CastRay(rayOrigin, rayDirection, out targetPoint);
            
            if(hit.collider != null)
            {
                HitBleed hb = hit.transform.GetComponent<HitBleed>();
                BasicEntityStatsComponent target = hit.transform.GetComponent<BasicEntityStatsComponent>();
                BulletImpactSound bis = hit.transform.GetComponent<BulletImpactSound>();

                if (target != null)
                {
                    target.stats.RecieveDamage(gunDamage, DamageSource.player);
                }
                if(bis != null)
                {
                    bis.PlayRandomHitImpact();
                }
                if(hb != null)
                {
                    hb.DisplayBloodFX(targetPoint);
                }
            }
            
        }

    }

    public void SecondaryFire()
    {
        
    }

    void Start()
    {
        base.Start();
    }

}
