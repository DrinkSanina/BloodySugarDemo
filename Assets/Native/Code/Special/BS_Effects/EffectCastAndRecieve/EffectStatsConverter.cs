using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �������� ������� �������� �� BasicEntityStats, ��������������� �� � ������ ���������� ��������
/// � ������ ����������� ����������� ����������� ��������
/// </summary>
[RequireComponent(typeof(EffectReciever))]
[RequireComponent(typeof(BasicEntityStatsComponent))]
public class EffectStatsConverter : MonoBehaviour, IEffectListener
{
    [HideInInspector]
    public EffectReciever EffectReciever { get; set; }

    [HideInInspector]
    public BasicEntityStatsComponent StatsComponent { get; set; }

    private BasicEntityStats defaults;

    private void Start()
    {
        EffectReciever = GetComponent<EffectReciever>();
        StatsComponent = GetComponent<BasicEntityStatsComponent>();

        defaults = new BasicEntityStats(StatsComponent.stats);

        if (EffectReciever != null)
        {
            EffectReciever.EffectAdded += OnEffectAdded;
            EffectReciever.EffectRemoved += OnEffectRemoved;
            EffectReciever.EffectTicked += OnEffectTicked;
        }
    }

    void Awake()
    {
        if (EffectReciever != null)
        {
            EffectReciever.EffectAdded += OnEffectAdded;
            EffectReciever.EffectRemoved += OnEffectRemoved;
            EffectReciever.EffectTicked += OnEffectTicked;
        }
    }

    public void OnEffectAdded()
    {
        CalculateStats();
    }

    public void OnEffectRemoved()
    {
        CalculateStats();
    }


    private void CalculateStats()
    {
        //�������� ���������������� ����� �������� ���� ���������� ��������� ������
        Dictionary<Affect, float> changing_stats = new Dictionary<Affect, float>();

        foreach (EffectWrapper ew in EffectReciever.castedEffects)
        {
            foreach (EntityEffectComponent effectComponent in ew.Effect.effects)
            {
                //���� �������� ������� �� ���������� � ������ - �������� � ��������� ���������
                if(!changing_stats.ContainsKey(effectComponent.affect))
                {
                    changing_stats.Add(effectComponent.affect, effectComponent.change_value);
                }
                else //���� ���������� - ��������� �������� �������
                {
                    changing_stats[effectComponent.affect] += effectComponent.change_value;
                }
            }
        }

        //������ ������
        Array all_affects = Enum.GetValues(typeof(Affect));
        
        //List<Affect> unchanging_stats = new List<Affect>();
        //foreach(Affect affect in all_affects)
        //{
        //    if (!changing_stats.ContainsKey(affect))
        //        unchanging_stats.Add(affect);
        //}

        //��� �������� �������� ����������, ����� ���, ������� ������ ���������� � �������
        foreach(Affect a in all_affects)
        {
            float subhp = StatsComponent.stats.HP;

            CorrespondingStat(StatsComponent.stats, a) = CorrespondingStat(defaults, a);

            StatsComponent.stats.HP = subhp;
        }

        //�� ���� ���������� ��������� �������� �������� �� ��������� �� ������� changing_stats
        foreach(KeyValuePair<Affect, float> effect in changing_stats)
        {
            CorrespondingStat(StatsComponent.stats, effect.Key) += effect.Value;
        }

        EffectReciever.StatsChanged?.Invoke();
    }

    private void OnEffectTicked(TickingEntityEffect tef)
    {
        foreach (EntityEffectComponent effectComponent in tef.effects)
        {
            //������ ��� ����� ����������� ����� �������� ������� �� ������ �������� ��������� �����
            switch(effectComponent.affect)
            {
                case Affect.TickingDamage:
                    StatsComponent.stats.RecieveDamage(effectComponent.change_value, DamageSource.effect);
                    break;
            }
        }
        EffectReciever.StatsChanged?.Invoke();
    }


    private ref float CorrespondingStat(BasicEntityStats stats, Affect affect)
    {
        switch (affect)
        {
            case Affect.MaxHP_value:
                return ref stats.maxHP;

            case Affect.MoveSpeed_percentage:
                return ref stats.movementSpeedPercent;

            case Affect.Accuracy_percentage:
                return ref stats.accuracyPercent;

            case Affect.Damage_percentage:
                return ref stats.damagePercent;

            case Affect.DamageOverTime:
                return ref stats.incomingDamagePerSecond;

            case Affect.Heal:
                return ref stats.healPerSecond;

        }

        return ref stats.HP;
    }
}
