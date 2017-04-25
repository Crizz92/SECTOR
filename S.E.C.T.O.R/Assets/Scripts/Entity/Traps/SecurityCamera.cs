using UnityEngine;
using System.Collections;

public class SecurityCamera : Trap {

    [SerializeField]
    protected float _minAngle;
    [SerializeField]
    protected float _maxAngle;
    protected float _currentAngle;
    [SerializeField]
    protected float _scanDistance;
    private float _direction;

    protected ConstantRotation _constantRotation;
    [SerializeField]
    protected Transform _securityCamera;
    [SerializeField]
    protected Transform _cameraPointOfView;
    [SerializeField]
    protected Transform _scanPlane;
    protected MaterialModifier _materialModifier;

    public override void Initialize()
    {
        base.Initialize();
        _materialModifier = _scanPlane.GetComponent<MaterialModifier>();
    }

    //public override void Update()
    //{
    //    base.Update();
    //
    //    CheckInRange();
    //}

    protected virtual void CheckInRange()
    {
        RaycastHit hit;
        if(Physics.Raycast(_cameraPointOfView.position, _cameraPointOfView.forward, out hit, _scanDistance, 1 << LayerMask.NameToLayer("Environment")))
        {
            Vector3 fromCameraToHit = hit.point - _cameraPointOfView.position;
            _scanPlane.localScale = new Vector3(1, 1, fromCameraToHit.magnitude);
            
        }
        if (Physics.Raycast(_cameraPointOfView.position, _cameraPointOfView.forward, out hit, _scanDistance, 1 << LayerMask.NameToLayer("Player")))
        {
            _materialModifier.SetAmbientColor(Color.red);
        }
        else
        {
            _materialModifier.SetAmbientColor(Color.blue);
        }

    }
}
