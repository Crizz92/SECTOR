using UnityEngine;
using System.Collections;

public class Water : Entity {

    private BoxCollider _waterCollider;
    protected BuoyancyForce _buoyancy;
    [SerializeField]
    private int _damagePerSec = 5;
    private BuoyancyForce _buoyancyEffect;
    public BuoyancyForce BuoyancyEffect
    {
        get { return _buoyancy; }
    }

    [SerializeField]
    private Transform _bottomLeft;
    [SerializeField]
    private Transform _topRight;

    public Vector2 GetBottomLeftLimit()
    {
        return _bottomLeft.position;
    }
    public Vector2 GetTopRightLimit()
    {
        return _topRight.position;
    }

    public override void Initialize()
    {
        base.Initialize();
        _waterCollider = GetComponent<BoxCollider>();
        _buoyancyEffect = GetComponent<BuoyancyForce>();
    }

    public override void Sleep()
    {
        base.Sleep();
    }

    public override void WakeUp()
    {
        base.WakeUp();
    }

    void OnTriggerStay(Collider col)
    {
        Drone drone = col.GetComponent<Drone>();
        if(drone)
        {
            drone.TakeDamage(this, _damagePerSec * Time.deltaTime);
        }
    }
}
