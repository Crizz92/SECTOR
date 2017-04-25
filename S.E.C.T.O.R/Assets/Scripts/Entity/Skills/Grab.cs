using UnityEngine;
using System.Collections;


public class Grab : Skill {
    
    public float _grabDistance = 1.0f;
    [SerializeField]
    private FixedJoint _grabJoint = null;
    [SerializeField]
    private LineRenderer _tractionLine = null;
    private bool _draggingSomething = false;
    private Transform _tractionPoint = null;


    void OnDrawGizmos()
    {
        RaycastHit hit;
        if (Physics.Raycast(_owner.transform.position, -_owner.transform.up, out hit, _grabDistance, 1 << LayerMask.NameToLayer("Object")))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.5f);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        _grabJoint.connectedAnchor = Vector3.zero;
        _tractionLine.enabled = false;
        if(!_tractionPoint) _tractionPoint = new GameObject().transform;
    }

    protected override void Update()
    {
        base.Update();
        if(_tractionLine && _tractionPoint && _draggingSomething)
        {
            _tractionLine.SetPosition(0, _tractionLine.transform.position);
            _tractionLine.SetPosition(1, _tractionPoint.position);
        }
    }

    public override void Activate()
    {
        if(_unlocked)
        {
            if(!_draggingSomething)
            {
                RaycastHit hit;
                if (Physics.Raycast(_owner.transform.position, -_owner.transform.up, out hit, _grabDistance, 1 << LayerMask.NameToLayer("Object")))
                {
                    ColliderData colliderData = hit.collider.GetComponent<ColliderData>();
                    _grabJoint.connectedBody = colliderData.OwnerRigidbody;
                    _grabJoint.connectedAnchor = colliderData.OwnerRigidbody.transform.InverseTransformPoint(hit.point);
                    _draggingSomething = true;
                    _tractionLine.enabled = true;
                    _tractionPoint.position = hit.point;
                    _tractionPoint.SetParent(colliderData.transform);
                    
                }
            }
            else
            {
                _grabJoint.connectedBody = null;
                _grabJoint.connectedAnchor = Vector3.zero;
                _draggingSomething = false;

                _tractionLine.enabled = false;
                _tractionPoint.position = transform.position;
                _tractionPoint.SetParent(transform);
            }
        }
    }
}
