using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualBasicUIController : MonoBehaviour
{
    public TextMeshProUGUI HpText;
    public RectTransform HpGauge;
    public TextMeshProUGUI KillcountText;
    public TextMeshProUGUI enemiesText;
    public TextMeshProUGUI wavesText;

    private SimpleWavesImporter spawnManager;
    private BasicEntityStats player;

    public GameObject StatisticsWindow;
    public GameObject LoadingScreen;
    public SmoothMouseLook sml;

    public TextMeshProUGUI label;
    public TextMeshProUGUI wavesSurvived;
    public TextMeshProUGUI demonsKilled;
    public TextMeshProUGUI outcomingDamage;
    public TextMeshProUGUI incomingHealing;
    public TextMeshProUGUI incomingDamageFromDemons;
    public TextMeshProUGUI incomingDamageFromEffects;
    public TextMeshProUGUI powerupsCollected;

    private bool isPaused;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                label.text = "Paused";
                label.color = Color.white;
                DisplayStatistics();
                Time.timeScale = 0;
                sml.enabled = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                label.text = "";
                StatisticsWindow.SetActive(false);
                sml.enabled = true;
                Time.timeScale = 1f;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            isPaused = !isPaused;
        }
    }
    void Start()
    {
        enemiesText.text = "";
        wavesText.text = "";

        GameObject[] possiblePlayer = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < possiblePlayer.Length; i++)
        {
            var component = possiblePlayer[i].GetComponent<BasicEntityStatsComponent>();
            if(component != null)
            {
                player = component.stats;
                break;
            }
        }

        Statistics.instance.StatisticsChanged += OnKillCountChange;

        spawnManager = GameObject.FindFirstObjectByType<SimpleWavesImporter>();
        {
            
            spawnManager.waveChanged += OnWaveChanged;
            spawnManager.WavesStarted += OnWaveStarted;
            spawnManager.CountdownChanged += OnTimerTicked;
            spawnManager.WavesEnded += OnWavesEnded;
        }
        

        OnHPChange();
        player.HealthChanged += OnHPChange;
        player.EntityIsDead += GameOver;
    }

    private void OnWaveStarted()
    {
        OnKillCountChange();
        OnWaveChanged(spawnManager.CurrentWaveNumber);
    }

    private void OnTimerTicked(int countdown)
    {
        wavesText.text = $"{countdown}";
        enemiesText.text = "";
    }

    private void OnHPChange()
    {
        HpText.text = Math.Round(player.HP).ToString();
        HpGauge.localScale = new Vector3((player.HP / player.maxHP), 1, 1);
    }

    private void OnKillCountChange()
    {
        KillcountText.text = Statistics.instance.DemonsKilled.ToString();
        enemiesText.text = $"{spawnManager.EnemiesLeft}/{spawnManager.MaxEnemies}";
    }

    private void OnWaveChanged(int wave)
    {
        wavesText.text = $"{spawnManager.CurrentWaveNumber+1}/{spawnManager.MaxWaves}";
        OnKillCountChange();
    }

    private void OnWavesEnded()
    {
        label.text = "YOU WIN!";
        label.color = Color.green;
        StartCoroutine(WaitAndDisplayStatistics());
        Time.timeScale = 0;
        sml.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void GameOver()
    {
        label.text = "YOU ARE DEAD";
        label.color = Color.red;
        StartCoroutine(WaitAndDisplayStatistics());
        Time.timeScale = 0;
        sml.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DisplayStatistics()
    {
        StatisticsWindow.SetActive(true);
        wavesSurvived.text = (Mathf.Round(Statistics.instance.WavesSurvived*100f)/100f).ToString();
        demonsKilled.text = (Mathf.Round(Statistics.instance.DemonsKilled*100f)/100f).ToString();
        outcomingDamage.text = (Mathf.Round(Statistics.instance.OutcomingDamage*100f)/100f).ToString();
        incomingHealing.text = (Mathf.Round(Statistics.instance.IncomingHealing*100f)/100f).ToString();
        incomingDamageFromDemons.text = (Mathf.Round(Statistics.instance.IncomingDamageFromDemons*100f)/100f).ToString();
        incomingDamageFromEffects.text = (Mathf.Round(Statistics.instance.IncomingDamageFromEffects*100f)/100f).ToString();
        powerupsCollected.text = (Mathf.Round(Statistics.instance.PowerupsCollected*100f)/100f).ToString();
    }

    private IEnumerator WaitAndDisplayStatistics()
    {
        yield return new WaitForSeconds(5f);
        DisplayStatistics();
    }

    IEnumerator LoadSceneAsync(string SceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync($"{SceneName}");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync("MainMenu"));
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync("Arena"));
    }

    
}
