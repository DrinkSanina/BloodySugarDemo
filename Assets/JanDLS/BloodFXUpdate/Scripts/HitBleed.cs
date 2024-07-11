using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBleed : MonoBehaviour
{
    public ParticleSystem BleedPrefab;

    public Color ColorMin;
    public Color ColorMax;

    

    public void DisplayBloodFX(Vector3 position)
    {
        ParticleSystem ps = Instantiate(BleedPrefab, position,  Quaternion.LookRotation(transform.forward), this.transform);

        var _main = ps.main;
        _main.startColor = new ParticleSystem.MinMaxGradient(ColorMin, ColorMax);
    }


}
