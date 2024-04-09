using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTemp : MonoBehaviour
{
    public Transform shootpoint;
    [SerializeField]
    private Camera MainCam;
    public float Range;
    public LayerMask Mask;
    Vector3 cursorPos;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray ray = MainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        

        Vector3 shootdirection = MainCam.transform.forward;
        
        if(Physics.Raycast(shootpoint.position, shootdirection, out hit, Range))
        {
            cursorPos = hit.point;
            Debug.Log(hit.transform.name);

            Target target = GameObject.FindGameObjectWithTag("Enemy").GetComponentInParent<Target>();
            if(target != null)
            {
                if (hit.collider.CompareTag("Head"))
                {
                    Debug.Log("hit head");
                }

                if (hit.collider.CompareTag("Body"))
                {
                    Debug.Log("hit Body");
                }

                if (hit.collider.CompareTag("Limb"))
                {
                    Debug.Log("hit Limbs");
                }


            }
        }
        else
        {
            cursorPos = ray.GetPoint(Range);
        }

        Debug.DrawRay(shootpoint.position, shootdirection * Range, Color.red);
    }
}
