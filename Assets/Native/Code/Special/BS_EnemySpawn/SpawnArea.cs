using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnAreaType
{
    enemy,
    player,
    lootbox
}

[RequireComponent(typeof(BoxCollider))]
public class SpawnArea : MonoBehaviour
{
    private BoxCollider area;
    public SpawnAreaType type;
    
    public void Start()
    {
        area = GetComponent<BoxCollider>();
        area.isTrigger = true;
    }

    public Vector3 RandomPointInBounds(float height_offset = 0f)
    {
        return new Vector3(Random.Range(area.bounds.min.x, area.bounds.max.x), area.bounds.min.y + height_offset, Random.Range(area.bounds.min.z, area.bounds.max.z));
    }
}
