using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTrapStepVisualize : MonoBehaviour
{
    public ParticleSystem effectParticle;
    private bool isStepped;

    private void Start()
    {
        if (effectParticle.isPlaying)
            effectParticle.Stop();
    }

    private void OnTriggerExit(Collider other)
    {
        if (isStepped)
        {
            if(effectParticle.isPlaying)
            {
                effectParticle.Stop();
            }
            isStepped = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var victim = other.gameObject.GetComponentInParent<EffectReciever>();

        if (victim != null)
        {
            isStepped = true;
            if(effectParticle.isStopped)
            {
                effectParticle.Play();
            }
        }
            
    }
}
