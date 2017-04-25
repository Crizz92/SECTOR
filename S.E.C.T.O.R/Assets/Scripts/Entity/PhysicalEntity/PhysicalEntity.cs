using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class PhysicalEntity : Entity {

    protected Collider _collider;
    protected Rigidbody _rigidbody;
    public Rigidbody EntityRigidbody
    {
        get { return _rigidbody; }
    }
    [SerializeField]
    protected Collider[] _allColliders = new Collider[3];
    [SerializeField]
    protected Rigidbody[] _allRigidbodies = new Rigidbody[3];
    private bool _rigidbodyCanSleep = false;
    private float _lastMovement = 0.0f;
    [SerializeField]
    protected bool _active = true;
    public bool Active
    {
        get { return _active; }
        set
        {
            if(value)
            {
                Sleep();
            }
            else
            {
                WakeUp();
            }
            _active = value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        Collider[] colliderFound = GetComponentsInChildren<Collider>();
        Rigidbody[] rigidbodyFound = GetComponentsInChildren<Rigidbody>();
        Helpers.AddArrayToArray(_allColliders, colliderFound);
        Helpers.AddArrayToArray(_allRigidbodies, rigidbodyFound);
        foreach (Collider col in _allColliders)
        {
            if (col && !col.GetComponent<ColliderData>())
            {
                ColliderData colliderData = col.gameObject.AddComponent<ColliderData>();
                colliderData.Initialize(this);
            }
        }
        if (!_active)
        {
            Sleep();
        }
    }
    public override void Start()
    {
        base.Start();

        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        Collider[] colliderFound = GetComponentsInChildren<Collider>();
        Rigidbody [] rigidbodyFound = GetComponentsInChildren<Rigidbody>();
        Helpers.AddArrayToArray(_allColliders, colliderFound);
        Helpers.AddArrayToArray(_allRigidbodies, rigidbodyFound);
        foreach(Collider col in _allColliders)
        {
            if(col)
            {
                ColliderData colliderData = col.gameObject.AddComponent<ColliderData>();
                colliderData.Initialize(this);
            }
        }
        if(!_active)
        {
            Sleep();
        }
    }
    public override void Update()
    {
        if(_active)
        {
            base.Update();
            if (_rigidbodyCanSleep && _rigidbody.velocity.magnitude < 0.3f)
            {
                _lastMovement += Time.deltaTime;
                if (_lastMovement > 0.5f)
                {
                    ForceSleep();
                }
            }
        }
    }
    public override void ForceSleep()
    {
        if (_active && !_sleeping)
        {
            base.ForceSleep();
            foreach (Collider collider in _allColliders)
            {
                if (collider) collider.enabled = false;
            }
            foreach (Rigidbody rigidbody in _allRigidbodies)
            {
                if (rigidbody)
                {
                    _rigidbodyCanSleep = false;
                    _rigidbody.Sleep();
                }
            }
        }
    }
    public override void Sleep()
    {
        if(_active && !_sleeping)
        {
            base.Sleep();
            foreach (Rigidbody rigidbody in _allRigidbodies)
            {
                if (rigidbody) _rigidbodyCanSleep = true;
            }
        }
    }

    public override void WakeUp()
    {
        if(_active && _sleeping)
        {
            base.WakeUp();
            foreach (Collider collider in _allColliders)
            {
                if (collider) collider.enabled = true;
            }
            foreach (Rigidbody rigidbody in _allRigidbodies)
            {
                if (rigidbody)
                {
                    _rigidbodyCanSleep = false;
                    rigidbody.WakeUp();
                    _lastMovement = 0.0f;
                }
            }
        }
    }
}
