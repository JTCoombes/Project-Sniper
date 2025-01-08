using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public Vector2 Wind;
    public float Speed;
    public float Gravity;
    public float impactForce;
    private Vector3 StartPos;
    private Vector3 StartDir;

    private bool initilized = false;
    private float StartTime = -1;

   

    public void Initilize(Transform startPosition, float speed, float gravity, Vector2 wind)
    {
        StartPos = startPosition.position;
        StartDir = startPosition.forward.normalized;
        this.Speed = speed;
        this.Gravity = gravity;
        this.Wind = wind;
        StopAllCoroutines();
        initilized = true;
        StartTime = -1f;

        Debug.Log("" + wind.x);
        Debug.Log("" + wind.y);
    }

    private Vector3 findPoint(float time)
    {

        Vector3 MovementVec = (StartDir * time * Speed);
        Vector3 WindVec = new Vector3(Wind.x, 0, Wind.y) * time * time;
        Vector3 gravityVector = Vector3.down * time * time * Gravity;
        return StartPos + MovementVec + gravityVector + WindVec;
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

        CalculateHit();
    }

    void OnHIT(RaycastHit hit)
    {
        Debug.Log(hit.transform.name);

        Shootable shootableObject = hit.transform.GetComponent<Shootable>();

        if (shootableObject)
        {
             shootableObject.OnHit(hit);
        }


        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
    void CalculateHit()
    {
        RaycastHit hit;
        float currentTime = Time.time - StartTime;
        float PrevTime = currentTime - Time.fixedDeltaTime;
        float nextTime = currentTime + Time.fixedDeltaTime;


        Vector3 currentPos = findPoint(currentTime);
        
        Vector3 NextPos = findPoint(nextTime);

        

        if (PrevTime > 0)
        {
            Vector3 prevPos = findPoint(PrevTime);

            
            if (CastRayBetweenPoints(prevPos, currentPos, out hit))
            {
               OnHIT(hit);
            }
            
        }
        if(CastRayBetweenPoints(currentPos, NextPos, out hit))
        {
            OnHIT(hit);
        }
    }

    private void Update()
    {
        if (!initilized || StartTime < 0) return;

        float currentTime = Time.time - StartTime;
        Vector3 currentPos = findPoint(currentTime);
        transform.position = currentPos;
    }


    public IEnumerator Despawn(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        obj.SetActive(false);
    }
}
