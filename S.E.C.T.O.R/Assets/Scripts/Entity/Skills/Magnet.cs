using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Magnet : Skill {

    public float _radius = 10.0f;
    public float _magnetizeForce = 8.0f;
    protected bool _activated = false;
    protected List<PhysicalEntity> _alreadyMagnetize = new List<PhysicalEntity>();

    public override void Activate()
    {
        base.Activate();
        if(_activated)
        {
            _activated = false;
            DeactivateMagnet();
        }
        else if(!_activated)
        {
            _activated = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        if(_activated)
        {
            List<PhysicalEntity> _entitiesFound = EntityManager.EntityInSphere<PhysicalEntity>(_owner.transform.position, _radius, EntityProperties.MagnetSensibility);
            foreach(PhysicalEntity entity in _entitiesFound)
            {
                bool alreadyKnown = false;
                foreach(PhysicalEntity knownEntity in _alreadyMagnetize)
                {
                    if (entity == knownEntity || entity == _owner)
                    {
                        alreadyKnown = true;
                        break;
                    }
                }

                if (!alreadyKnown)
                {
                    // Joint Configuration
                    ConfigurableJoint joint = entity.gameObject.AddComponent<ConfigurableJoint>();
                    joint.connectedBody = _owner.GetComponent<Rigidbody>();
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedAnchor = Vector3.zero;
                    JointDrive jointDrive = joint.xDrive;
                    jointDrive.positionSpring = 50.0f;
                    jointDrive.positionDamper = 1.0f;
                    jointDrive.maximumForce = 100.0f;
                    joint.xDrive = jointDrive;
                    joint.yDrive = jointDrive;
                    joint.zDrive = jointDrive;
                    joint.enableCollision = true;
                    _alreadyMagnetize.Add(entity);
                }
            }
        }
    }

    protected void DeactivateMagnet()
    {
        foreach(PhysicalEntity entity in _alreadyMagnetize)
        {
            Destroy(entity.gameObject.GetComponent<ConfigurableJoint>());
        }
        _alreadyMagnetize.Clear();
    }

    protected void CreateMagnetizeLink()
    {

    }
}
