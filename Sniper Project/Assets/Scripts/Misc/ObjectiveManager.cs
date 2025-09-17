using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public MainObjectives MainObj;

    public SecondaryObjective SecondaryObjective;

    public bool MainObjectiveComplete;

    [SerializeField]
    private TMP_Text ObjectiveText, ObjectiveDescriptionText;

    private float SearchCountdown = 1f; 

    // Start is called before the first frame update
    void Start()
    {
        // Set UI Elememnts 
    }

    // Update is called once per frame
    void Update()
    {
        if (!TargetAlive())
        {
            
            //MainObjectiveComplete = MainObj.ObjectiveComplete;
            //MainObj.ObjectiveComplete = true;

            MainObjectiveComplete = true;
            Debug.Log("Objective Complete");
        }
        else
        {
            //Debug.Log("Objective Incomplete");
            return;
        }
    }



    bool TargetAlive() 
    {
        SearchCountdown -= Time.deltaTime;
        if(SearchCountdown <= 0f)
        {
            SearchCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                SearchCountdown = 0f;
                return false;
            }
        }
        return true;
    }
}
[System.Serializable]
public class MainObjectives
{
    public string ObjectiveName, ObjectiveDescription;
    public GameObject[] enemies;
    //public bool ObjectiveComplete;
}

[System.Serializable]
public class SecondaryObjective
{
    public string ObjectiveName, ObjectiveDescription;
    public bool ObjectiveComplete;
    public ObjectiveType ObjType;

    public GameObject[] enemies;

    public enum ObjectiveType 
    {
        Kill,
        Destruction,
    
    }
}
