using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Cleaner : MonoBehaviour
{
    public float cleanRange;
    ParticleSystem particles;
    bool firsthit;

    public LayerMask trap_layer;

    private AudioSource flame_audio;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.gameObject.transform.position, this.gameObject.transform.forward * cleanRange);
    }

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        flame_audio = GetComponent<AudioSource>();

        if (particles.isPlaying)
            particles.Stop();
        if (flame_audio.isPlaying)
            flame_audio.Stop();
    }

    private void OnEnable()
    {
        particles = GetComponent<ParticleSystem>();
        flame_audio = GetComponent<AudioSource>();
        if (particles.isPlaying)
        {
            particles.Stop();
        }
        if(flame_audio.isPlaying)
        {
            flame_audio.Stop();
        }
    }


    public void PrimaryFire(Vector3 rayOrigin, Vector3 rayDirection)
    {
        if(particles.isStopped)
        {
            particles.Play();
            flame_audio.Play();
        }

        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection, cleanRange, trap_layer);
        for(int i = 0; i < hits.Length;i++)
        {
            if (hits[i].collider != null)
            {
                ICleanable cleanable = hits[i].transform.GetComponent<CleanableObject>();
                if (cleanable != null)
                {
                    cleanable.Clean();
                }
            }
        }

    }

    public void StopEmitting()
    {
        particles.Stop();
        flame_audio.Stop();
    }

    private RaycastHit CastRay(Vector3 origin, Vector3 direction, out Vector3 targetPoint)
    {
        RaycastHit hit;

        targetPoint = new Vector3(0, 0, 0);
        Physics.Raycast(origin, direction, out hit, cleanRange);

        return hit;
    }
}
