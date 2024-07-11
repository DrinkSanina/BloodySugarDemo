using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(EffectInstigatorNullifier))]
public class EffectTrap : MonoBehaviour
{
    private EffectInstigatorNullifier effectInstigatorNullifer;
    private EffectReciever victim;

    public float timeToDissappear = 0f;

    private void Start()
    {
        effectInstigatorNullifer = GetComponent<EffectInstigatorNullifier>();
        if(timeToDissappear > 0.0f)
        {
            Destroy(gameObject, timeToDissappear);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (victim != null)
        {
            effectInstigatorNullifer.CastEffect(victim, effectInstigatorNullifer.possibleEffects[0]);
            victim = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        victim = other.gameObject.GetComponentInParent<EffectReciever>();
        
        if(victim != null)
            effectInstigatorNullifer.CastEffect(victim, effectInstigatorNullifer.possibleEffects[0]);
    }


}
