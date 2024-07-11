using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityEffectComponent
{
    public Affect affect;
    
    [Tooltip("¬еличина изменени€ значени€. ƒобавл€етс€ к стату при расчете. ќтрицательные значени€ означают уменьшение стата")]
    public float change_value;
}
