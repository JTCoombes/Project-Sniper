using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyHit : Shootable
{
    //public Rigidbody[] Rigidbodies;
    //public ParticleSystem BloodSplat;

    public AI ai;
    public AudioClip[] audioClips;
    public AudioSource Source;

    [Range(0.1f,.5f)]
    public float VolumeChangeMultiplier;
    [Range(0.1f,.5f)]
    public float PitchChangeMultiplier;
    public float Impact;

    public void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    public override void OnHit(RaycastHit hit)
    {
       GameObject bloodspalt = ObjectPoolingManager.instance.SpawnFromPool("BloodSplat", hit.point + (hit.normal * 0.05f), Quaternion.LookRotation(hit.normal));
       ai.ActivateRagdoll();
       GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal * Impact, hit.point);
        Source.clip = audioClips[Random.Range(0, audioClips.Length)];
        Source.volume = Random.Range(1 - VolumeChangeMultiplier,1);
        Source.pitch = Random.Range(1 - PitchChangeMultiplier, 1 + PitchChangeMultiplier);
        Source.PlayOneShot(Source.clip);
           // play hit sfx
       //ActivateRagdoll();
       
       //this.gameObject.SetActive(false);
    }

    /*
    void ActivateRagdoll()
    {
        foreach (var rb in Rigidbodies)
        {
            rb.useGravity = true;
        }
    }


    void DisableRagdoll()
    {
        foreach (var rb in Rigidbodies)
        {
            rb.useGravity = false;
        }
    }
    */
}
