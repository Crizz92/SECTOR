using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    private Light _light;
    private float _initIntensity;
    private float _currentIntensity;
    private Color _initColor;
    private Color _currentColor;
    private bool _castingShadows = false;
    public bool CastingShadows
    {
        get { return _castingShadows; }
    }
    private bool _enabled = true;
    public bool Enabled
    {
        get { return _enabled; }
    }
    [SerializeField]
    private bool _lightManagerDependant = true;
    public bool LightManagerDependant
    {
        get { return _lightManagerDependant; }
    }
    [SerializeField]
    private float _disableDistance = 20.0f;
    public float DisableDistance
    {
        get { return _disableDistance; }
    }

    //void OnDrawGizmos()
    //{
    //    if(!_isBaked)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawCube(transform.position, Vector3.one * 2.0f);
    //    }
    //}
    public void Initialize()
    {
        _light = GetComponent<Light>();
        _initIntensity = _light.intensity;
        _currentIntensity = _initIntensity;
        _initColor = _light.color;
        _currentColor = _initColor;
        if (_light.shadows == LightShadows.None) _castingShadows = false;
        else _castingShadows = true;
        
    }

    public void Enable()
    {
        if (_light)
        {
            _light.enabled = true;
            _enabled = true;
        }
    }

    public void Disable()
    {
        if(_light)
        {
            _light.enabled = false;
            _enabled = false;
        }
    }

    public void PercentIntensityModifier(float percent)
    {
        _currentIntensity = _initIntensity * percent * 0.01f;
        _light.intensity = _currentIntensity;
    }
    public void ColorSwitch(Color color)
    {
        _initColor = color;
        _currentColor = _initColor;
        _light.color = _currentColor;
    }
    public void ColorSwitch(Color color, float time)
    {
        
    }

    //IEnumerator ColorSwitching(Color color)
    //{
    //    float ratio = 0;
    //    while (ratio < 1)
    //    {
    //        ratio = Time.deltaTime * 
    //    }
    //}
}
