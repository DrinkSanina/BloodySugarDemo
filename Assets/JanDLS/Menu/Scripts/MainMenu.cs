using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuButtons;
    public GameObject settingsScreen;
    public GameObject loadingScreen;
    public GameObject arenaSetupScreen;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMenuButtons(bool value)
    {
        mainMenuButtons.SetActive(value);
    }

    public void ShowSettingsScreen(bool value)
    {
        settingsScreen.SetActive(value);
    }

    public void ShowLoadingScreen(bool value)
    {
        loadingScreen.SetActive(value);
    }

    public void ShowArenaSetupScreen(bool value)
    {
        arenaSetupScreen.SetActive(value);
    }

    public void StartLoadingLevel()
    {
        mainMenuButtons.SetActive(false);
        settingsScreen.SetActive(false);
        arenaSetupScreen.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Arena");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
