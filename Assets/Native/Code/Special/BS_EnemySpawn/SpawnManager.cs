using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTimeStamp
{
    public List<float> timeStamps = new List<float>();
}

public class SpawnManager : MonoBehaviour
{
    public List<EnemyWave> EnemyWaves = new List<EnemyWave>();
    private List<SpawnArea> SpawnAreas = new List<SpawnArea>();

    private float spawningTime = 0f;
    private float currentWaveTime = 0f;
    private int currentWaveNumber = 0;
    
    private int passedTimestamps = 0;
    private int previousPassedTimestamps = 0;

    private List<WaveTimeStamp> WaveTimeStamps = new List<WaveTimeStamp>();

    public int killCount = 0;
    public System.Action spawnedCreatureDied;
    
    public float damageGot = 0.0f;
    public System.Action spawnedCreatureGotDamaged;

    public float damageDealt = 0.0f;
    public System.Action spawnedCreatureDealtDamage;

    void Start()
    {
        SpawnAreas.AddRange(FindObjectsByType<SpawnArea>(FindObjectsSortMode.InstanceID).Where(x=>x.type == SpawnAreaType.enemy));
        foreach(EnemyWave ew in EnemyWaves)
        {
            WaveTimeStamps.Add(CalculateRandomTimeStamps(ew));
        }

    }

    void Update()
    {
        if(currentWaveNumber >= EnemyWaves.Count)
        {
            //this.enabled = false;
            return;
        }

        spawningTime += Time.deltaTime;
        currentWaveTime += Time.deltaTime;

        passedTimestamps = 0;
        for(int i = 0; i < WaveTimeStamps[currentWaveNumber].timeStamps.Count; i++)
        {
            if(currentWaveTime >= WaveTimeStamps[currentWaveNumber].timeStamps[i])
            {
                passedTimestamps++;
            }
        }

        if(passedTimestamps > previousPassedTimestamps)
        {
            GameObject enemy = Instantiate(
                PickRandomEnemyInWave(EnemyWaves[currentWaveNumber]),
                SpawnAreas[Random.Range(0, SpawnAreas.Count)].RandomPointInBounds(1.21f),
                Quaternion.identity);

            enemy.GetComponent<BasicEntityStatsComponent>().stats.EntityIsDead += KillCount;

            previousPassedTimestamps = passedTimestamps;
        }

        if (currentWaveTime >= EnemyWaves[currentWaveNumber].durationSeconds)
        {
            currentWaveNumber++;
            currentWaveTime = 0;
        }
    }

    /// <summary>
    /// –ассчитать врем€ по€влени€ врагов по какому-либо закону.
    /// ѕока используетс€ простое равномерное распределение на отрезке от 0 до durationSeconds
    /// </summary>
    public WaveTimeStamp CalculateRandomTimeStamps(EnemyWave wave)
    {
        WaveTimeStamp wts = new WaveTimeStamp();
        System.Random r = new System.Random();
        for (int i = 0; i < wave.enemyQuantity; i++)
        {
            float randomPoint = (float)r.NextDouble() * (wave.durationSeconds - 0) + 0;
            wts.timeStamps.Add(randomPoint);
        }

        wts.timeStamps.Sort();
        return wts;
    }

    public GameObject PickRandomEnemyInWave(EnemyWave wave)
    {
        if (wave.PossibleEnemies.Count == 0)
        {
            Debug.Log($"ѕроверь волну {this.name}. “ут нет PossibleEnemies");
            return null;
        }
        else if (wave.PossibleEnemies.Count == 1)
        {
            return wave.PossibleEnemies[0];
        }
        else
        {
            return wave.PossibleEnemies[Random.Range(0, wave.PossibleEnemies.Count)];
        }
    }

    public void KillCount()
    {
        killCount++;
        spawnedCreatureDied?.Invoke();
    }
}
