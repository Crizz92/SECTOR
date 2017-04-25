using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLight : PhysicalEntity {

    [SerializeField]
    private List<CableLineRender> _cableLineRender;
    [SerializeField]
    private List<Joint> _joints;
    [SerializeField]
    private LightController _lightController;

    public override void Sleep()
    {
        base.Sleep();
    }
    public override void ForceSleep()
    {
        base.ForceSleep();
        if (_active)
        {
            foreach (CableLineRender cable in _cableLineRender)
            {
                if (cable) cable.Deactivate();
            }
            foreach (Joint joint in _joints)
            {

            }
            _lightController.Disable();
        }
    }
    public override void WakeUp()
    {
        base.WakeUp();
        if (_active)
        {
            foreach (CableLineRender cable in _cableLineRender)
            {
                if (cable) cable.Activate();
            }
            foreach (Joint joint in _joints)
            {

            }
            _lightController.Enable();
        }
    }
}
