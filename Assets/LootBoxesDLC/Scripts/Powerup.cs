using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(EffectInstigator))]
public class Powerup : MonoBehaviour, ICollectable
{
    public float rotationSpeed = 2f;
    public float moveUpTime = 0.1f;
    public float moveUpDistance = 0.5f;
    public float dissappearTimeInSeconds = 20f;

    private EffectInstigator instigator;

    void Start()
    {
        instigator = GetComponent<EffectInstigator>();
        
        StartCoroutine(Rotate());
        StartCoroutine(MoveUp());

        Destroy(gameObject, dissappearTimeInSeconds);
    }

    public void OnPickUp(GameObject pickuper)
    {
        EffectReciever reciever = pickuper.GetComponent<EffectReciever>();
        if(reciever != null)
        {
            foreach (EntityEffect ef in instigator.possibleEffects)
            {
                instigator.CastEffect(reciever, ef);
            }

            Destroy(gameObject);
        }
        
    }
    public IEnumerator MoveUp()
    {
        Vector3 Origin = this.transform.position;
        Vector3 Destination = Origin + new Vector3(0, moveUpDistance, 0);

        float currentMovementTime = 0f;
        while (Vector3.Distance(transform.localPosition, Destination) > 0)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(Origin, Destination, currentMovementTime / moveUpTime);
            yield return null;
        }  
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(0, rotationSpeed, 0);
            yield return null;
        }
    }
}
