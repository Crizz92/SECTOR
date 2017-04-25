using UnityEngine;
using System.Collections;

public class TrailInfo : MonoBehaviour {

    [SerializeField]
    protected TrailRenderer _trail;
    protected Gradient _basicGradient;

    protected float _time;
    protected float _startWidth;
    protected float _endWidth;

    void Awake()
    {
        if(!_trail) _trail = GetComponent<TrailRenderer>();
        if(_trail)
        {
            _basicGradient = _trail.colorGradient;
            _time = _trail.time;
            _startWidth = _trail.startWidth;
            _endWidth = _trail.endWidth;
        }
    }
    public void TrailEnabled(bool enabled)
    {
        if(!enabled)
        {
            _trail.Clear();
            _trail.enabled = false;
            StartCoroutine("TrailReset");
        }
        else
        {
            _trail.enabled = true;
            _trail.time = _time;
            _trail.startWidth = _startWidth;
            _trail.endWidth = _endWidth;
        }
    }
    
    IEnumerator TrailReset()
    {
        _trail.time = 0.0f;
        yield return new WaitForSeconds(.1f);
        _trail.time = _time;
    }

    public void SetColor(Color color)
    {
        GradientColorKey[] basicColorKeys = _basicGradient.colorKeys;
        GradientColorKey[] recolorKeys = basicColorKeys;
        Gradient newGradient = new Gradient();
        for(int i = 0; i < basicColorKeys.Length; i++)
        {
            recolorKeys[i].color = basicColorKeys[i].color * color;
        }
        newGradient.SetKeys(recolorKeys, _basicGradient.alphaKeys);
        _trail.colorGradient = newGradient;
    }
}
