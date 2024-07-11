using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(EffectInstigator))]
public class DemoTrap : MonoBehaviour
{
    public float cooldownSeconds = 60.0f;

    public Material armedTrap;
    public Material disarmedTrap;

    public bool trapIsArmed = true;

    private MeshRenderer meshRenderer;
    private EffectInstigator effectInstigator;


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        effectInstigator = GetComponent<EffectInstigator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(trapIsArmed)
        {
            EffectReciever victim = other.gameObject.GetComponent<EffectReciever>();
            if (victim != null)
            {
                effectInstigator.CastEffect(victim, effectInstigator.possibleEffects[0]);
                SwithTrapState();
                StartCoroutine(ReloadTrap());
            }
        }
        
    }

    private void SwithTrapState()
    {
        trapIsArmed = !trapIsArmed;
        if (trapIsArmed)
        {
            meshRenderer.material = armedTrap;
        }
        else
        {
            meshRenderer.material = disarmedTrap;
        }
            
    }

    private IEnumerator ReloadTrap()
    {
        float secondsLeft = cooldownSeconds;
        while (secondsLeft > 0.0f)
        {
            secondsLeft -= 1.0f;
            yield return new WaitForSeconds(1.0f);
        }

        SwithTrapState();
    }

    
}
