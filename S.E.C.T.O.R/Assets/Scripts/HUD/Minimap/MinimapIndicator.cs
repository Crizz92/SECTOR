using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapIndicator : MonoBehaviour {

    [SerializeField]
    private bool _autoGesture = true;
    [SerializeField]
    private Sprite _indicator;
    public Sprite Indicator
    {
        get { return _indicator; }
    }
    private Image _minimapTarget;
    public Image MinimapTarget
    {
        get { return _minimapTarget; }
    }
    [SerializeField]
    private bool _movingTarget = false;
    [SerializeField]
    private float _size = 10.0f;
    public float Size
    {
        get { return _size; }
    }
    private bool _initialized = false;
    public bool Initialized
    {
        get { return _initialized; }
    }
    private Transform _target;
    public Transform Target
    {
        get { return _target; }
        set { _target = value; }
    }
    private bool _displayed = false;

    void Awake()
    {
        if(Minimap.Instance && Minimap.Instance.Displayed)
        {
            _displayed = true;
            if (_autoGesture && !_target) _target = transform;
            _minimapTarget = Minimap.Instance.AddIndicator(this, _target.position);
            _initialized = true;
        }
    }

	public void Initialize()
    {
        _displayed = true;
        if (_autoGesture && !_target) _target = transform;
        if (Minimap.Instance.Displayed && _minimapTarget == null)
        {
            if(_target) _minimapTarget = Minimap.Instance.AddIndicator(this, _target.position);
            else _minimapTarget = Minimap.Instance.AddIndicator(this);
            _initialized = true;
        }
        else if(Minimap.Instance.Displayed && _minimapTarget != null && _target)
        {
            if(_target)Minimap.Instance.UpdateIndicatorPosition(this, _target.position);
            else _minimapTarget = Minimap.Instance.AddIndicator(this);
        }

    }
	
	void Update ()
    {
        if(_movingTarget && Minimap.Instance.Displayed && _target)
        {
            Minimap.Instance.UpdateIndicatorPosition(this, _target.position);
        }
	}

    public void Display(bool display)
    {
        _displayed = display;
        if (display) _minimapTarget.gameObject.SetActive(true);
        else _minimapTarget.gameObject.SetActive(false);
    }
}
