using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindMeter : MonoBehaviour
{

    public Transform player;
    public WindManager Windmanager;
    public Slider slider;


    private void UpdateSlider()
    {
        Vector2 playerVector = new Vector2(player.right.x, player.right.z);
        float windProjection = Vector2.Dot(Windmanager.getWind(), playerVector);
        slider.value = windProjection / (Windmanager.windMaxMagnitude * 2) + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlider();
    }

  
}
