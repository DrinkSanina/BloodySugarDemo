using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class AutoPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponents<ICollectable>().Length > 0)
        {
            ICollectable item = other.GetComponents<ICollectable>()[0];
            item.OnPickUp(this.transform.parent.gameObject);
            Statistics.instance.PowerupsCollected++;
        }
    }

}
