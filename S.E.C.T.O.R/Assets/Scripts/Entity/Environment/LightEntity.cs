using UnityEngine;
using System.Collections;

public class LightEntity : PhysicalEntity, IElectricSensibility {

    [SerializeField]
    protected Light _light;
    protected float _lightIntensity;

    #region IElectricSensibility
    protected bool _isElectrocuted = false;
    public bool IsElectrocuted
    {
        get { return _isElectrocuted; }
    }
    #endregion

    public override void Initialize()
    {
        base.Initialize();
        if(!_light) _light = GetComponentInChildren<Light>();
        if (_light) _lightIntensity = _light.intensity;
    }

    public void Electrocute()
    {
        if(!_killed)
        {
            _isElectrocuted = true;
            StartCoroutine("LightOut");
        }
    }
    public void Reboot()
    {
        if(!_killed)
        {
            _isElectrocuted = false;
            StartCoroutine("LightIn");
        }
    }

    IEnumerator LightOut()
    {
        float lightRatio = 1;
        while (lightRatio > 0 && !_killed)
        {
            lightRatio -= Time.deltaTime * 2.0f;
            if (lightRatio < 0) lightRatio = 0;
            _light.intensity = _lightIntensity * lightRatio;
            yield return null;
        }
    }
    IEnumerator LightIn()
    {
        float lightRatio = 0;
        while (lightRatio < 1 && !_killed)
        {
            lightRatio += Time.deltaTime * 2.0f;
            if (lightRatio > 1) lightRatio += 1;
            _light.intensity = _lightIntensity * lightRatio;
            yield return null;
        }
    }
}
