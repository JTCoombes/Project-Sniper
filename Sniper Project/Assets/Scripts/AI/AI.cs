using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public enum States
    {
        Patrol,
        RaiseAlarm,
        Dead,
    }

    public Transform[] Waypoints;
    [SerializeField]
    int randomSpot;

    private float Waitime;
    public float StartWaitime = 1f;

    [SerializeField]
    private States AiStates;
    public Sight AiSight;

    [SerializeField]
    private string NewTag;
    [SerializeField]
    private int NewLayer;
    bool isDead;
    [SerializeField]
    Collider[] Cols;

    NavMeshAgent agent;
    [SerializeField]
    private Rigidbody[] rigidbodies;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        rigidbodies = GetComponentsInChildren<Rigidbody>();
        Cols = GetComponentsInChildren<Collider>();
        anim = GetComponent<Animator>();

        AiStates = States.Patrol;
    }

    // Start is called before the first frame update
    void Start()
    {
        DisableRagdoll();

        anim.SetFloat("Forward", 0);

        Waitime = StartWaitime;
        randomSpot = Random.Range(0, Waypoints.Length);

    }

   

    private void Patrol()
    {
        anim.SetFloat("Forward", 1);
        agent.SetDestination(Waypoints[randomSpot].position);

        if(Vector3.Distance(transform.position, Waypoints[randomSpot].position) < 2.0f)
        {
            anim.SetFloat("Forward", 0);
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
        this.gameObject.tag = NewTag;
        this.gameObject.layer = NewLayer;

        foreach (Collider t in Cols)
        {
            t.gameObject.layer = NewLayer;
        }
        /*
        foreach(Transform t in gameObject.GetComponentInChildren<Transform>(false))
        {
            t.gameObject.layer = NewLayer;
        }
        */
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
            //isDead = true;
            Invoke("Death", .9f);
        }

        if(AiStates == States.RaiseAlarm)
        {
            RaiseAlarm();
        }

        if (AiSight.Insight)
        {
            AiStates = States.RaiseAlarm;
        }
        else
        {
            AiStates = States.Patrol;
        }
        
    }

    void RaiseAlarm()
    {
        Debug.Log("BODY FOUND");
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
