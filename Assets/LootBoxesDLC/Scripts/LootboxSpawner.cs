using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LootboxSpawner : MonoBehaviour
{
    public Lootbox lootbox;
    public float spawnEveryNSeconds = 1f;
    
    private List<SpawnArea> LootSpawnAreas = new List<SpawnArea>();

    private float timePassed;

    void Start()
    {
        LootSpawnAreas.AddRange(FindObjectsByType<SpawnArea>(FindObjectsSortMode.InstanceID).Where(x => x.type == SpawnAreaType.lootbox));
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= spawnEveryNSeconds)
        {
            Instantiate(
                lootbox,
                LootSpawnAreas[Random.Range(0, LootSpawnAreas.Count)].RandomPointInBounds(),
                Quaternion.identity);
            timePassed = 0;
        }
    }
}
