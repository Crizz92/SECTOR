using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiUIElementEditor : MonoBehaviour {

    [SerializeField]
    private Text[] _textList;
    [SerializeField]
    private Image[] _imageList;

    public void SetAlpha(float alpha)
    {
        foreach (Text text in _textList)
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }
        foreach (Image image in _imageList)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
