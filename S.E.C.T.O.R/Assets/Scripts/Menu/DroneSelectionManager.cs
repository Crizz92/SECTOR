using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSelectionManager : MonoBehaviour {

    public MainMenu _mainMenu;
    public List<DroneSelectionTab> _droneSelectionTabs;
    public List<MaterialModifier> _dronePreview;
    public List<PlayerInformations> _info;
    public PlayerInformations _currentInfo;
    public bool _invertedPlayer = false;
    public int _currentPlayer = 1;
    public MenuTab _tabWhenDone;
	// Use this for initialization
	void Start () {
        _currentInfo = _info[0];
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void InvertDrone()
    {
        if(_invertedPlayer)
        {
            _invertedPlayer = false;
            _currentInfo = _info[0];
        }
        else
        {
            _invertedPlayer = true;
            _currentInfo = _info[1];
        }
        
    }
    public void GoToSecondPlayer()
    {
        _currentInfo._color = _droneSelectionTabs[0].GetColor();
        CoopManager.Instance.SetDroneInfo(_currentInfo);
        _droneSelectionTabs[1].gameObject.SetActive(true);
        _mainMenu.GoToTab(_droneSelectionTabs[1]._name);
        _droneSelectionTabs[1].SwitchDrone();
        _droneSelectionTabs[0].gameObject.SetActive(false);
        //if (_invertedPlayer)
        //{
        //    _currentInfo = _info[0];
        //}
        //else _currentInfo = _info[1];

        _currentPlayer = 2;
    }

    public void Validate()
    {
        _currentInfo._color = _droneSelectionTabs[1].GetColor();
        CoopManager.Instance.SetDroneInfo(_currentInfo);
        _mainMenu.GoToTab(_tabWhenDone._name);
    }
    public void GoBack()
    {
        if(_currentPlayer == 1)
        {
            _mainMenu.GoToTab(_droneSelectionTabs[0]._previousMenuTab._name);
        }
        else
        {
            _droneSelectionTabs[0].gameObject.SetActive(true);
            _mainMenu.GoToTab(_droneSelectionTabs[0]._name);
            _droneSelectionTabs[1].gameObject.SetActive(false);
            _currentPlayer = 1;
        }
    }

}
