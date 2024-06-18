using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public float Gravity;
    private Vector3 StartPos;
    private Vector3 StartDir;

    private bool initilized = false;
    private float StartTime = -1;

    public void Initilize(Transform startPosition, float speed, float gravity)
    {
        StartPos = startPosition.position;
        StartDir = startPosition.forward.normalized;
        this.Speed = speed;
        this.Gravity = gravity;
        initilized = true;
    }

    private Vector3 findPoint(float time)
    {
        Vector3 point = StartPos + (StartDir * Speed * time);
        Vector3 gravityVector = Vector3.down * time * time;
        return point + gravityVector;
    }

    private bool CastRayBetweenPoints(Vector3 startPoint, Vector3 Endpoint, out RaycastHit hit)
    {
        return Physics.Raycast(startPoint, Endpoint - startPoint, out hit, (Endpoint - startPoint).magnitude);
    }

    private void FixedUpdate()
    {
        if (!initilized) return;
        if(StartTime < 0)
        {
            StartTime = Time.time;
        }
    }

    void CalculateHit()
    {
        RaycastHit hit;
        float currentTime = Time.time - StartTime;
        float nextTime = currentTime + Time.fixedDeltaTime;

        Vector3 currentPos = findPoint(currentTime);
        Vector3 NextPos = findPoint(nextTime);

        if(CastRayBetweenPoints(currentPos, NextPos, out hit))
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (!initilized || StartTime < 0) return;

        float currentTime = Time.time - StartTime;
        Vector3 currentPos = findPoint(currentTime);
        transform.position = currentPos;
    }
}
