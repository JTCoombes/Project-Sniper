using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CamController : MonoBehaviour
{
    [Header("Properties")]
    [Range(0,500)]
    public float MouseSens;
    float RotX = 0f;
    [Range(60,100)]
    public float BaseFov = 90;

    [Space]
    public float VerticalRecoil;
    public float HorizontalRecoil;
    public float RecoilResetSpeed;

    [Header("Refs")]
    public Camera Cam;
    public Transform Player;

    [Header("UI")]
    public Slider MouseSensSlider;
    public TMP_Text MouseSensText;



    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        MouseSensSlider.value = MouseSens;
        MouseSensText.text = MouseSens.ToString();
        Cam.fieldOfView = BaseFov;

        LoadSettings();
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSens * Time.deltaTime;

        RotX -= mouseY;
        RotX = Mathf.Clamp(RotX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(RotX + -VerticalRecoil, 0f + -HorizontalRecoil, 0f);
        


        if(VerticalRecoil > 0f)
        {
            VerticalRecoil -= Time.smoothDeltaTime * RecoilResetSpeed;
        }
        else
        {
            VerticalRecoil = 0;
        }

        if(HorizontalRecoil > 0f)
        {
            HorizontalRecoil -= Time.smoothDeltaTime * RecoilResetSpeed;
        }
        else
        {
            HorizontalRecoil = 0f;
        }

        if (HorizontalRecoil < 0f)
        {
            HorizontalRecoil += Time.smoothDeltaTime * RecoilResetSpeed;
        }
        else
        {
            HorizontalRecoil = 0f;
        }

        Player.Rotate(Vector3.up * mouseX);
    }

    public void Recoil(float vertical, float Horizontal)
    {
        HorizontalRecoil = Random.Range(-Horizontal, Horizontal);
        VerticalRecoil = vertical;
        
    }

    private void LoadSettings()
    {
        float camsens = PlayerPrefs.GetFloat("BaseSens");
        MouseSens = camsens;
        MouseSensSlider.value = camsens;
        MouseSensText.text = MouseSens.ToString();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("BaseSens", MouseSens);
    }

    public void MouseSensitivity(float amount)
    {
        MouseSens = amount;
        MouseSensText.text = MouseSens.ToString();
    }
}
