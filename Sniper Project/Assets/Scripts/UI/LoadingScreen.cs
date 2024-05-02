using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    #region LoadingScreen_1
    /*
    public string LevelToLoad;
    public GameObject[] Screens;
    public Slider LoadingBar;
    public TMP_Text loadingtext;
    public Image[] LoadingImages;

    // Start is called before the first frame update
    void Start()
    {
        Screens[1].SetActive(false);
        LoadingImages[0].enabled = true;
        LoadingImages[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        Screens[0].SetActive(false);
        Screens[1].SetActive(true);

        StartCoroutine(LoadLevelAsync());
        //SceneManager.LoadScene(LevelToLoad);
    }

    public IEnumerator LoadLevelAsync()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(LevelToLoad);

        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            LoadingBar.value = async.progress;

            if(async.progress >= .9f && !async.allowSceneActivation)
            {
                loadingtext.text = "Press Any Key To Continue";
                LoadingBar.value = 1;
                LoadingImages[0].enabled = false;
                LoadingImages[1].enabled = true;
                if (Input.anyKey)
                {
                    async.allowSceneActivation = true;

                }
                Debug.Log("" + async.isDone);
            }
            yield return null;
        }
    }
    */
    #endregion
}
