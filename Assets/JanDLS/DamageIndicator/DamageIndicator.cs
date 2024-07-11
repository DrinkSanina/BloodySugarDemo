using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageIndicator : MonoBehaviour
{
    public BasicEntityStatsComponent playerStats;

    private PostProcessVolume volume;
    Vignette vignette;

    public float startIntensity = 0.643f;
    
    void Start()
    {
        playerStats.stats.RecievedDamage += DamageDealt;
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<Vignette>(out vignette);

        if (vignette)
            vignette.enabled.Override(false);

        effect = null;
    }

    private Coroutine effect;
    private void DamageDealt(float value, DamageSource source)
    {
        if(effect == null)
        {
            effect = StartCoroutine(TakeDamageEffect());
        }
        else
        {
            StopCoroutine(effect);
            effect = StartCoroutine(TakeDamageEffect());
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        float intensity = startIntensity;

        vignette.enabled.Override(true);
        vignette.intensity.Override(startIntensity);

        yield return new WaitForSeconds(0.4f);

        while (intensity > 0.45f)
        {
            intensity -= 0.01f;
            if (intensity < 0) intensity = 0;

            vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(0.1f);
        }

        vignette.enabled.Override(false);
        yield break;
    }


}
