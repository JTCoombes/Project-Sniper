using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
using TMPro;
using System;

public class Options : MonoBehaviour
{
    [Header("Screen Refs")]
    public AudioMixer Mixer;
    public TMP_Dropdown ResDropDown;
    public TMP_Dropdown QualityDropDown;
    public Toggle Fullscreen;
    private int Screenint;
    private bool isFullScreen = false;
    string QualityName = "Quality";
    string ResName = "ResOptions";
    public Slider MusicSlider;
    public Slider SfxSlider;
    public Slider FovSlider;
    public TMP_Text MusicText;
    public TMP_Text FovText;
    public TMP_Text SfxText;
    public float Fov;
    Resolution[] resolutions;
    
    [Space]

    [Header("Player Refs")]
    public CamController CC;


    private void Awake()
    {
        Screenint = PlayerPrefs.GetInt("toggleState");

        if(Screenint == 1)
        {
            isFullScreen = true;
            Fullscreen.isOn = true;
        }
        else
        {
            Fullscreen.isOn = false;
        }

        ResDropDown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(ResName, ResDropDown.value);
            SaveSettings();

        }));

        QualityDropDown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(QualityName, QualityDropDown.value);
            SaveSettings();

        }));

        
    }
    // Start is called before the first frame update
    void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MVolume", 1f);
        Mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MVolume"));

        SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 1f);
        Mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SfxVolume"));

        QualityDropDown.value = PlayerPrefs.GetInt(QualityName, 3);

        resolutions = Screen.resolutions;

        ResDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        ResDropDown.AddOptions(options);
        ResDropDown.value = PlayerPrefs.GetInt(ResName, currentResolutionIndex);
        ResDropDown.RefreshShownValue();

        FovText.text = FovSlider.value.ToString();
        MusicText.text = MusicSlider.value.ToString();
        SfxText.text = SfxSlider.value.ToString();

        LoadFov();
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRes(int ResIndex)
    {
        Resolution resolution = resolutions[ResIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMusicVolume(float Volume)
    {
        PlayerPrefs.SetFloat("MVolume", Volume);
        Mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MVolume"));
        MusicText.text = Volume.ToString();
    }
    public void SetSFXVolume(float Volume)
    {
        PlayerPrefs.SetFloat("SfxVolume", Volume);
        Mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SfxVolume"));
        SfxText.text = Volume.ToString();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        if (isFullscreen == false)
        {
            PlayerPrefs.SetInt("toggleState", 0);
        }
        else
        {
            PlayerPrefs.SetInt("toggleState", 1);
        }
        SaveSettings();
    }

    public void FovBase(float amount)
    {
        PlayerPrefs.SetFloat("baseFov", amount);
        CC.BaseFov = amount;
        CC.Cam.fieldOfView = CC.BaseFov;
        FovText.text = amount.ToString();

        SaveSettings();
    }

    private void LoadFov()
    {
        CC.BaseFov = PlayerPrefs.GetFloat("baseFov");
        CC.Cam.fieldOfView = CC.BaseFov;
        FovSlider.value = CC.BaseFov;
        FovText.text = CC.BaseFov.ToString();
        Fov = CC.BaseFov;
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
}
