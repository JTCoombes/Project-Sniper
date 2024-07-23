using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPool> objectPools = new List<ObjectPool>();

    public static ObjectPoolingManager instance;

    private void Awake()
    {
        PoolSetup();
        instance = this;
    }

    private void PoolSetup()
    {
        foreach (ObjectPool pools in objectPools)
        {
            pools.Setup();
        }
    }

    public GameObject SpawnFromPool(string PoolName, Vector3 pos, Quaternion Rot)
    {
        foreach (ObjectPool pools in objectPools)
        {
            if(string.Equals(pools.name, PoolName))
            {
                return pools.Spawn(pos, Rot);
            }
        }

        return null;
    }
}
