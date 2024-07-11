using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : Singleton<Statistics>
{
    private int wavesSurvived = 0;
    private int demonsKilled = 0;
    private float outcomingDamage = 0;
    private float incomingHealing = 0;
    private float incomingDamageFromDemons = 0;
    private float incomingDamageFromTraps = 0;
    private float incomingDamageFromEffects = 0;
    private int powerupsCollected = 0;

    public Action StatisticsChanged;

    public BasicEntityStatsComponent player;

    private void Start()
    {
        player.stats.RecievedDamage += (float damage, DamageSource source) =>
        {
            switch (source)
            {
                case DamageSource.effect: incomingDamageFromEffects += damage; break;
                case DamageSource.trap: incomingDamageFromTraps += damage; break;
                case DamageSource.enemy: incomingDamageFromDemons += damage; break;
            }
        };
        
        player.stats.RecievedHealing += (float heal) => { incomingHealing += heal; };

    }

    public int WavesSurvived
    {
        get
        {
            return wavesSurvived;
        }
        set
        {
            wavesSurvived = value;
            StatisticsChanged?.Invoke();
        }
    }
    public int DemonsKilled
    {
        get
        {
            return demonsKilled;
        }
        set
        {
            demonsKilled= value;
            StatisticsChanged?.Invoke();
        }
    }
    public float OutcomingDamage
    {
        get
        {
            return outcomingDamage;
        }
        set
        {
            outcomingDamage = value;
            StatisticsChanged?.Invoke();
        }
    }
    public float IncomingHealing
    {
        get
        {
            return incomingHealing;
        }
        set
        {
            incomingHealing = value;
            StatisticsChanged?.Invoke();
        }
    }
    public float IncomingDamageFromDemons
    {
        get
        {
            return incomingDamageFromDemons;
        }
        set
        {
            incomingDamageFromDemons = value;
            StatisticsChanged?.Invoke();
        }
    }
    public float IncomingDamageFromTraps
    {
        get
        {
            return incomingDamageFromTraps;
        }
        set
        {
            incomingDamageFromTraps = value;
            StatisticsChanged?.Invoke();
        }
    }
    public float IncomingDamageFromEffects
    {
        get
        {
            return incomingDamageFromEffects;
        }
        set
        {
            incomingDamageFromEffects = value;
            StatisticsChanged?.Invoke();
        }
    }
    public int PowerupsCollected
    {
        get
        {
            return powerupsCollected;
        }
        set
        {
            powerupsCollected = value;
            StatisticsChanged?.Invoke();
        }
    }


}
