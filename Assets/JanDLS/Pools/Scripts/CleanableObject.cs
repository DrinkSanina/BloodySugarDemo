using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CleanableObject : MonoBehaviour, ICleanable
{
    public float SecondsToClean = 2f;
    public float maxParticleIntencity = 100f;
    public float cooldown_rate = 0.6f;
    

    ParticleSystem particleSystem;
    bool is_being_cleaned = false;
    public float cleaningProgress = 0f;

    IEnumerator progress_check;
    bool check_is_running = false;
    float checkProgressRate = 0.5f;

    float progressCheckpoint;
    public void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();

        ChangeEmissionRate(0);

        InvokeRepeating("ProgressCheck", 0, checkProgressRate);
        InvokeRepeating("SetCheckpoint", 0, checkProgressRate/5);
    }

    void ProgressCheck()
    {
        if(is_being_cleaned)
        {
            if (progressCheckpoint == cleaningProgress)
            {
                is_being_cleaned = false;
                progressCheckpoint = cleaningProgress;
            }
        }
        
    }

    void SetCheckpoint()
    {
        if(is_being_cleaned)
            progressCheckpoint = cleaningProgress;
    }

    public void Update()
    {
        if(!is_being_cleaned)
        {
            if (cleaningProgress > 0)
            {
                cleaningProgress -= Time.deltaTime * cooldown_rate;
                ChangeEmissionRate(cleaningProgress);
            }
        }
                  
    }

    public void Clean()
    {
        cleaningProgress += Time.deltaTime;

        ChangeEmissionRate(cleaningProgress);

        if(particleSystem.isStopped)
        {
            particleSystem.Play();
        }

        if(cleaningProgress >= SecondsToClean)
            Destroy(gameObject);
        
        is_being_cleaned = true;
    }

    private void ChangeEmissionRate(float value)
    {
        var em = particleSystem.emission;
        em.rateOverTime = new ParticleSystem.MinMaxCurve(value * maxParticleIntencity / SecondsToClean);
    }
}
