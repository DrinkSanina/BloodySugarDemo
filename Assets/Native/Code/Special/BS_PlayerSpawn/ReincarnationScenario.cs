using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ReincarnationScenario : MonoBehaviour
{
    public BasicEntityStatsComponent playerStats;
    private List<SpawnArea> spawnAreas = new List<SpawnArea>();
    public bool kd_spawn = false;

    public void Start()
    {
        spawnAreas.AddRange(FindObjectsByType<SpawnArea>(FindObjectsSortMode.InstanceID).Where(x => x.type == SpawnAreaType.player));

        playerStats.stats.EntityIsDead += OnPlayerDeath;

        if (kd_spawn)
            InvokeRepeating("DebugSpawn", 0, 2f);
    }

    private void OnPlayerDeath()
    {
        //playerStats.transform.gameObject.SetActive(false);

        //playerStats.transform.position = spawnAreas[Random.Range(0, spawnAreas.Count)].RandomPointInBounds();
        var controller = playerStats.gameObject.GetComponent<CharacterController>();
        Vector3 diff = transform.TransformDirection(spawnAreas[Random.Range(0, spawnAreas.Count)].RandomPointInBounds() - controller.gameObject.transform.position);
        controller.Move(diff);

        //playerStats.transform.gameObject.SetActive(true);

        playerStats.stats.ResetStats();
        playerStats.stats.isDead = false;
        playerStats.GetComponent<EffectReciever>().NullifyAllEffects();
    }

    public void DebugSpawn()
    {
        playerStats.transform.gameObject.SetActive(false);

        playerStats.transform.position = spawnAreas[Random.Range(0, spawnAreas.Count)].RandomPointInBounds();

        playerStats.transform.gameObject.SetActive(true);

        playerStats.stats.ResetStats();
        playerStats.stats.isDead = false;
        playerStats.GetComponent<EffectReciever>().NullifyAllEffects();
    }
}
