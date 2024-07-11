using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Entity/Effect")]
public class EntityEffect : ScriptableObject
{
    public string EffectName;
    public string Description;

    public float durationSeconds;
    public List<EntityEffectComponent> effects;
}
