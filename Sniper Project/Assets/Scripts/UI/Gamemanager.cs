using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public GameObject[] Screens;

    public void Start()
    {
        //instance = this;

        //SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
    }

    public void Loadgame()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Options()
    {
        Screens[0].SetActive(false);
        Screens[1].SetActive(true);
    } 
    public void Return()
    {
        Screens[0].SetActive(true);
        Screens[1].SetActive(false);
    }

    public void Exitgame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
