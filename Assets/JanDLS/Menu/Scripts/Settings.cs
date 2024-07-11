using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    Resolution[] rsl;
    List<string> resolutions;

    public Toggle fullscren_toggle;
    public TMP_Dropdown dropdown;


    public void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        resolutions = new List<string>();
        rsl = Screen.resolutions;

        int currentResolutionIndex = -1;
        foreach (var i in rsl)
        {
            resolutions.Add(i.width + "x" + i.height);
        }

        resolutions = resolutions.Distinct().ToList();

        dropdown.ClearOptions();
        dropdown.AddOptions(resolutions);

        fullscren_toggle.isOn = Screen.fullScreen;
        dropdown.value = resolutions.Count - 1;
    }

    public void Resolution()
    {
        Screen.SetResolution(int.Parse(resolutions[dropdown.value].Split("x")[0]), int.Parse(resolutions[dropdown.value].Split("x")[1]), fullscren_toggle.isOn);
    }


}
