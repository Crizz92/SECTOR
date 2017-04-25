using UnityEngine;
using System.Collections;

public class Door : Entity {

    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected bool _locked = false;
    [SerializeField]
    protected bool _opened = false;
    [SerializeField]
    protected Light _leftLight;
    [SerializeField]
    protected MaterialModifier _leftNeon;
    [SerializeField]
    protected Light _rightLight;
    [SerializeField]
    protected MaterialModifier _rightNeon;
    [SerializeField]
    protected Color _openColor;
    [SerializeField]
    protected Color _closeColor;
    [SerializeField]
    protected Color _lockColor;
    protected Color _currentColor;

    [SerializeField]
    protected bool _automaticDoor;
    private Drone[] _playerOnTrigger = new Drone[4];
    [SerializeField]
    protected Collider _collider;


    public override void Awake()
    {
        base.Awake();
        if (!_animator) _animator = GetComponentInChildren<Animator>();
        if(_locked)
        {
            if (_animator) _animator.SetBool("Open", false);
            _currentColor = _lockColor;
            _collider.enabled = false;
        }
        else
        {
            if (_animator) _animator.SetBool("Open", _opened);
            _currentColor = (_opened) ? _openColor : _closeColor;
        }
        _leftLight.color = _currentColor;
        _rightLight.color = _currentColor;
        _leftNeon.SetAmbientColor(_currentColor);
        _rightNeon.SetAmbientColor(_currentColor);
    }

    public void Lock(bool locked)
    {
        _locked = locked;
        if(_locked)
        {
            Close();
            if(_collider)
            {
                _collider.enabled = false;
                Helpers.ClearArray(_playerOnTrigger);
            }
            StartCoroutine("SwitchColor", _lockColor);
        }
        else
        {
            if(_collider)
            {
                _collider.enabled = true;
            }
            if(_opened)
            {
                Open();
            }
            else
            {
                StopCoroutine("SwitchColor");
                StartCoroutine("SwitchColor", _closeColor);
            }
        }
    }
    public void Open()
    {
        if(!_locked)
        {
            if (_animator)
            {
                if(!_opened)
                {
                    _animator.SetBool("Open", true);
                    StopCoroutine("SwitchColor");
                    StartCoroutine("SwitchColor", _openColor);
                }
            }
            else
            {
                Debug.LogWarning("Missing Animator Component on the door");
            }
        }
        _opened = true;
    }
    public void Close()
    {
        if (_animator)
        {
            if(_opened)
            {
                _animator.SetBool("Open", false);
                StopCoroutine("SwitchColor");
                StartCoroutine("SwitchColor", _closeColor);
            }
        }
        else
        {
            Debug.LogWarning("Missing Animator Component on the door");
        }
        _opened = false;
    }

    public override void Sleep()
    {
        base.Sleep();
        _leftLight.enabled = false;
        _rightLight.enabled = false;
        if(_collider)_collider.enabled = false;
    }

    public override void WakeUp()
    {
        base.WakeUp();
        _leftLight.enabled = true;
        _rightLight.enabled = true;
        if(_collider)_collider.enabled = true;

    }

    void OnTriggerEnter(Collider col)
    {
        if (!_locked)
        {
            if (_automaticDoor)
            {
                Drone drone = col.GetComponent<Drone>();
                if (drone)
                {
                    if (Helpers.ArrayIsEmpty(_playerOnTrigger)) Open();
                    Helpers.AddToArray(_playerOnTrigger, drone);
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (!_locked)
        {
            if (_automaticDoor)
            {
                Drone drone = col.GetComponent<Drone>();
                if (drone)
                {
                    for (int i = 0; i < _playerOnTrigger.Length; i++)
                    {
                        if (_playerOnTrigger[i] == drone)
                        {
                            _playerOnTrigger[i] = null;
                        }
                    }
                    if (Helpers.ArrayIsEmpty(_playerOnTrigger)) Close();
                }
            }
        }
    }

    IEnumerator SwitchColor(Color color)
    {
        float ratio = 0.0f;
        while (ratio < 1.0f)
        {
            ratio += Time.deltaTime;
            if (ratio > 1.0f) ratio = 1.0f;
            _currentColor = Color.Lerp(_currentColor, color, ratio);
            _leftLight.color = _currentColor;
            _rightLight.color = _currentColor;
            _leftNeon.SetAmbientColor(_currentColor);
            _rightNeon.SetAmbientColor(_currentColor);
            yield return null;
        }
        _currentColor = color;
    }

}
