using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Language
{
    French,
    English,
    German,
}

public static class Settings {

    public static InputContainer _PCInputContainer;
    public static InputContainer _GPPInputContainer;
    public static int _screenWidth;
    public static int _screenHeight;
    public static string _language;
    public static Dictionary<string, string> _mainMenuTranslation;
    public static Dictionary<string, string> _currentLevelTranslation;

    public static void Initialize()
    {
        InputContainer[] inputContainerFound = GameObject.FindObjectsOfType<InputContainer>();
        for(int i = 0; i < inputContainerFound.Length; i++)
        {
            if (inputContainerFound[i]._name == "PC") _PCInputContainer = inputContainerFound[i];
            if (inputContainerFound[i]._name == "Gamepad") _GPPInputContainer = inputContainerFound[i];
        }
        UpdateLanguage();

    }

    public static void UpdateLanguage()
    {
        _mainMenuTranslation = XMLParser.ParseXMLFile("MainMenu", _language);
        UIElement [] uiElements = GameObject.FindObjectsOfType<UIElement>();
        for(int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].Invoke("OnLanguageChanged", 0.0f);
        }
    }
    
}
