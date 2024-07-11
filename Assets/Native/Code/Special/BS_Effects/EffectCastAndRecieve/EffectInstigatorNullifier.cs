using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Особый вид инстигатора эффектов, который помимо их наложения способен их снимать
/// </summary>
public class EffectInstigatorNullifier : EffectInstigator
{
    public List<EntityEffect> NullifyEffects;

    public void NullifyAllChosenEffects(EffectReciever target)
    {
        if(target != null)
        {
            foreach (EntityEffect e in NullifyEffects)
            {
                target.NullifyEffect(e);
            }
        }
        
    }

    public void NullifyAllEffects(EffectReciever target)
    {
        target.NullifyAllEffects();
    }
}
