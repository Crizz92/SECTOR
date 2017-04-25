using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronesLink : MonoBehaviour {

    [SerializeField]
    private LineRenderer _visualLink;
    private Material _material;
    private Color _color;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    [SerializeField]
    private Color _nearColor;
    [SerializeField]
    private Color _farColor;

    public void Initialize()
    {
        _visualLink = GetComponent<LineRenderer>();
        if (_visualLink) _material = _visualLink.material;
    }

    public void SetLink(Vector3 startPos, Vector3 endPos)
    {
        _visualLink.SetPosition(0, startPos);
        _visualLink.SetPosition(1, endPos);
    }
    public void ChangeColor(Color color)
    {
        _material.SetColor("_Color", color);
        //_visualLink.startColor = color;
        //_visualLink.endColor = color;
    }
    public void ChangeWidth(float width)
    {
        if (width < 0.0f) width = 0;
        _visualLink.startWidth = width;
        _visualLink.endWidth = width;
    }

    public void LimitDistanceRatio(float ratio)
    {
        Color finalColor = Color.Lerp(_nearColor, _farColor, ratio) * 0.3f;
        finalColor.a = 1.0f;
        ChangeColor(finalColor);
    }
}
