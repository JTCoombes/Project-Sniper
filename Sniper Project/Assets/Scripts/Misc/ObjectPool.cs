using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectPool
{
    [SerializeField]
    private string Id;

    public string name => Id;

    [SerializeField]
    private GameObject Prefab;

    [SerializeField]
    private int amount;

    [SerializeField]
    private int index = 0;

    [SerializeField]
    private Transform parent;

    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();


    public void Setup()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newObj = GameObject.Instantiate(Prefab, parent);
            newObj.SetActive(false);
            objects.Add(newObj);
        }
    }

    public GameObject Spawn(Vector3 Pos, Quaternion Rot)
    {
        GameObject obj = objects[index];

        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }

        obj.SetActive(true);
        obj.transform.position = Pos;
        obj.transform.rotation = Rot;

        index = (index + 1) % objects.Count;

        return obj;
    }

    public GameObject Despawn()
    {
        GameObject obj = objects[index];
        obj.SetActive(false);

        return obj;
    }
}
