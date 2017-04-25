using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevelScenario : LevelScenario {



    public override void Initialize()
    {
        base.Initialize();

        FindObjectOfType<DronesManager>().DroneP2.DroneAnimator.Play("P12_Idle");
        FindObjectOfType<DronesManager>().DroneP1.DroneAnimator.Play("P11_Idle");
    }
}
