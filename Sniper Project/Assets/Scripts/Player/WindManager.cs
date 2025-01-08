using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public float WindDelta = 0.1f;
    public float WindUpdateTime = 0.5f;

    public float windMaxMagnitude = 10f;
    public float windStartMaxMagnitude = 3f;

    [SerializeField]
    private Vector2 Wind;
    private Vector2 windUpdated;
    [SerializeField]
    private float time;

    public Vector2 getWind()
    {
        return Wind;
    }

    private void SetWind(Vector2 newWind)
    {
        Wind = newWind;
        windUpdated = Wind;
        time = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetWind(Random.insideUnitCircle * windStartMaxMagnitude);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWind();

        time += Time.deltaTime;

        if(time > WindUpdateTime)
        {
            time = 0f;

            windUpdated = GetUpdatedWind(); 
        }
    }

    private void UpdateWind()
    {
        Wind = Vector2.Lerp(Wind, windUpdated, Time.deltaTime);


    }

    private Vector2 GetUpdatedWind()
    {
        Vector2 result = Wind + Random.insideUnitCircle * WindDelta;
        result = result.normalized * Mathf.Min(result.magnitude, windMaxMagnitude);
        return result;
    }
}
