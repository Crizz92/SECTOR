using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {

    private bool _canDealDmg = false;
    private SpikeTrap _spikeTrap;

    public void Initialize(SpikeTrap owner)
    {
        _spikeTrap = owner;
    }

    public void CanDealDamage(bool dealDamage)
    {
        _canDealDmg = dealDamage;
    }

    void OnTriggerEnter(Collider col)
    {
        if(_canDealDmg)
        {
            Drone drone = col.GetComponent<Drone>();
            if(drone)
            {
                drone.TakeDamage(_spikeTrap, _spikeTrap._stats._damage);
            }
        }
    }
}

