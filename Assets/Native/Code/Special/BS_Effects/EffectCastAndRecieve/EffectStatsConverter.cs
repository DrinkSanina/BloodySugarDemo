using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Получает базовые значения от BasicEntityStats, перерасчитывает их с учетом наложенных эффектов
/// и выдает необходимым подсистемам высчитанные значения
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
        //Получить просуммированные новые значения всех изменяемых эффектами статов
        Dictionary<Affect, float> changing_stats = new Dictionary<Affect, float>();

        foreach (EffectWrapper ew in EffectReciever.castedEffects)
        {
            foreach (EntityEffectComponent effectComponent in ew.Effect.effects)
            {
                //Если значение аффекта не содержится в списке - добавить с величиной изменения
                if(!changing_stats.ContainsKey(effectComponent.affect))
                {
                    changing_stats.Add(effectComponent.affect, effectComponent.change_value);
                }
                else //Если содержится - прибавить величину аффекта
                {
                    changing_stats[effectComponent.affect] += effectComponent.change_value;
                }
            }
        }

        //Список статов
        Array all_affects = Enum.GetValues(typeof(Affect));
        
        //List<Affect> unchanging_stats = new List<Affect>();
        //foreach(Affect affect in all_affects)
        //{
        //    if (!changing_stats.ContainsKey(affect))
        //        unchanging_stats.Add(affect);
        //}

        //Все значения заменить дефолтными, кроме тех, которые нельзя возвращать к базовым
        foreach(Affect a in all_affects)
        {
            float subhp = StatsComponent.stats.HP;

            CorrespondingStat(StatsComponent.stats, a) = CorrespondingStat(defaults, a);

            StatsComponent.stats.HP = subhp;
        }

        //Ко всем измененным значениям добавить величину их изменения из словаря changing_stats
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
            //Каждый раз когда добавляются новые тикающие эффекты их логику придется прописать здесь
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
