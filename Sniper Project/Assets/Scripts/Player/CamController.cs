using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour
{
    [Header("Properties")]
    public float MouseSens;
    float RotX = 0f;
    public float BaseFov = 90;

    [Header("Refs")]
    public Camera Cam;
    public Transform Player;

    [Header("UI")]
    public Slider MouseSensSlider;
    public Text MouseSensText;



    // Start is called before the first frame update
    void Start()
    {
        Cam.fieldOfView = BaseFov;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        /*
        MouseSensSlider.value = MouseSens;
        MouseSensText.text = MouseSens.ToString();

        LoadSettings();
        */
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSens * Time.deltaTime;

        RotX -= mouseY;
        RotX = Mathf.Clamp(RotX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(RotX, 0f, 0f);
        Player.Rotate(Vector3.up * mouseX);
    }

    private void LoadSettings()
    {
       
    }

    public void SaveSettings()
    {

    }

    public void MouseSensitivity(float amount)
    {
        MouseSens = amount;
        MouseSensText.text = MouseSens.ToString();
    }
}
