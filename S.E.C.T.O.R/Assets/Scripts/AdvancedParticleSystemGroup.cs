using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvancedParticleSystemGroup : MonoBehaviour {

    [SerializeField]
    protected List<AdvancedParticleSystem> _particleSystemList = new List<AdvancedParticleSystem>();

    public void Initialize()
    {
        foreach(AdvancedParticleSystem particleSystem in _particleSystemList)
        {
            particleSystem.Initialize();
        }
    }

    public void EmissionRateBasedOnPercent(float percent)
    {
        foreach(AdvancedParticleSystem particleSystem in _particleSystemList)
        {
            particleSystem.EmissionRateBasedOnPercent(percent);
        }
    }
}
