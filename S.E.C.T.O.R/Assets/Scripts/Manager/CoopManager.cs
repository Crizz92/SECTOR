using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[System.Serializable]
public struct PlayerInformations
{
    public DroneType _droneType;
    public Drone _dronePrefab;
    public string _name;
    public Color _color;
    public InputContainer _input;
    public PlayerInput.PlayerIndex _controllerType;
    
}

public class CoopManager : Singleton<CoopManager> {

    [SerializeField]
    private List<PlayerInformations> _playersInfo = new List<PlayerInformations>();
    public List<PlayerInformations> PlayerInfo
    {
        get { return _playersInfo; }
    }

    [SerializeField]
    private Drone _playerOne;
    [SerializeField]
    private Drone _playerTwo;

    [SerializeField]
    private DronesManager _dronesManager;

    private LevelInformations _levelInformations;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    public void DebugInitialize()
    {
        PlayerInformations playerInfo = new PlayerInformations();
        playerInfo._color = Color.blue;
        playerInfo._dronePrefab = _playerOne;
        playerInfo._droneType = DroneType.P11;
        playerInfo._name = "P11";
        SetDroneInfo(playerInfo);

        playerInfo = new PlayerInformations();
        playerInfo._color = Color.green;
        playerInfo._dronePrefab = _playerTwo;
        playerInfo._droneType = DroneType.P12;
        playerInfo._name = "P12";
        SetDroneInfo(playerInfo);

        DronesManager dronesManager = Instantiate(_dronesManager, Vector3.zero, Quaternion.identity) as DronesManager;
        dronesManager.Initialize();
        FindObjectOfType<LevelInformations>().InitializeLevel();
        FindObjectOfType<IngameHUD>().Activate();


    }
    void InitializeCoopGame()
    {
        DronesManager dronesManager = Instantiate(_dronesManager, Vector3.zero, Quaternion.identity) as DronesManager;
        dronesManager.Initialize();
        FindObjectOfType<LevelInformations>().InitializeLevel();
        FindObjectOfType<IngameHUD>().Activate();
    }

    void InitializeDronesManager()
    {
        _dronesManager.Initialize(_playerOne, _playerTwo);
    }


    void OnSceneSwitch()
    {
        if(GameManager.GameMod == EGameMod.Coop)
        {
            InitializeCoopGame();
        }
    }

    public void SetDroneInfo(PlayerInformations playerInfo)
    {
        bool replaced = false;
        for(int i = 0; i < _playersInfo.Count; i++)
        {
            if(_playersInfo[i]._droneType == playerInfo._droneType)
            {
                _playersInfo[i] = playerInfo;
                replaced = true;
            }
        }
        if(!replaced)
        {
            _playersInfo.Add(playerInfo);
        }
    }
    public PlayerInformations GetDroneInfo(DroneType droneType)
    {
        foreach(PlayerInformations playerInfo in _playersInfo)
        {
            if(playerInfo._droneType == droneType)
            {
                return playerInfo;
            }
        }
        return new PlayerInformations();
    }
}
