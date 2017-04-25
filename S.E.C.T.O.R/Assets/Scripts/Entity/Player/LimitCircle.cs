using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitCircle : MonoBehaviour {

    private Material _material;
    [SerializeField]
    private Color _color = Color.red;

    public void Initialize()
    {
        _material = transform.GetComponent<Renderer>().material;
    }

    public void SetLimit(float distance)
    {
        transform.localScale = new Vector3(distance, distance, 1.0f);
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, 0.0f);
    }
    public void SetColor(Color color)
    {
        _color = color;
        _material.SetColor("Color", color);
    }

    public void LimitDistanceRatio(float ratio)
    {
        float alphaRatio = Mathf.Clamp01(ratio - 0.6f) * 0.5f;
        Color color = _color * alphaRatio;
        _material.SetColor("_Color", color);
    }

}
