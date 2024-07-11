using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum DamageSource
{ 
    player,
    enemy,
    trap,
    effect,
    none
}


[Serializable]
public class BasicEntityStats
{
    //Глобальные параметры. Признаки:
    // 1. Меняются любым образом (любой механикой геймплея)
    // 2. Имеют максимальное значение
    // 3. Не имеют дефолтных значений
    public float HP = 100.0f;
    public float maxHP = 100.0f;

    //Локальные параметры. Признаки:
    // 1. Меняются только эффектами
    // 2. Имеют одно дефолтное значение
    public float movementSpeedPercent = 1.0f;
    public float accuracyPercent = 1.0f;
    public float damagePercent = 1.0f;
    public float healPerSecond = 0.0f;
    public float incomingDamagePerSecond = 0.0f;
    public float damageResistancePercent = 0.0f;
    public bool isDead;

    public Action EntityIsDead;
    public Action HealthChanged;

    public Action<float, DamageSource> RecievedDamage;
    public Action<float> RecievedHealing;

    public void ResetStats()
    {
        HP = 100.0f;
        maxHP = 100.0f;
        movementSpeedPercent = 1.0f;
        accuracyPercent = 1.0f;
        damagePercent = 1.0f;
        healPerSecond = 0.0f;
        incomingDamagePerSecond = 0.0f;
        damageResistancePercent = 0.0f;
        isDead = false;
    }

    public BasicEntityStats(BasicEntityStats copy)
    {
        this.HP = copy.HP;
        this.maxHP = copy.maxHP;
        this.movementSpeedPercent = copy.movementSpeedPercent;
        this.accuracyPercent = copy.accuracyPercent;
        this.damagePercent = copy.damagePercent;
        this.healPerSecond = copy.healPerSecond;
        this.incomingDamagePerSecond = copy.incomingDamagePerSecond;
        this.damageResistancePercent = copy.damageResistancePercent;
        this.isDead = copy.isDead;
    }

    public double RecieveHealing(float value)
    {
        if (!isDead)
        {
            if (HP + value <= maxHP)
            {
                HP += value;
                HealthChanged?.Invoke();
                RecievedHealing?.Invoke(value);
                return value;
            }
            else
            {
                float safevalue = maxHP - HP;
                HP += safevalue;
                HealthChanged?.Invoke();
                RecievedHealing?.Invoke(safevalue);
                return safevalue;
            }
        }
        else
            return 0;

        
        
    }
    public double RecieveDamage(float value, DamageSource source)
    {
        if (!isDead)
        {
            if (HP - value > 0)
            {
                HP -= value;
                HealthChanged?.Invoke();
                RecievedDamage?.Invoke(value, source);
                return value;
            }
            else
            {
                float safevalue = HP;
                HP = 0;
                isDead = true;
                EntityIsDead?.Invoke();
                HealthChanged?.Invoke();
                RecievedDamage?.Invoke(safevalue, source);
                return safevalue;
            }
        }
        else
            return 0;
        
    }
}


