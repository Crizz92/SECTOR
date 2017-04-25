using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class DebugGameManager : MonoBehaviour 
{
    public DronesManager _dronesManager;

    void Awake()
    {
        GameManager.SetGameMod(EGameMod.Debug);
        GameManager.Initialize();

    }
	void Start () {

        Settings._language = "English";
        Settings.Initialize();
        PopupManager.Initialize();
        Initialize();
    }

    void Update()
    {
        EntityManager.Update();
    }

    void Initialize()
    {
        CoopManager.Instance.DebugInitialize();
        EntityManager.Initialize();
        FindObjectOfType<InGameCamera>().TargetPlayers();
        Minimap.Instance.Display(true);
    }

}
