using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lava : Entity {

    [SerializeField]
    protected BuoyancyForce _buoyancy;
    protected List<PhysicalEntity> _inLavaEntities = new List<PhysicalEntity>();
    public override void Initialize()
    {
        base.Initialize();
        if (!_buoyancy) _buoyancy = GetComponent<BuoyancyForce>();
    }
    public override void Update()
    {
        //base.Update();
        //
        //_inLavaEntities = _buoyancy.FloatingEntities;
        //foreach (PhysicalEntity floatingEntity in _inLavaEntities)
        //{
        //    if(floatingEntity && EntityManager.EntityMatchProperty(floatingEntity, EntityProperties.LavaSensibility))
        //    {
        //        floatingEntity.TakeDamage(this, _stats._damage * Time.deltaTime);
        //    }
        //}
    }

}
