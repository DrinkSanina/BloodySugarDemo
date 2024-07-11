using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonHoverChangeColor : MonoBehaviour
{
    private TextMeshProUGUI buttonText;
    public Color hoveredColor;
    public Color exitedColor;

    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void HoveredColor()
    {
        buttonText.color = hoveredColor;
    }

    public void ExitedColor()
    {
        buttonText.color = exitedColor;
    }
}
