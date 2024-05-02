using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public Transform[] Waypoints;
    [SerializeField]
    int randomSpot;

    private float Waitime;
    public float StartWaitime = 1f;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Waitime = StartWaitime;
        randomSpot = Random.Range(0, Waypoints.Length);

        
    }

    private void Patrol()
    {
        agent.SetDestination(Waypoints[randomSpot].position);

        if(Vector3.Distance(transform.position, Waypoints[randomSpot].position) < 2.0f)
        {
            if(Waitime <= 0)
            {
                randomSpot = Random.Range(0, Waypoints.Length);

                Waitime = StartWaitime;
            }
            else
            {
                Waitime -= Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }
}
