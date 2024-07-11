using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������ ���������� ��������, � ����� �������� �� ����� �� ��������
/// </summary>
public class EffectReciever : MonoBehaviour
{
    /// <summary>
    /// ������ ���� ���������� �������� � �������� �������
    /// </summary>
    [SerializeField]
    public List<EffectWrapper> castedEffects = new List<EffectWrapper>();

    /// <summary>
    /// ������� ��������� �������
    /// </summary>
    public Action EffectAdded;

    /// <summary>
    /// ������� ������ �������
    /// </summary>
    public Action EffectRemoved;

    public Action StatsChanged;

    public Action<TickingEntityEffect> EffectTicked;

    public void AttachEffect(EntityEffect effect)
    {
        EffectWrapper effectAlreadyCasted = CheckEffectAlreadyCasted(effect);

        if(effectAlreadyCasted != null)
        {
            effectAlreadyCasted.secondsLeft = effect.durationSeconds;
        }
        else
        {
            EffectWrapper ew = new EffectWrapper(effect);

            if (effect is TickingEntityEffect)
                ew.attachedCoroutine = TickingAffection(ew);
            else
                ew.attachedCoroutine = Affection(ew);

            StartCoroutine(ew.attachedCoroutine);
            castedEffects.Add(ew);
            EffectAdded?.Invoke();
        }

    }

    /// <summary>
    /// ������, ������� �� ��������� �������� ��� ���������. ������ ����� ������� ����������
    /// </summary>
    /// <param name="effect"></param>
    public void AttachTimelessEffect(EntityEffect effect)
    {
        EffectWrapper effectAlreadyCasted = CheckEffectAlreadyCasted(effect);
        if(effectAlreadyCasted == null)
        {
            EffectWrapper ew = new EffectWrapper(effect);
            ew.secondsLeft = -1; //-1 - ��� �����������, ��� ������ �����������

            if (effect is TickingEntityEffect)
            {
                ew.attachedCoroutine = TimelessTickingAffection(ew);
                StartCoroutine(ew.attachedCoroutine);
            }

            castedEffects.Add(ew);
            EffectAdded?.Invoke();
        }
    }

    public EffectWrapper CheckEffectAlreadyCasted(EntityEffect effect)
    {
        EffectWrapper effectAlreadyCasted = null;
        foreach (EffectWrapper effectWrapper in castedEffects)
        {
            if (effectWrapper.effect == effect)
            {
                effectAlreadyCasted = effectWrapper;
            }
        }

        return effectAlreadyCasted;
    }

    public void NullifyEffect(EntityEffect effect)
    {
        foreach(EffectWrapper ew in castedEffects)
        {
            if(ew.effect == effect)
            {
                if (ew.attachedCoroutine != null)
                    StopCoroutine(ew.attachedCoroutine);

                castedEffects.Remove(ew);
                EffectRemoved?.Invoke();
                break;
            }
        }
    }

    public void NullifyAllEffects()
    {
        foreach (EffectWrapper ew in castedEffects)
        {
            if(ew.attachedCoroutine != null)
                StopCoroutine(ew.attachedCoroutine);
        }

        castedEffects.Clear();

        EffectRemoved?.Invoke();
    }

    IEnumerator Affection(EffectWrapper effectWrapper)
    {
        while (effectWrapper.secondsLeft > 0.0f)
        {
            effectWrapper.secondsLeft -= 1.0f;
            yield return new WaitForSeconds(1.0f);
        }

        castedEffects.Remove(effectWrapper);
        EffectRemoved?.Invoke();
        yield break;
    }

    /// <summary>
    /// ������� ���������� �� ������������ Affection ���, ��� ������������ ������ ������� ���� �������
    /// � ���������, ���������� �� "���" �������
    /// </summary>
    /// <param name="effectWrapper"></param>
    /// <returns></returns>
    IEnumerator TickingAffection(EffectWrapper effectWrapper)
    {
        TickingEntityEffect tef = (TickingEntityEffect) effectWrapper.effect;
        float tickSecondsPassed = 0;
        EffectTicked(tef);
        tickSecondsPassed += 0.1f;
        while (effectWrapper.secondsLeft > 0.0f)
        {
            effectWrapper.secondsLeft -= 0.1f;
            tickSecondsPassed += 0.1f;

            if (tickSecondsPassed >= tef.tickseconds)
            {
                EffectTicked(tef);
                tickSecondsPassed = 0;
            }

            yield return new WaitForSeconds(0.1f);
        }

        castedEffects.Remove(effectWrapper);
        EffectRemoved?.Invoke();
        yield break;
    }
    
    IEnumerator TimelessTickingAffection(EffectWrapper effectWrapper)
    {
        TickingEntityEffect tef = (TickingEntityEffect)effectWrapper.effect;
        float tickSecondsPassed = 0;
        EffectTicked(tef);
        tickSecondsPassed += 0.1f;
        while (true)
        {
            tickSecondsPassed += 0.1f;
            if (tickSecondsPassed >= tef.tickseconds)
            {
                EffectTicked(tef);
                tickSecondsPassed = 0;
            }
            

            yield return new WaitForSeconds(0.1f);
        }
    }
}
