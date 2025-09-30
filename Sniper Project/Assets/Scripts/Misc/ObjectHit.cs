using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ObjectHit : Shootable
{
     
    public enum StateType
    {
        Static,
        Moveable
    }

    [SerializeField]
    private StateType Type;

    public override void OnHit(RaycastHit hit)
    {
        if (Type == StateType.Static)
        {
            GameObject HitEffect = ObjectPoolingManager.instance.SpawnFromPool("WallEffect", hit.point + (hit.normal * 0.05f), Quaternion.LookRotation(hit.normal));
            Debug.Log("Hit Object");
        }

        else if (Type == StateType.Moveable)
        {
            GameObject HitEffect = ObjectPoolingManager.instance.SpawnFromPool("WallEffect", hit.point + (hit.normal * 0.05f), Quaternion.LookRotation(hit.normal));
            hit.rigidbody.isKinematic = false;
            Debug.Log("Hit Object");
        }
            

           //play hit sfx
        
    }
}
