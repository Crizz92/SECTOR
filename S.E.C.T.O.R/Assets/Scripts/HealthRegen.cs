using UnityEngine;
using System.Collections;

public class HealthRegen : MonoBehaviour {

    [SerializeField]
    private int _healthRefund = 20;

    void OnTriggerEnter(Collider col)
    {
        Drone drone = col.GetComponent<Drone>();
        if(drone)
        {
            if(FindObjectOfType<DronesManager>()._stats.HealthRatio < 1)
            {
                drone.RefundHealth(_healthRefund);
                Destroy(gameObject);
            }
        }
    }
}
