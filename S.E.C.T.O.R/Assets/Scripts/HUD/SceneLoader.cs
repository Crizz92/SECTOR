using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : Singleton<SceneLoader> {

    private AsyncOperation _asyncData;
    [SerializeField]
    private LoadingScreen _loadingScreen;

    
    public void LoadScene(string scene)
    {
        _asyncData = SceneManager.LoadSceneAsync(scene);
        _loadingScreen.StartLoadingScreen();
        _asyncData.allowSceneActivation = true;
        StopCoroutine("SceneLoading");
        StartCoroutine("SceneLoading");
    }

    IEnumerator SceneLoading()
    {
        while(_asyncData.progress < 1.0f)
        {
            _loadingScreen.LoadingBarProgression(_asyncData.progress / 0.9f);
            yield return null;
        }
        _loadingScreen.HideLoadingScreen();
    }

}
