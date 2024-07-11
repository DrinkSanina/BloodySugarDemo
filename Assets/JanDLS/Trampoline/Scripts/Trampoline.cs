using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float jump_force = 1000f;

    Rigidbody rb;
    CharacterController cc;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            Rigidbody rb = other.gameObject.AddComponent<Rigidbody>();
            rb.mass = 1;
            rb.angularDrag = 0.05f;
            rb.automaticCenterOfMass = true;
            rb.automaticInertiaTensor = true;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.interpolation = RigidbodyInterpolation.None;

            cc = other.gameObject.GetComponent<CharacterController>();
            FPSWalker fpc = other.gameObject.GetComponent<FPSWalker>();

            cc.enabled = false;
            
            

            rb.AddForce(this.transform.forward * jump_force);

            fpc.grounded = false;
            fpc.sent_flying = true;
        }
    }

}
