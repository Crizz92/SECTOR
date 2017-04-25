using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElement : MonoBehaviour {

    [SerializeField]
    protected string _name;
    [SerializeField]
    protected Text _text;
    public string ElementName
    {
        get { return _name; }
    }

    public virtual void Initialize()
    {
        if (_text)
        {
            SetText(Settings._mainMenuTranslation[_name]);
        }
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    void OnLanguageChanged()
    {
        if(_text)
        {
            SetText(Settings._mainMenuTranslation[_name]);
        }
    }

}
