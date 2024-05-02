using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTemp : MonoBehaviour
{
    public Transform shootpoint;
    [SerializeField]
    private Camera MainCam;
    [Range(0, 1000)]
    public float Range;
    public LayerMask IgnoreRaycast;
    Vector3 cursorPos;
    public float Damage;

    public bool CantShoot;

    // Start is called before the first frame update
    void Start()
    {
        //MainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CantShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray ray = MainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        

        Vector3 shootdirection = MainCam.transform.forward;
        
        if(Physics.Raycast(shootpoint.position, shootdirection, out hit, Range, ~IgnoreRaycast))
        {
            cursorPos = hit.point;
            Debug.Log(hit.transform.name);

            Target target = GameObject.FindGameObjectWithTag("Enemy").GetComponentInParent<Target>();
            if(target != null)
            {
                if (hit.collider.CompareTag("Head"))
                {
                    Debug.Log("hit head");
                    target.TakeDamage(Damage);
                }

                if (hit.collider.CompareTag("Body"))
                {
                    Debug.Log("hit Body");
                    target.TakeDamage(Damage/2);
                }

                if (hit.collider.CompareTag("Limb"))
                {
                    Debug.Log("hit Limbs");
                    target.TakeDamage(Damage/3);
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
