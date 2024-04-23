using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public GameObject[] Screens;

    private Scene CurrentScene;

    public void Start()
    {
        instance = this;
        

        SceneManager.LoadSceneAsync((int)SceneIndex.StartScreen,LoadSceneMode.Additive);
    }

    public void Loadgame()
    {
        SceneManager.UnloadSceneAsync((int)SceneIndex.StartScreen);
        SceneManager.LoadSceneAsync((int)SceneIndex.GameScene, LoadSceneMode.Additive);
    }

    public void LoadStartScreen()
    {
        SceneManager.UnloadSceneAsync((int)SceneIndex.GameScene);
        SceneManager.LoadSceneAsync((int)SceneIndex.StartScreen, LoadSceneMode.Additive);
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

public enum SceneIndex 
{
    PersitantScene = 0,
    StartScreen = 1,
    GameScene = 2,

}

