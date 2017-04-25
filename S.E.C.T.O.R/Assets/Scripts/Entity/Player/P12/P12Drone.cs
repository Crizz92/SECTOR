using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P12Drone : Drone
{

    public override void Update()
    {
        base.Update();
        if(!_moving && _playerMotor.Moving)
        {
            _moving = true;

        }
        else if(_moving && !_playerMotor.Moving)
        {
            _moving = false;
        }
    }

}
