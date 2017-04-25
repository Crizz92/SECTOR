using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public static class MatchMakerManager {

    enum EOnlineMod
    {
        Offline,
        ServerWaiting,
        Connected,
    }

    [SerializeField]
    private static string _status;
    public static string Status
    {
        get { return _status; }
    }
    private static NetworkManager _networkManager;
    private static bool _searching = false;
    private static EOnlineMod _onlineMod = EOnlineMod.Offline;
    public static string _sceneToLoad = "1-1BunkerArea";
    private static List<RoomItem> _roomList = new List<RoomItem>();

    #region Debug
    private static Text _typeInfo;
    #endregion

    // Use this for initialization
    public static void Initialize () {
        _networkManager = NetworkManager.singleton;
        if (_networkManager.matchMaker == null)
        {
            _networkManager.StartMatchMaker();
        }

        _typeInfo = GameObject.Find("TypeInfo").GetComponent<Text>();

    }

    public static void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Someone joined"); 
    }

    public static void Update()
    {
        //_typeInfo.text = (Network.isServer) ? "Server" : "Client";
        switch (_onlineMod)
        {
            default:
            case (EOnlineMod.ServerWaiting):
                {
                    if (_networkManager.numPlayers > 1)
                    {
                        _networkManager.ServerChangeScene(_sceneToLoad);
                        _status = "Found a partner, connecting...";
                    }
                    break;
                }
            case (EOnlineMod.Connected):
                {
                    break;
                }
        }
    }

    #region Online Management

    public static void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("Changing Scene");
        _status = "Found a partner, connecting...";
        ClientScene.Ready(conn);
    }

    //call this method to request a match to be created on the server
    public static void CreateInternetMatch(string matchName)
    {
        
        NetworkManager.singleton.matchMaker.CreateMatch(matchName, 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
    }

    //this method is called when your request for creating a match is returned
    private static void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        
        if (success)
        {
            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);
            NetworkManager.singleton.StartHost(hostInfo);
            GameManager.SetGameMod(EGameMod.Online);
            _onlineMod = EOnlineMod.ServerWaiting;

        }
        else
        {
            _status = "Matchmaking failed... try to check your connection...";
        }
    }
    public static void StartResearch()
    {
        RefreshRoomList();
        
    }
    public static void FindPartner()
    {
        _searching = true;
        if(_roomList.Count == 0)
        {
            _status = "There is no other player available... Waiting for one";
            CreateInternetMatch("default");
        }
        else
        {
            _status = "Found a partner, connecting...";
            _networkManager.matchMaker.JoinMatch(_roomList[0]._matchDesc.networkId, "", "", "", 0, 0, OnMatchJoin);
        }
    }
    public static void OnMatchJoin(bool success, string extendInfo, MatchInfo matchInfo)
    {
        if(success)
        {
            _status = "Found a partner, connecting...";
            MatchInfo hostInfo = matchInfo;
            _networkManager.StartClient(hostInfo);
            GameManager.SetGameMod(EGameMod.Online);
            _networkManager.ServerChangeScene(_sceneToLoad);
            _typeInfo.text = (Network.isServer) ? "Server" : "Client";
        }
    }

    public static void RefreshRoomList()
    {
        _networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnInternetMatchList);
        _status = "Searching for a partner...";
    }

    private static void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {

        _status = "";
        if (!success || matchList == null)
        {
            _status = "Couldn't retrieve partner list, check out your connection...";
        }
        ClearRoomList();

        foreach (MatchInfoSnapshot match in matchList)
        {
            if (match.currentSize > 1) continue;
            RoomItem room = new RoomItem();
            room.Setup(match);
            _roomList.Add(room);
        }
        FindPartner();
    }

    private static void ClearRoomList()
    {
        _roomList.Clear();
    }

    #endregion

    #region Offline Management
    public static void LaunchCoop(string level)
    {
        _networkManager.StartHost();
        GameManager.SetGameMod(EGameMod.Coop);
        //SceneManager.LoadScene(level);
        SceneLoader.Instance.LoadScene(level);
    }
    #endregion
}
