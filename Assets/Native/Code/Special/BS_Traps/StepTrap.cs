using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ловушка, накладывающая эффект, когда персонаж наступил на неё, и сбрасывающая, когда отошел
/// </summary>
[RequireComponent(typeof(EffectInstigatorNullifier))]
public class StepTrap : MonoBehaviour
{
    private EffectInstigatorNullifier effectInstigator;


    private void Start()
    {
        effectInstigator = GetComponent<EffectInstigatorNullifier>();
    }


    private void OnTriggerEnter(Collider other)
    {
        EffectReciever victim = other.gameObject.GetComponent<EffectReciever>();
        if (victim != null)
        {
            effectInstigator.CastTimelessEffect(victim, effectInstigator.possibleEffects[0]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EffectReciever victim = other.gameObject.GetComponent<EffectReciever>();
        if (victim != null)
        {
            effectInstigator.NullifyAllChosenEffects(victim);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EffectReciever victim = collision.gameObject.GetComponent<EffectReciever>();
        if (victim != null)
        {
            effectInstigator.CastTimelessEffect(victim, effectInstigator.possibleEffects[0]);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        EffectReciever victim = collision.gameObject.GetComponent<EffectReciever>();
        if (victim != null)
        {
            effectInstigator.NullifyAllChosenEffects(victim);
        }
    }

}
