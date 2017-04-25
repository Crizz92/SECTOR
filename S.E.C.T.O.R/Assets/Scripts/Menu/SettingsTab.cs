using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsTab : MenuTab {

    private Slider _globalSound;
    private Slider _voiceSound;
    private Slider _fxSound;
    [SerializeField]
    private Dropdown _language; 

    public void SaveSettings()
    {
        string language = _language.options[_language.value].text;
        if(Settings._language != language)
        {
            Settings._language = language;
            Settings.UpdateLanguage();
        }
    }
}
