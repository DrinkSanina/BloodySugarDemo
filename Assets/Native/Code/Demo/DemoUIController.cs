using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DemoUIController : MonoBehaviour
{
    public BasicEntityStatsComponent playerStatsComponent;
    public EffectReciever playerEffectReciever;

    private ProgressBar healthBar;
    private Label maxHPlabel;
    private Label statsLabel;
    private Label effectsLabel;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        healthBar = root.Q<ProgressBar>("healthbar");
        maxHPlabel = root.Q<Label>("maxHP");
        statsLabel = root.Q<Label>("statsLabel");
        effectsLabel = root.Q<Label>("effectsLabel");
        
        RedrawHealthBar();
        RedrawEffects();
        RedrawStats();
        InvokeRepeating("RedrawEffects", 0f, 1f);

        playerStatsComponent.stats.HealthChanged += RedrawHealthBar;
        playerEffectReciever.StatsChanged += RedrawEffects;
        playerEffectReciever.StatsChanged += RedrawStats;
    }

    private void RedrawHealthBar()
    {
        maxHPlabel.text = Mathf.Round(playerStatsComponent.stats.maxHP).ToString();
        healthBar.title = Mathf.Round(playerStatsComponent.stats.HP).ToString();
        healthBar.value = (100 * playerStatsComponent.stats.HP / playerStatsComponent.stats.maxHP);
    }

    private void RedrawStats()
    {
        statsLabel.text = $"Movement Speed: {playerStatsComponent.stats.movementSpeedPercent*100}%\n";
        statsLabel.text += $"Accuracy: {playerStatsComponent.stats.accuracyPercent*100}%\n";
        statsLabel.text += $"Outcoming Damage: {playerStatsComponent.stats.damagePercent*100}%\n";
        statsLabel.text += $"Heal Per Second: {playerStatsComponent.stats.healPerSecond}\n";
        statsLabel.text += $"Incoming Overtime damage: {playerStatsComponent.stats.incomingDamagePerSecond}\n";
    }

    private void RedrawEffects()
    {
        effectsLabel.text = "";
        foreach(EffectWrapper ew in playerEffectReciever.castedEffects)
        {
            if(ew.secondsLeft != -1)
            {
                effectsLabel.text += $"{ew.EffectName} ({ew.DurationInHMS})\n";
                effectsLabel.text += ew.effect.Description + "\n";
                effectsLabel.text += "\n";
            }
            
        }
    }

}
