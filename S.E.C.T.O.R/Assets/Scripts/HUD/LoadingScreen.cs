using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {

    [SerializeField]
    private Image _screenImage;
    [SerializeField]
    private Image _loadingBar;
    [SerializeField]
    private Image _loadingBarBackground;

	void Start ()
    {
        _screenImage.gameObject.SetActive(false);
	}

    public void LoadingBarProgression(float progress)
    {
        _loadingBar.fillAmount = progress;
    }
    public void StartLoadingScreen()
    {
        _screenImage.gameObject.SetActive(true);
        _loadingBar.fillAmount = 0.0f;
    }
    public void HideLoadingScreen()
    {
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        float fadeRatio = 1.0f;
        Color color = Color.white;
        while(fadeRatio > 0.0f)
        {
            fadeRatio -= Time.deltaTime;
            color.a = fadeRatio;
            _screenImage.color = color;
            _loadingBar.color = color;
            _loadingBarBackground.color = color;
            yield return null;
        }
        _screenImage.gameObject.SetActive(false);
    }
}
