using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IEM : Skill {

    [SerializeField]
    protected float _radius;
    [SerializeField]
    protected int _damage;
    [SerializeField]
    protected float _actionTime;

    protected List<Entity> _entitiesAffected = new List<Entity>();

    //protected List<Entity> _entities
    public override void Activate()
    {
        base.Activate();
        CheckEntitiesAround();
    }

    protected void CheckEntitiesAround()
    {
        List<Entity> _entitiesFound = EntityManager.EntityInSphere(_owner.transform.position, _radius, EntityProperties.ElectricSensibility);
        foreach (Entity entity in _entitiesFound)
        {
            if (entity)
            {
                IElectricSensibility castedEntity = (IElectricSensibility)entity;
                castedEntity.Electrocute();
            }
        }
        _entitiesAffected = _entitiesFound;
        Invoke("RebootEntities", _actionTime);
    }
    
    protected void RebootEntities()
    {
        foreach(Entity entity in _entitiesAffected)
        {
            if(entity)
            {
                IElectricSensibility castedEntity = (IElectricSensibility)entity;
                castedEntity.Reboot();
            }
        }
        _entitiesAffected.Clear();
    }
}
