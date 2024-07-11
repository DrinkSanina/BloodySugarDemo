using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : AWeapon, IWPN_Reloadable, IWPN_HasSecondaryFire
{
    [field: SerializeField]
    public int ClipSize { get; set; }
    
    [field: SerializeField]
    public int MaxAmmo { get; set; }

    [field: SerializeField]
    public float ShotgunSpread { get; set; }

    [field: SerializeField]
    public int MaxPellets { get; set; }

    [field: SerializeField]
    public int MinPellets { get; set; }

    public override void PrimaryFire(Vector3 rayOrigin, Vector3 rayDirection)
    {
        if(canShoot)
        {
            int pellets = Random.Range(MinPellets, MaxPellets);

            Vector3[] targetPoints = null;
            RaycastHit[] hits = CastMultipleRays(rayOrigin, rayDirection, out targetPoints, pellets, ShotgunSpread);
            
            for(int i =0; i < pellets; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.collider != null)
                {
                    HitBleed hb = hit.transform.GetComponent<HitBleed>();
                    BasicEntityStatsComponent target = hit.transform.GetComponent<BasicEntityStatsComponent>();
                    BulletImpactSound bis = hit.transform.GetComponent<BulletImpactSound>();

                    if (target != null)
                    {
                        target.stats.RecieveDamage(gunDamage, DamageSource.player);
                    }
                    if (bis != null)
                    {
                        bis.PlayRandomHitImpact();
                    }
                    if (hb != null)
                    {
                        hb.DisplayBloodFX(targetPoints[i]);
                    }
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
