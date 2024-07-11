using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletImpactSound : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayRandomHitImpact()
    {
        source.PlayOneShot(this.audioClips[Random.Range(0, this.audioClips.Length)]);
    }
}
