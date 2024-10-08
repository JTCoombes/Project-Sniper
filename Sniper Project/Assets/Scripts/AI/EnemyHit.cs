using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : Shootable
{
    //public Rigidbody[] Rigidbodies;
    //public ParticleSystem BloodSplat;

    public AI ai;
    public float Impact;

    public override void OnHit(RaycastHit hit)
    {
       GameObject bloodspalt = ObjectPoolingManager.instance.SpawnFromPool("BloodSplat", hit.point + (hit.normal * 0.05f), Quaternion.LookRotation(hit.normal));
       ai.ActivateRagdoll();
       GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal * Impact, hit.point);
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
