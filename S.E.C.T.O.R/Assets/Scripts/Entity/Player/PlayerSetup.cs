using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    private Behaviour[] _componentsToDisable;

    // Use this for initialization
    void Start()
    {
        if (GameManager.GameMod == EGameMod.Online && !isLocalPlayer)
        {
            for (int i = 0; i < _componentsToDisable.Length; i++)
            {
                if (_componentsToDisable[i])
                {
                    _componentsToDisable[i].enabled = false;
                }
            }
        }
        GameObject.Find("Main Camera").GetComponent<InGameCamera>().AddTarget(transform);
    }
}
