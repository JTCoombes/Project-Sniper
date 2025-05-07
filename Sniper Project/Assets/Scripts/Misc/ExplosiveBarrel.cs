using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveBarrel : Shootable
{
    public float explosionRadius;
    public float ExplosionForce;

    [SerializeField]
    private Collider[] colliders;
    [SerializeField] 
    private Collider[] collidersToMove;

    public override void OnHit(RaycastHit hit)
    {
        colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        

        foreach (Collider Objects in colliders)
        {
            AI ai = Objects.GetComponentInParent<AI>();
            if (ai != null)
            {
                Debug.Log("AI Found");
                ai.ActivateRagdoll();
            }
        }

        collidersToMove = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider Objects in collidersToMove)
        {
            Rigidbody RB = Objects.GetComponent<Rigidbody>();
            if (RB != null)
            {
                Debug.Log("AI Found");
                RB.AddExplosionForce(ExplosionForce, transform.position, explosionRadius);
            }
        }





    }



}
