using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ � ����������� ��������� �������
/// </summary>
public class EffectInstigator : MonoBehaviour
{
    /// <summary>
    /// ������ ��������, ������� ����� ��������
    /// </summary>
    public List<EntityEffect> possibleEffects;

    /// <summary>
    /// ����������� ������
    /// </summary>
    /// <param name="target">���� ��������� �������</param>
    /// <param name="effect">��������� ������</param>
    public void CastEffect(EffectReciever target, EntityEffect effect)
    {
        target.AttachEffect(effect);
    }

    public void CastTimelessEffect(EffectReciever target, EntityEffect effect)
    {
        target.AttachTimelessEffect(effect);
    }

}
