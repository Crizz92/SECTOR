using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilation : Entity {

    [SerializeField]
    protected List<PhysicalEntity> _ventilationComponentList;
    [SerializeField]
    protected List<GameObject> _needToDisappear;
    [SerializeField]
    protected ConstantRotation _constantRotation;
    protected Collider _collider;

    public override void Initialize()
    {
        base.Initialize();
        _collider = GetComponent<Collider>();

        foreach (PhysicalEntity component in _ventilationComponentList)
        {
            component.EntityRigidbody.useGravity = false;
            component.ManagerDependant = false;
            component.ForceSleep();
        }

    }

    public override void TakeDamage(Entity entity, float damage)
    {
        base.TakeDamage(entity, damage);
        EntityInfo entityInfo = entity.EntityInformations;
        if(entityInfo._type == EntityType.Drone)
        {
            _constantRotation.enabled = false;
            // Manage each ventilation component to make them react to the physics
            foreach(PhysicalEntity component in _ventilationComponentList)
            {
                component.Active = true;
                component.WakeUp();
                component.EntityRigidbody.useGravity = true;
                component.ManagerDependant = true;
                // add a random force
                Vector3 randomForce = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0) * 20.0f;
                component.EntityRigidbody.AddForce(randomForce, ForceMode.Impulse);
            }
            foreach(GameObject objectToHide in _needToDisappear)
            {
                objectToHide.SetActive(false);
            }

            _collider.enabled = false;
        }
    }
}
