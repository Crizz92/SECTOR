using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCable : MonoBehaviour {

    [SerializeField]
    private List<JointAgent> _jointList = new List<JointAgent>();

    [SerializeField]
    private JointAgent _lastJoint;

    public void Initialize()
    {

    }

    public void SetConnectedBody(Rigidbody bodyToConnect, float breakForce)
    {
        _lastJoint.transform.position = bodyToConnect.transform.position;
        _lastJoint.SetTarget(bodyToConnect, breakForce);
    }

    void Update()
    {

    }
    
}
