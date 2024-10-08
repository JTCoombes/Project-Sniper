using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ObjectHit : Shootable
{


    public override void OnHit(RaycastHit hit)
    {
        GameObject HitEffect = ObjectPoolingManager.instance.SpawnFromPool("WallEffect", hit.point + (hit.normal * 0.05f), Quaternion.LookRotation(hit.normal));
        Debug.Log("Hit Object");
    }
}
