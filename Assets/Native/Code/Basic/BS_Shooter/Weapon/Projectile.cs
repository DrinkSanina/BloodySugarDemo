using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LayerMask))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    public float damage;

    //Lifetime
    public float maxLifetime = 100.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Set gravity
        rb.useGravity = useGravity;

        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        //Временная, до момента создания урона
        Destroy(gameObject);

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        //if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }
}
