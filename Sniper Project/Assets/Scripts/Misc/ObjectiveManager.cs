using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public MainObjectives MainObj;

    public SecondaryObjective SecondaryObjective;

    public bool MainObjectiveComplete, SecondaryObjectiveComplete;

    [SerializeField]
    private TMP_Text ObjectiveText, ObjectiveDescriptionText;

    [SerializeField]
    private float PrimaryCountDown, SecondaryCountdown; 

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
            //return;
            MainObjectiveComplete = false;
        }

        if (!GuardsAlive())
        {
            SecondaryObjectiveComplete = true;
        }
        else
        {
            SecondaryObjectiveComplete = false;
            //return;
        }
    }

    bool TargetAlive() 
    {
        PrimaryCountDown -= Time.deltaTime;
        if(PrimaryCountDown <= 0f)
        {
            PrimaryCountDown = 1f;
            if(GameObject.FindGameObjectWithTag("Target") == null)
            {
                PrimaryCountDown = 0f;
                return false;
            }
        }
        return true;
    }

    bool GuardsAlive()
    {
        SecondaryCountdown -= Time.deltaTime;
        if(SecondaryCountdown <= 0f)
        {
            SecondaryCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                SecondaryCountdown = 0f;
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
    public ObjectiveType ObjType;
    //public bool ObjectiveComplete;

    public GameObject[] enemies;

    public enum ObjectiveType 
    {
        Kill,
        Destruction,
    
    }
}
