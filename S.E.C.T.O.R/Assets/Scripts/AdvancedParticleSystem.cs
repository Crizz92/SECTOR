using UnityEngine;
using System.Collections;

public class AdvancedParticleSystem : MonoBehaviour {

    protected ParticleSystem _particleSystem;
    protected float _basicEmissionRate;
    protected float _currentEmissionRateRatio;

    public void Initialize()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _basicEmissionRate = ParticleSystemExtension.GetEmissionRate(_particleSystem);
        _currentEmissionRateRatio = 1.0f;

    }

    public void EmissionRateBasedOnPercent(float percent)
    {
        float ratio = percent / 100.0f;
        _currentEmissionRateRatio = ratio;
        ParticleSystemExtension.SetEmissionRate(_particleSystem, _basicEmissionRate * ratio);
    }
}
