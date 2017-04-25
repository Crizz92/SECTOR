using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public enum EntityProperties
{
    ElectricSensibility = 0,
    Floating,
    LavaSensibility,
    MagnetSensibility,
}

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(EntityInfo))]
public class Entity : MonoBehaviour {

    public bool _sleepingWhenFar = true;
    protected bool _sleeping = false;
    public bool Sleeping
    {
        get { return _sleeping; }
    }

    public bool _killed = false;
    public Stats _stats;
    [SerializeField]
    protected EntityInfo _entityInfo;
    public EntityInfo EntityInformations
    {
        get { return _entityInfo; }
    }
    // Define the properties of the entity
    [SerializeField]
    protected List<EntityProperties> _properties = new List<EntityProperties>();
    public List<EntityProperties> Properties
    {
        get { return _properties; }
    }
    // Manager dependant says if the entity is manage by the Entity Manager or not
    [SerializeField]
    protected bool _managerDependant = true;
    public bool ManagerDependant
    {
        get { return _managerDependant; }
        set { _managerDependant = value; }
    }


    public virtual void Initialize()
    {
        if (!_stats) _stats = GetComponent<Stats>();
        if (!_entityInfo) _entityInfo = GetComponent<EntityInfo>();
        _sleeping = false;
    }

    public virtual void Awake()
    {
        if (!_stats) _stats = GetComponent<Stats>();
        if (!_entityInfo) _entityInfo = GetComponent<EntityInfo>();
    }

	public virtual void Start () {
	
	}

	public virtual void Update () {
	
	}

    public virtual void FixedUpdate()
    {

    }

    public virtual void WakeUp()
    {
        _sleeping = false;
    }

    public virtual void ForceSleep()
    {
        _sleeping = true;
    }

    public virtual void Sleep()
    {
        _sleeping = true;
    }

    public virtual void TakeDamage(Entity entity, float damage)
    {
        _stats.TakeDamage(damage);
    }
    public virtual void RefundHealth(float health)
    {
        _stats.RefundHealth(health);
    }
}
