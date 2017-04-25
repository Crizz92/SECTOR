using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

    public int _maxhealth;
    private float _currenthealth;
    public float HealthRatio
    {
        get { return (float)_currenthealth / (float)_maxhealth; }
    }
    public float Health
    {
        get { return (int)Mathf.Ceil(_currenthealth); }
    }
    public float _speed;
    public int _damage;
    public float _attackspeed;
    public float _maxEnergy;
    private float _currentEnergy = 10.0f;
    public float EnergyRatio
    {
        get { return _currentEnergy / _maxEnergy; }
    }
    public float Energy
    {
        get { return _currentEnergy; }
    }

    void Awake()
    {
        _currenthealth = _maxhealth;
        _currentEnergy = _maxEnergy;
    }


    public void TakeDamage(float damage)
    {
        _currenthealth -= damage;
        if (_currenthealth < 0)
        {
            _currenthealth = 0;
        }
    }

    public void RefundHealth(float healthAmount)
    {
        _currenthealth += healthAmount;
        if(_currenthealth > _maxhealth)
        {
            _currenthealth = _maxhealth;
        }
    }
    public void LoseEnergy(float energy)
    {
        _currentEnergy -= energy;
        if(_currentEnergy <= 0.0f)
        {
            _currentEnergy = 0.0f;
        }
    }
    public void RefundEnergy(float energy)
    {
        _currentEnergy += energy;
        if(_currentEnergy >= _maxEnergy)
        {
            _currentEnergy = _maxEnergy;
        }
    }
    public bool EnoughEnergy(float energy)
    {
        if (energy >= _currentEnergy) return true;
        else return false;
    }
}
