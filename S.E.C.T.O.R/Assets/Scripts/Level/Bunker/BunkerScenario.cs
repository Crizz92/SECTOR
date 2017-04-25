using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BunkerScenario : LevelScenario {

    private DronesManager _dronesManager;
    [SerializeField]
    private ListOfMessage _messageList;

    [SerializeField]
    private List<PhysicalCable> _cableSpawnP1;
    [SerializeField]
    private List<PhysicalCable> _cableSpawnP2;
    private InGameCamera _ingameCamera;


    public override void Initialize()
    {
        base.Initialize();
        _dronesManager = FindObjectOfType<DronesManager>();
        _ingameCamera = FindObjectOfType<InGameCamera>();
        _ingameCamera.Initialize();
        _ingameCamera.TargetPlayers();
        StartPhase();
    }

    public void DisablePlayer()
    {
        _dronesManager.DroneP1.DisableCompletly();
        _dronesManager.DroneP2.DisableCompletly();
    }
    public void ActivatePlayer()
    {
        _dronesManager.DroneP1.EnableCompletly(false);
        _dronesManager.DroneP2.EnableCompletly(false);
    }

    public void StartPhase()
    {
        DisablePlayer();
        _messageList.Activate();
        Invoke("ActivatePlayer", 3.0f);

        foreach(PhysicalCable cable in _cableSpawnP1)
        {
            cable.Initialize();
            cable.SetConnectedBody(FindObjectOfType<DronesManager>().DroneP2.EntityRigidbody, 1000);
        }
        foreach (PhysicalCable cable in _cableSpawnP2)
        {
            cable.Initialize();
            cable.SetConnectedBody(FindObjectOfType<DronesManager>().DroneP1.EntityRigidbody, 1100);
        }

    }
}
