using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnStruct
{
    public GameObject Object;
    public float spawn_chance;
}

public class Lootbox : MonoBehaviour
{
    public List<SpawnStruct> PossibleObject;
    public GameObject OpenedModel;
    public float OpenCrateRemainingTime = 5f;

    private void Start()
    {
        float overall_chance = 0.0f;
        foreach (var powerup in PossibleObject)
        {
            overall_chance += powerup.spawn_chance;
        }
        if (overall_chance != 1.0f)
        {
            throw new System.Exception($"Пу пу пу. У лутбокса {this.gameObject} вероятности выпадения в сумме не равны единице. Такой код может случайно открыть портал в другую вселенную или уничтожить время");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PossibleObject.Count == 0)
            Debug.Log($"Наслаждайся ошибкой. PossiblePowerup пустой");
        else if (PossibleObject.Count == 1)
            Instantiate(PossibleObject[0].Object, this.transform.position, Quaternion.identity);
        else
        {
            Instantiate(RolldiceObject(), LowestMiddlePoint(), Quaternion.identity);
        }

        Destroy(gameObject);
        
        if(OpenedModel != null)
            Destroy(Instantiate(OpenedModel, LowestMiddlePoint(), Quaternion.identity), OpenCrateRemainingTime);
    }

    private Vector3 LowestMiddlePoint()
    {
        Collider collider = GetComponent<Collider>();
        if (collider == null)
            return this.transform.position;
        else
        {
            return new Vector3(collider.bounds.center.x,
                               collider.bounds.min.y,
                               collider.bounds.center.z);
        }
    }

    private GameObject RolldiceObject()
    {
        System.Random r = new System.Random();
        double diceRoll = r.NextDouble();

        double cumulative = 0.0;

        for (int i = 0; i < PossibleObject.Count; i++)
        {
            cumulative += PossibleObject[i].spawn_chance;
            if (diceRoll < cumulative)
            {
                return PossibleObject[i].Object;
            }
        }

        return null;
    }
}
