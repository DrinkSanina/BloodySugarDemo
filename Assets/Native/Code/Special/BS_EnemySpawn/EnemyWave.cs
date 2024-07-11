using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Enemies/Wave")]
public class EnemyWave : ScriptableObject
{
    public List<GameObject> PossibleEnemies = new List<GameObject>();
    public int enemyQuantity;
    public int maximumEnemiesAtOnce = 15;
    public float durationSeconds;
}
