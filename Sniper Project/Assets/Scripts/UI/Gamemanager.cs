using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public GameObject[] Screens;

    public GameObject LoadingScreen;

    public Slider Progressbar;
    public TMP_Text ProgressText;

    private Scene CurrentScene;

    float totalSceneProgress;
    List<AsyncOperation> asyncsList = new List<AsyncOperation>(); 

    public void Start()
    {
        instance = this;
        

        SceneManager.LoadSceneAsync((int)SceneIndex.StartScreen,LoadSceneMode.Additive);
    }

    public void Loadgame()
    {
        LoadingScreen.SetActive(true);
        asyncsList.Add(SceneManager.LoadSceneAsync((int)SceneIndex.GameScene, LoadSceneMode.Additive));

        StartCoroutine(GetLoadProgress());
        //SceneManager.UnloadSceneAsync((int)SceneIndex.StartScreen);
    }

    public IEnumerator GetLoadProgress()
    {
        for (int i = 0; i < asyncsList.Count; i++)
        {
            while (!asyncsList[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in asyncsList)
                {
                    totalSceneProgress += operation.progress;
                    
                }

                totalSceneProgress = (totalSceneProgress / asyncsList.Count) * 100f;

                Progressbar.value = Mathf.RoundToInt(totalSceneProgress);

                ProgressText.text = string.Format("Loading Level: {0}% ", totalSceneProgress);

                yield return null;
            }
        }

        LoadingScreen.SetActive(false);
        SceneManager.UnloadSceneAsync((int)SceneIndex.StartScreen);
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

