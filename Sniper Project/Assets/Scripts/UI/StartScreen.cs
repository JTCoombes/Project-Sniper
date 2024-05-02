using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject[] Screens;

    public void StartGame()
    {
        Gamemanager.instance.Loadgame();
    }


    public void OptionsScreen()
    {
        Screens[0].SetActive(false);
        Screens[1].SetActive(true);
    }

    public void Return()
    {
        Screens[0].SetActive(true);
        Screens[1].SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
