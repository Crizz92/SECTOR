using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointAgent : MonoBehaviour {

    private bool _broken = false;
    public bool Broken
    {
        get { return _broken; }
    }

    [SerializeField]
    private ConfigurableJoint _joint;
    public Rigidbody ConnectedBody
    {
        get { return _joint.connectedBody; }
        set { _joint.connectedBody = value; }
    }


    void Awake()
    {
        _joint = GetComponent<ConfigurableJoint>();
    }

    public void SetTarget(Rigidbody bodyToConnect, float breakForce)
    {
        if (!_joint) _joint = gameObject.AddComponent<ConfigurableJoint>();
        Initialize();
        _joint.breakForce = breakForce;
        _joint.connectedBody = bodyToConnect;
    }

    void Initialize()
    {
        _joint.autoConfigureConnectedAnchor = true;
        _joint.axis = Vector3.forward;
        _joint.xMotion = _joint.yMotion = _joint.zMotion = ConfigurableJointMotion.Locked;
        _joint.angularXMotion = _joint.angularYMotion = ConfigurableJointMotion.Locked;
        _joint.angularZMotion = ConfigurableJointMotion.Free;
        _joint.projectionMode = JointProjectionMode.PositionAndRotation;
    }

    void OnJointBreak(float breakForce)
    {
        _broken = true;
    }
}
