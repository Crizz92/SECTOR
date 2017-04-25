using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderData : MonoBehaviour {

    [SerializeField]
    private PhysicalEntity _owner;
    public PhysicalEntity Owner
    {
        get { return _owner; }
    }
    [SerializeField]
    private Rigidbody _ownerRigidbody;
    public Rigidbody OwnerRigidbody
    {
        get { return _ownerRigidbody; }
    }

    public void Initialize(PhysicalEntity owner)
    {
        _owner = owner;
        _ownerRigidbody = _owner.EntityRigidbody;
    }
}
