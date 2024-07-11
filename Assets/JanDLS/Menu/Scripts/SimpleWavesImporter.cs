using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleWavesImporter : MonoBehaviour
{
    public float secondsToStart = 3f;
    public ArenaSettings arenaSettings;

    public GameObject JijaEnemy;
    public GameObject CaramelEnemy;
    public GameObject ChocoEnemy;

    private List<GameObject> enabledEnemies;

    private bool WavesStart;

    private float spawningTime;
    private float currentWaveTime;
    private int passedTimeStamps;

    private List<WaveTimeStamp> WaveTimeStamps;
    private int currentWaveNumber;
    private int previousPassedTimestamps;

    public int CurrentWaveNumber => currentWaveNumber;
    public int MaxEnemies => arenaSettings.enemies;
    public int MaxWaves => arenaSettings.waves;

    private List<SpawnArea> SpawnAreas = new List<SpawnArea>();


    public System.Action<int> waveChanged;
    public System.Action WavesEnded;
    public System.Action WavesStarted;
    public System.Action<int> CountdownChanged;

    private int currentEnemiesLeft;
    public int EnemiesLeft => currentEnemiesLeft;


    private void Start()
    {
        SpawnAreas.AddRange(FindObjectsByType<SpawnArea>(FindObjectsSortMode.InstanceID).Where(x => x.type == SpawnAreaType.enemy));
        StartCoroutine(BeginSpawnDelay());

    }

    IEnumerator BeginSpawnDelay()
    {
        while(secondsToStart > 0)
        {
            CountdownChanged?.Invoke((int)Mathf.Round(secondsToStart));
            secondsToStart--;
            yield return new WaitForSeconds(1f);
        }

        arenaSettings = GameObject.FindAnyObjectByType<ArenaSettings>();

        WaveTimeStamps = new List<WaveTimeStamp>();
        for(int i = 0; i < arenaSettings.waves; i++)
        {
            WaveTimeStamps.Add(CalculateRandomTimeStamps());
        }

        enabledEnemies = new List<GameObject>();

        if (arenaSettings.soda)
            enabledEnemies.Add(JijaEnemy);

        if (arenaSettings.choco)
            enabledEnemies.Add(ChocoEnemy);

        if (arenaSettings.caramel)
            enabledEnemies.Add(CaramelEnemy);

        if(enabledEnemies.Count != 0)
        {
            WavesStart = true;
            currentEnemiesLeft = arenaSettings.enemies;
            WavesStarted?.Invoke();
        }

        yield break;
    }

    private void Update()
    {
        if (!WavesStart)
            return;

        spawningTime += Time.deltaTime;
        currentWaveTime += Time.deltaTime;

        passedTimeStamps = 0;



        for (int i = 0; i < WaveTimeStamps[currentWaveNumber].timeStamps.Count; i++)
        {
            if (currentWaveTime >= WaveTimeStamps[currentWaveNumber].timeStamps[i])
            {
                passedTimeStamps++;
            }
        }

        if (passedTimeStamps > previousPassedTimestamps)
        {
            GameObject enemy = Instantiate(
                enabledEnemies[Random.Range(0, enabledEnemies.Count)],
                SpawnAreas[Random.Range(0, SpawnAreas.Count)].RandomPointInBounds(1.21f),
                Quaternion.identity);

            enemy.GetComponent<BasicEntityStatsComponent>().stats.EntityIsDead += () => { Statistics.instance.DemonsKilled++; currentEnemiesLeft--; };
            enemy.GetComponent<BasicEntityStatsComponent>().stats.RecievedDamage += (float damage, DamageSource source) => { Statistics.instance.OutcomingDamage += damage; };
            

            previousPassedTimestamps = passedTimeStamps;
        }

        if (currentEnemiesLeft <= 0)
        {
            currentWaveNumber++;
            if (CurrentWaveNumber < MaxWaves)
            {
                currentWaveTime = 0.0f;
                currentEnemiesLeft = arenaSettings.enemies;
                waveChanged?.Invoke(currentWaveNumber);
                previousPassedTimestamps = 0;
                Statistics.instance.WavesSurvived = currentWaveNumber;
            }
            else
            {
                WavesEnded?.Invoke();
                WavesStart = false;
                currentWaveNumber = MaxWaves;
            }
            
        }
    }

    private WaveTimeStamp CalculateRandomTimeStamps()
    {
        WaveTimeStamp wts = new WaveTimeStamp();
        System.Random r = new System.Random();
        for (int i = 0; i < arenaSettings.enemies; i++)
        {
            float randomPoint = (float)r.NextDouble() * (arenaSettings.waveDuration - 0) + 0;
            wts.timeStamps.Add(randomPoint);
        }

        wts.timeStamps.Sort();
        return wts;
    }
}
