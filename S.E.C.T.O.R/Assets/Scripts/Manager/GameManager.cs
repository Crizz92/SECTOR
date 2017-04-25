using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public enum EGameMod
{
    Menu,
    Solo,
    Coop,
    Online,
    Debug,
    CameraFree,
}

public static class GameManager {

    public static bool _inGame = false;
    private static EGameMod _gameMod = EGameMod.Menu;
    public static EGameMod GameMod
    {
        get { return _gameMod; }
    }
    public static void SetGameMod(EGameMod newGameMod)
    {
        _gameMod = newGameMod;
    }
    public static Scene _currentScene;
    private static PlayerInput _globalPlayerInput;
    public static PlayerInput GlobalPlayerInput
    {
        get { return _globalPlayerInput; }
    }
    private static bool _isInPause = false;
    public static bool IsInPause
    {
        get { return _isInPause; }
    }
    public static void Initialize()
    {
        _currentScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnSceneSwitch;
        //if (_gameMod != EGameMod.Debug)
        //{
        //    PopupManager.Initialize();
        //    Settings._language = "English";
        //    Settings.Initialize();
        //    _globalPlayerInput = GameObject.FindObjectOfType<PlayerInput>();
        //    _globalPlayerInput._PADInputContainer = Settings._GPPInputContainer;
        //    _globalPlayerInput._PCInputContainer = Settings._PCInputContainer;
        //    _globalPlayerInput.Initialize();
        //    UIManager.Instance.Initialize();
        //}
        PopupManager.Initialize();
        Settings._language = "English";
        Settings.Initialize();
        _globalPlayerInput = GameObject.FindObjectOfType<PlayerInput>();
        _globalPlayerInput._PADInputContainer = Settings._GPPInputContainer;
        _globalPlayerInput._PCInputContainer = Settings._PCInputContainer;
        _globalPlayerInput.Initialize();
        UIManager.Instance.Initialize();
    }

    public static void PauseGame()
    {
        _isInPause = true;
        Time.timeScale = 0.0f;
    }
    public static void UnpauseGame()
    {
        _isInPause = false;
        Time.timeScale = 1.0f;
    }

    public static void BackToMainMenu()
    {
        _gameMod = EGameMod.Menu;
        SceneLoader.Instance.LoadScene("Menu");
    }

    public static void OnSceneSwitch(Scene scene, LoadSceneMode loadSceneMod)
    {
        _currentScene = scene;
        GameObject.FindObjectOfType<CoopManager>().Invoke("OnSceneSwitch", 0.0f);
        if(scene.name == "Menu")
        {
            GameObject.FindObjectOfType<MainMenu>().Initialize();
        }
    }


}
