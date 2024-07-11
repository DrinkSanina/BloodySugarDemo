using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����-�������, ����������� �������� ������ � ������� ������� ������� ��������
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
    /// ���������� ����� ������� � ������� HH:MM:SS
    /// </summary>
    public string DurationInHMS => Utils.SecondsToHMS(secondsLeft);


    public string EffectName => effect.EffectName;
}