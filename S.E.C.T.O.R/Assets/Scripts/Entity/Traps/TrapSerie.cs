using UnityEngine;
using System.Collections;

public class TrapSerie : Trap {

    [SerializeField]
    private Trap[] _trapList;

    public override void Start()
    {
        base.Start();
        foreach(Trap trap in _trapList)
        {
            trap._sleepingWhenFar = false;
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        foreach (Trap trap in _trapList)
        {
            trap.Sleep();
        }
    }

    public override void WakeUp()
    {
        base.WakeUp();
        foreach (Trap trap in _trapList)
        {
            trap.WakeUp();
        }
    }
}
