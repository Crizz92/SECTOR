using UnityEngine;
using System.Collections;

public class Dash : Skill {

    public float _dashForce = 20.0f;
    private float _basicMaxVelocity;
    public float _dashDamage = 20.0f;
    private bool _dashing = false;
    public override void Activate()
    {

        base.Activate();
        if (_used)
        {
            Dashing();
            _used = false;
        }

    }

    protected void Dashing()
    {
        _dashing = true;
        _owner.DroneAnimator.SetTrigger("Dash");
        _basicMaxVelocity = _owner.PlayerMotor._maxVelocity;
        _owner.PlayerMotor._maxVelocity = 200.0f;
        _owner.EntityRigidbody.AddForce(_owner.transform.up * _dashForce, ForceMode.Impulse);
        StartCoroutine("EndDash");
    }

    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(0.1f);
        _owner.PlayerMotor._maxVelocity = _basicMaxVelocity;
        _dashing = false;
        _canBeUse = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if(_dashing)
        {
            Entity entity = col.collider.GetComponent<Entity>();
            
            if(entity != null && entity.EntityInformations._type != EntityType.Drone)
            { 
                
                entity.TakeDamage(_owner, _dashDamage);
            }
        }
    }
}
