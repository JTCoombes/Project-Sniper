using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public Objectives objectives;

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
            
            MainObjectiveComplete = objectives.ObjectiveComplete;
            objectives.ObjectiveComplete = true;
            Debug.Log("Objective Complete");
        }
        else
        {
            Debug.Log("Objective Incomplete");
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
public class Objectives
{
    public string ObjectiveName, ObjectiveDescription;
    public GameObject[] enemies;
    public bool ObjectiveComplete;
}
