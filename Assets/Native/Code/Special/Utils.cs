using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static string SecondsToHMS(float timeInSeconds)
    {
        int hours = TimeSpan.FromSeconds(timeInSeconds).Hours;
        int minutes = TimeSpan.FromSeconds(timeInSeconds).Minutes;
        int seconds = TimeSpan.FromSeconds(timeInSeconds).Seconds;

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
