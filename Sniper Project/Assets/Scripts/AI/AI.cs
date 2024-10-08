using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public enum States
    {
        Patrol,
        Dead,
    }

    public Transform[] Waypoints;
    [SerializeField]
    int randomSpot;

    private float Waitime;
    public float StartWaitime = 1f;

    private States AiStates;

    NavMeshAgent agent;
    [SerializeField]
    private Rigidbody[] rigidbodies;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        rigidbodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();

        AiStates = States.Patrol;
    }

    // Start is called before the first frame update
    void Start()
    {
        DisableRagdoll();

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

    private void Death()
    {
        agent.SetDestination(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(AiStates == States.Patrol)
        {
            Patrol();
        }

        if(AiStates == States.Dead)
        {
            Invoke("Death", .5f);
        }

        
    }

    void DisableRagdoll()
    {
        anim.enabled = true;
        foreach (Rigidbody R in rigidbodies)
        {
            
            R.useGravity = false;
        }

    }

    public void ActivateRagdoll()
    {
        anim.enabled = false;
        AiStates = States.Dead;
        foreach (Rigidbody R in rigidbodies)
        {

            R.useGravity = true;
            
        }

    }
}
