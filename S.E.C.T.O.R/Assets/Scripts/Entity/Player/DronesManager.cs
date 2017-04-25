using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DronesManager : MonoBehaviour {
    [SerializeField]
    private Drone _playerOnePrefab;
    [SerializeField]
    private Drone _playerTwoPrefab;

    private Drone _droneP1;
    public Drone DroneP1
    {
        get { return _droneP1; }
    }
    private Drone _droneP2;
    public Drone DroneP2
    {
        get { return _droneP2; }
    }

    public Stats _stats;
    [SerializeField]
    private float _maxDroneDistance = 15.0f;

    public Vector3 _center;

    [SerializeField]
    private DronesLink _dronesLink;
    [SerializeField]
    private LimitCircle _limitCircle;


    [SerializeField]
    private float _timeAllowedOutOfRange = 2.0f;

    private float _dronesDistance = 0.0f;
    public float DronesDistance
    {
        get { return _dronesDistance; }
    }

    private bool _droneVisible = true;

    public void Initialize(Drone playerOne, Drone playerTwo)
    {
        _playerOnePrefab = playerOne;
        _playerTwoPrefab = playerTwo;
        Initialize();
    }
    public void Initialize()
    {
        transform.position = Vector3.zero;
        InitializeDrones();
    }

	
    void InitializeDrones()
    {
        EntityManager.Activate();
        LevelInformations levelInfo = FindObjectOfType<LevelInformations>();


        _droneP1 = Instantiate(CoopManager.Instance.PlayerInfo[0]._dronePrefab, levelInfo.GetCheckpoint().SpawnPosition(0), Quaternion.identity) as Drone;
        _droneP2 = Instantiate(CoopManager.Instance.PlayerInfo[1]._dronePrefab, levelInfo.GetCheckpoint().SpawnPosition(1), Quaternion.identity) as Drone;

        if(_droneP1 && _droneP2) _dronesLink.SetLink(_droneP1.transform.position, _droneP2.transform.position);

        _droneP1.transform.SetParent(transform);
        _droneP2.transform.SetParent(transform);

        _droneP1.Initialize(this);
        _droneP2.Initialize(this);

        _dronesLink.Initialize();
        _dronesLink.SetLink(_droneP1.transform.position, _droneP2.transform.position);
        _limitCircle.Initialize();
        _limitCircle.SetLimit(_maxDroneDistance);
        
        _stats = GetComponent<Stats>();
        DronesHealthBar dronesHealthbar = FindObjectOfType<DronesHealthBar>();
        dronesHealthbar.Initialize(this);
        dronesHealthbar.Show();
    }

	// Update is called once per frame
	void Update () {
        if(GameManager.GlobalPlayerInput.LeftJoystickDown())
        {
            DisplayDrones(!_droneVisible);
        }
        if (_droneVisible)
        {
            CheckPlayerDistance();
            UpdateLimit();
        }
	}

    public void TakeDamage(Entity entity, float damage)
    {
        if (_droneVisible)
        {
            _stats.TakeDamage(damage);
            if (_stats.HealthRatio <= 0.0f)
            {
                FindObjectOfType<InGameMenu>().BackToMainMenu();
            }
        }
    }
    public void RefundHealth(float health)
    {
        _stats.RefundHealth(health);
    }

    void CheckPlayerDistance()
    {
        if (_droneP1 && _droneP2)
        {
            _center = (_droneP2.transform.position + _droneP1.transform.position) / 2.0f;
            _dronesDistance = Vector3.Magnitude(_droneP2.transform.position - _droneP1.transform.position);
            if (_dronesDistance > _maxDroneDistance)
            {
                StartCoroutine("DronesMissingSignal");
            }
            else
            {
                StopCoroutine("DronesMissingSignal");
            }
        }
    }

    public void DeactivateHUD()
    {
        _droneP1.DestroyEnergyBar();
        _droneP2.DestroyEnergyBar();
    }

    IEnumerator DronesMissingSignal()
    {
        float currentTimePassed = _timeAllowedOutOfRange;
        while(currentTimePassed > 0.0f)
        {
            currentTimePassed -= Time.deltaTime;
            yield return null;
        }
        _droneP1.Disconnect();
        _droneP2.Disconnect();
    }

    private void UpdateLimit()
    {

        if (_droneP1 && _droneP2)
        {
            _dronesLink.SetLink(_droneP1.transform.position, _droneP2.transform.position);
            _limitCircle.SetPosition(_center);
            _dronesLink.LimitDistanceRatio(_dronesDistance / _maxDroneDistance);
            _limitCircle.LimitDistanceRatio(_dronesDistance / _maxDroneDistance);
        }

       
    }

    public void DisplayDrones(bool display)
    {
        _droneVisible = display;
        _droneP1.Display(display);
        _droneP2.Display(display);
        _dronesLink.gameObject.SetActive(display);
        FindObjectOfType<Minimap>().Display(display);
        if (display) FindObjectOfType<DronesHealthBar>().Show();
        else FindObjectOfType<DronesHealthBar>().Hide();
        FindObjectOfType<LocationIndicator>().Display(display);
    }
}
