using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public Lootbox Container;
    public float spawnChance = 0.5f;
    private BasicEntityStatsComponent entity;

    public int minTraps = 1;
    public int maxTraps = 3;

    public float minDropRadius = 400;
    public float maxDropRadius = 600;

    void Start()
    {
        entity = GetComponent<BasicEntityStatsComponent>();
        entity.stats.EntityIsDead += OnEntityDie;
    }

    private void OnEntityDie()
    {
        float chance = Random.Range(0, 1);
        if(spawnChance >= chance)
        {
            int trapsNum = Random.Range(minTraps, maxTraps);
            for(int i = 0; i < trapsNum; i++)
            {
                Lootbox trap = Instantiate(Container,this.transform.position,Quaternion.identity);
                trap.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere * (float)UnityEngine.Random.Range(minDropRadius, maxDropRadius));
            }
            
        }
    }
}
