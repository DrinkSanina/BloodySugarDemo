using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ��� ����������� ��������, ������� ������ �� ��������� �������� �� �������
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
