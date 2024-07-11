using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaSettingsUI : MonoBehaviour
{
    public ArenaSettings arenaSettings;

    public int maxWaves;
    public int maxEnemies;

    public Slider wavesSlider;
    public TextMeshProUGUI wavesNumber;

    public Slider enemiesSlider;
    public TextMeshProUGUI enemiesNumber;

    public Toggle caramelDemonToggle;
    public Toggle chocolateSpawnToggle;
    public Toggle sodaMonsterToggle;

    private int waves;
    private int enemies;

    private void Start()
    {
        float minWaves = 1.0f / maxWaves;
        float minEnemies = 1.0f / maxEnemies;

        wavesSlider.minValue = minWaves;
        wavesSlider.value = minWaves;
        enemiesSlider.minValue = minEnemies;
        enemiesSlider.value = minEnemies;

        waves = 1;
        enemies = 1;

        //OnEnemiesSliderChanged();
        //OnWavesSliderChanged();
    }

    public void OnWavesSliderChanged()
    {
        if(wavesSlider.value == 1.0)
        {
            waves = -1;
            wavesNumber.text = "inf";
        }
        else
        {
            int scaled_waves = (int) Mathf.Round(wavesSlider.value*maxWaves);
            wavesNumber.text = $"{scaled_waves}";
            waves = scaled_waves;
        }
    }
    
    public void OnEnemiesSliderChanged()
    {
        int scaled_enemies = (int)Mathf.Round(enemiesSlider.value * maxEnemies);
        enemiesNumber.text = $"{scaled_enemies}";
        enemies = scaled_enemies;
    }

    public void OnToBattleClick()
    {
        arenaSettings.enemies = enemies;
        arenaSettings.waves = waves;
        arenaSettings.soda = sodaMonsterToggle.isOn;
        arenaSettings.caramel = caramelDemonToggle.isOn;
        arenaSettings.choco = chocolateSpawnToggle.isOn;
    }

}
