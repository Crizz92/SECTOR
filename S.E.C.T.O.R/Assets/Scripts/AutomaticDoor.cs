using UnityEngine;
using System.Collections;

public class AutomaticDoor : MonoBehaviour {

    private Drone[] _playerOnTrigger = new Drone[4];

    [SerializeField]
    private Animator _door;
    private bool _open;


    void OnTriggerEnter(Collider col)
    {
        Drone player = col.GetComponent<Drone>();
        bool alreadyPlayer = false;
        bool alreadyAddedPlayer = false;
        if(player)
        {
            for(int i = 0; i < _playerOnTrigger.Length; i++)
            {
                if(_playerOnTrigger[i] != null)
                {
                    alreadyPlayer = true;
                }
                if(_playerOnTrigger[i] == null && !alreadyAddedPlayer)
                {
                    _playerOnTrigger[i] = player;
                    alreadyAddedPlayer = true;
                }
            }
        }

        if (!alreadyPlayer && player)
        {
            _open = true;
            _door.SetTrigger("Open");
        }
    }

    void OnTriggerExit(Collider col)
    {
        Drone player = col.GetComponent<Drone>();
        bool stillPlayer = false;
        if(player)
        {
            for (int i = 0; i < _playerOnTrigger.Length; i++)
            {
                if (_playerOnTrigger[i] == player)
                {
                    _playerOnTrigger[i] = null;
                }
                if(_playerOnTrigger[i] != null)
                {
                    stillPlayer = true;
                }
            }
        }
        
        if(!stillPlayer && player)
        {
            _open = false;
            _door.SetTrigger("Close");
        }
    }
}
