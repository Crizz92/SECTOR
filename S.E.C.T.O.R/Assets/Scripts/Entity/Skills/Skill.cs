using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
    public string _name = "Default";
    public float _cooldown = 1.0f;
    private float _timeLeft = 0.0f;
    protected bool _canBeUse = true;
    protected bool _used = false;
    public int _energyCost = 20;
    [SerializeField]
    protected Drone _owner;
    [SerializeField]
    protected bool _unlocked = false;
    public bool Unlocked
    {
        get { return _unlocked; }
        set { _unlocked = value; }
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {

    }


    public virtual void Activate()
    {
        if(_unlocked)
        {
            if (_owner._stats.EnoughEnergy(_energyCost)) return;
            if (_canBeUse)
            {
                _owner._stats.LoseEnergy(_energyCost);
                _canBeUse = false;
                _used = true;
            }
        }
        else
        {
            return;
        }
    }

    public virtual void Stop()
    {

    }

}
