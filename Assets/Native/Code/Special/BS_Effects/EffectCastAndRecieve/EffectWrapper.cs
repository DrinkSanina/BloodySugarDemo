using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ласс-обертка, позвол€ющий получить доступ к времени эффекта снаружи корутина
/// </summary>
[Serializable]
public class EffectWrapper
{
    public EntityEffect effect;
    public float secondsLeft;
    public IEnumerator attachedCoroutine;

    public EntityEffect Effect => effect;

    public EffectWrapper(EntityEffect effect)
    {
        this.effect = effect;
        secondsLeft = effect.durationSeconds;
    }

    /// <summary>
    /// ќставшеес€ врем€ эффекта в формате HH:MM:SS
    /// </summary>
    public string DurationInHMS => Utils.SecondsToHMS(secondsLeft);


    public string EffectName => effect.EffectName;
}