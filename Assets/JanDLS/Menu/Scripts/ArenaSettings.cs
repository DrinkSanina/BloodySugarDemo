using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSettings : MonoBehaviour
{
    public static ArenaSettings Instance;

    public int waves;
    public int enemies;
    public bool soda;
    public bool choco;
    public bool caramel;

    public float waveDuration = 15f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
