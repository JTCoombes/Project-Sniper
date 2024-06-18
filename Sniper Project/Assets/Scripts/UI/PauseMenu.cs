using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject PauseUI;
    public GameObject SettingsUI;

    [SerializeField]
    public bool IsPaused;
    [SerializeField]
    private bool Insettings;
    [SerializeField]
    //GunTemp gun;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&& !Insettings)
        {
            if (!IsPaused)
            {
                Paused();
            }
            else
            {
                Resume();
            }
            
        }
    }

    public void Paused()
    {
        IsPaused = true;
        Insettings = false;
        PauseUI.SetActive(true);
        SettingsUI.SetActive(false);
        //gun.CantShoot = true;
        Time.timeScale = Mathf.Epsilon;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Paused Game");
    }

    public void Resume()
    {
        IsPaused = false;
        Insettings = false;
        PauseUI.SetActive(false);
        SettingsUI.SetActive(false);
        //gun.CantShoot = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Resume Game");
    }

    public void Settings()
    {
        IsPaused = true;
        Insettings = true;
        PauseUI.SetActive(false);
        SettingsUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("In Settings Menu");
    }

    public void Return()
    {
        PauseUI.SetActive(true);
        SettingsUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void MainMenu()
    {
        //IsPaused = false;
        //Insettings = false;
        Time.timeScale = 1f;
        Gamemanager.instance.LoadStartScreen();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
