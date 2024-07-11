using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectInstigator))]
public class EffectCastingProjectile : Projectile
{  
    [HideInInspector]
    public EffectInstigator instigator;

    void OnEnable()
    {
        instigator = GetComponent<EffectInstigator>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(instigator.possibleEffects.Count > 0)
        {
            EffectReciever ef = collision.collider.gameObject.GetComponent<EffectReciever>();
            if (ef != null)
            {
                ef.AttachEffect(instigator.possibleEffects[0]);
            }
        }
        

        BasicEntityStatsComponent besc = collision.collider.gameObject.GetComponent<BasicEntityStatsComponent>();
        if(besc != null)
        {
            besc.stats.RecieveDamage(damage, DamageSource.player);
        }

        //Временная, до момента создания урона
        Destroy(gameObject);

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        //if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }
}
