using UnityEngine;
using System.Collections;

public class MaterialModifier : MonoBehaviour {

    [SerializeField]
    private Material _material;
    public Material MaterialModified
    {
        get { return _material; }
    }

    private Color _emissiveColor;
    private Color _ambientColor;
    private float _emissiveRatio;


    public void Initialize()
    {
        if (!_material) _material = GetComponent<Renderer>().material;
        _emissiveColor = _material.GetColor("_EmissionColor");
        _emissiveRatio = 1.0f;
        _ambientColor = _material.GetColor("_Color");
    }

    public void SetEmissiveColor(Color color)
    {
        
        _material.SetColor("_EmissionColor", color);
    }
    public void SetEmissivePowerRatio(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        _material.SetColor("_EmissionColor", _emissiveColor * ratio);
    }
    public void SetAmbientColor(Color color)
    {
        _material.SetColor("_Color", color);
    }
    public void SetOutlineColor(Color color)
    {
        _material.SetColor("_OutlineColor", color);
    }
}
