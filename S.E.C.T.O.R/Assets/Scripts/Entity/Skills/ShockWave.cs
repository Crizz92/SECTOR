using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShockWave : Skill {

    [SerializeField]
    private float _shockForce;
    [SerializeField]
    private float _shockRadius;

    [SerializeField]
    private ParticleSystem _shockwaveEffectPrefab;
    private ParticleSystem _shockwaveEffect;

    protected override void Awake()
    {
        base.Awake();
        _shockwaveEffect = Instantiate(_shockwaveEffectPrefab);
        _shockwaveEffect.transform.position = _owner.transform.position - (Vector3.forward * 1.0f);
    }
    public override void Activate()
    {
        base.Activate();
        if(_unlocked)
        {
            DoShockWave();
            _shockwaveEffect.transform.position = _owner.transform.position;
            _shockwaveEffect.Play();
        }
    }

    private void DoShockWave()
    {

        List<PhysicalEntity> _entitiesInRange = EntityManager.EntityInSphere<PhysicalEntity>(transform.position, _shockRadius);
        for(int i = 0; i < _entitiesInRange.Count; i++)
        {
            PhysicalEntity entity = _entitiesInRange[i];
            if(entity)
            {
                Vector3 toEntity = (entity.transform.position - _owner.transform.position).normalized;
                Rigidbody rigidbody = entity.GetComponent<Rigidbody>();
                if(rigidbody) rigidbody.AddForce(toEntity * _shockForce, ForceMode.Impulse);
                
            }
        }
    }

}
