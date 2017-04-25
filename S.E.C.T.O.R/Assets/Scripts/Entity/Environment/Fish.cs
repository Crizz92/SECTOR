using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish : Entity
{

    enum EFishState
    {
        Idle = 0,
        Normal,
        Escape,
    }

    [SerializeField]
    private float _checkTime = 1.0f;
    [SerializeField]
    private float _minNormalSpeed = 1.0f;
    [SerializeField]
    private float _maxNormalSpeed = 3.0f;
    private float _currentNormalSpeed = 0.0f;
    [SerializeField]
    private float _escapeSpeed = 6.0f;
    [SerializeField]
    private float _oneTravelMinDistance = 5.0f;
    [SerializeField]
    private float _oneTravelMaxDistance = 10.0f;

    // Idle 
    [SerializeField]
    private float _minTimeIdleMod = 2.0f;
    [SerializeField]
    private float _maxTimeIdleMod = 5.0f;
    private float _idleTimeRemaining = 0.0f;
    // Normal


    // Escape
    private bool _searchingForDestination = false;
    private bool _escapeState = false;
    [SerializeField]
    private float _afraidArea = 4.0f;
    private Vector3 _movementDirection = new Vector3(1, 0, 0);
    private Vector3 _destination;
    private EFishState _state;
    private FishManager _fishManager;
    private Water _water;
    private float _nextLookAround = 0.1f;


    #region General function
    public virtual void Initialize(FishManager fishManager)
    {
        base.Initialize();
        _fishManager = fishManager;
        _water = _fishManager.Water;
        _state = EFishState.Idle;
        _idleTimeRemaining = Random.Range(_minTimeIdleMod, _maxTimeIdleMod);
    }
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        CheckState();

        if(_nextLookAround > 0.0f)
        {
            _nextLookAround -= Time.deltaTime;
            if(_nextLookAround <= 0.0f)
            {
                LookAround();
                _nextLookAround = Random.Range(0.2f, 0.4f);
            }
        }
    }

    public override void Sleep()
    {
        base.Sleep();
    }

    public override void WakeUp()
    {
        base.WakeUp();
    }
    #endregion

    private void CheckState()
    {
        
        switch (_state)
        {
            case EFishState.Idle:
                {
                    if(_searchingForDestination)
                    {
                        FindDestination();
                        _state = EFishState.Normal;
                        _searchingForDestination = false;
                        _currentNormalSpeed = Random.Range(_minNormalSpeed, _maxNormalSpeed);
                    }
                    else
                    {
                        _idleTimeRemaining -= Time.deltaTime;
                        if(_idleTimeRemaining <= 0.0f)
                        {
                            _idleTimeRemaining = 0.0f;
                            _searchingForDestination = true;
                        }
                    }
                    break;
                }
            case EFishState.Normal:
                {
                    Move(_currentNormalSpeed);
                    break;
                }
            case EFishState.Escape:
                {
                    Move(_escapeSpeed);
                    break;
                }
        }
    }

    private void Move(float speed)
    {
        Vector3 toDestination = _destination - transform.position;
        Vector3 movement = toDestination.normalized * speed * Time.deltaTime;

        if(movement.magnitude >= toDestination.magnitude)
        {
            transform.position = _destination;
            _state = EFishState.Idle;
            _idleTimeRemaining = Random.Range(_minTimeIdleMod, _maxTimeIdleMod);
        }
        else
        {
            transform.position += movement;
        }
    }

    private void FindDestination()
    {
        Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
        direction.Normalize();
        FindDestinationInDirection(direction);
    }

    private void FindDestinationInDirection(Vector3 direction)
    {
        direction.z = 0.0f;
        direction.Normalize();
        float distance = Random.Range(_oneTravelMinDistance, _oneTravelMaxDistance);
        Vector3 destination = transform.position + direction * distance;
        
        // Check environment
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distance, 1 << LayerMask.NameToLayer("Environment")))
        {
            destination = hit.point - direction * 1.0f;
        }

        // Correct the destination
        destination = Helpers.ClampInRectangle(destination, _water.GetBottomLeftLimit(), _water.GetTopRightLimit());

        _destination = destination;

        // Set looking direction
        if (_destination.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else transform.rotation = Quaternion.Euler(0, 0, 0);
        
    }

    private void Escape(Vector3 direction)
    {
        FindDestinationInDirection(direction);
        _state = EFishState.Escape;
    }
    private void LookAround()
    {
        List<PhysicalEntity> physicalEntityFound = EntityManager.EntityInSphere<PhysicalEntity>(transform.position, _afraidArea);
        foreach(PhysicalEntity physicalEntity in physicalEntityFound)
        {
            if(physicalEntity.EntityRigidbody.velocity.magnitude > 2.0f)
            {
                Vector3 direction = transform.position - physicalEntity.transform.position;
                Escape(direction);
                return;
            }
        }
    }
}
