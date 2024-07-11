using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Хранит и накладывает возможные эффекты
/// </summary>
public class EffectInstigator : MonoBehaviour
{
    /// <summary>
    /// Список эффектов, которые может наложить
    /// </summary>
    public List<EntityEffect> possibleEffects;

    /// <summary>
    /// Накладывает эффект
    /// </summary>
    /// <param name="target">Цель наложения эффекта</param>
    /// <param name="effect">Выбранный эффект</param>
    public void CastEffect(EffectReciever target, EntityEffect effect)
    {
        target.AttachEffect(effect);
    }

    public void CastTimelessEffect(EffectReciever target, EntityEffect effect)
    {
        target.AttachTimelessEffect(effect);
    }

}
