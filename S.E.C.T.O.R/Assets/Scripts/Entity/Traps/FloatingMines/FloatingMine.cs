using UnityEngine;
using System.Collections;

public class FloatingMine : Trap {

    public int _damage = 10;

    void OnTriggerEnter(Collider col)
    {
        Drone drone = col.GetComponent<Drone>();
        if(drone)
        {

        }
    }
}
