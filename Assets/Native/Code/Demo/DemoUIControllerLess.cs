using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DemoUIControllerLess : MonoBehaviour
{
    public BasicEntityStatsComponent playerStatsComponent;

    private ProgressBar healthBar;
    private Label maxHPlabel;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        healthBar = root.Q<ProgressBar>("healthbar");
        maxHPlabel = root.Q<Label>("maxHP");
        
        RedrawHealthBar();

        playerStatsComponent.stats.HealthChanged += RedrawHealthBar;
    }

    private void RedrawHealthBar()
    {
        maxHPlabel.text = Mathf.Round(playerStatsComponent.stats.maxHP).ToString();
        healthBar.title = Mathf.Round(playerStatsComponent.stats.HP).ToString();
        healthBar.value = (100 * playerStatsComponent.stats.HP / playerStatsComponent.stats.maxHP);
    }


}
