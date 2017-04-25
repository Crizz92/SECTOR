using UnityEngine;
using System.Collections;

public class Shield : Skill {

    [SerializeField]
    private float _duration;
    [SerializeField]
    private int _health;

    [SerializeField]
    private GameObject _shield;
    
    public override void Activate()
    {
        base.Activate();

    }

    private void ActivateShield()
    {

    }
}
