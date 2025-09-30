using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Impact"  + collision.gameObject.name);

        if (collision.gameObject.tag == "Head")
        {
            
            AI ai = collision.gameObject.GetComponentInParent<AI>();
            if (ai != null)
            {
                ai.ActivateRagdoll();
                Debug.Log("Hit Target");
            }
        }
    }


}
