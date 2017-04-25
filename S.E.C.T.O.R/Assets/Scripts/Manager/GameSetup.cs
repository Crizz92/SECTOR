using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum EGameSetupType
{
    //Debug,
    CameraFree,
    Normal,
}

public class GameSetup : MonoBehaviour {

    public EGameSetupType _gameStartMod = EGameSetupType.Normal;

    void Start()
    {
        
        DontDestroyOnLoad(gameObject);
        switch (_gameStartMod)
        {
            //case EGameSetupType.Debug:
            //    break;
            case EGameSetupType.CameraFree:
                {

                }
                break;
            case EGameSetupType.Normal:
                {
                    GameManager.Initialize();
                    //MatchMakerManager.Initialize();
                    SceneManager.LoadScene("Menu");
                    break;
                }
            default:
                break;
        }
    }

    void Update()
    {
        EntityManager.Update();
    }
}
