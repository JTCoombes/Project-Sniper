using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private bool IsPaused;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !IsPaused)
        {
            Paused();
        }
        else if (IsPaused)
        {
            Resume();
        }
    }

    public void Paused()
    {
        IsPaused = true;
        Debug.Log("Paused Game");
    }

    void Resume()
    {
        IsPaused = false;
        Debug.Log("Resume Game");
    }
}
