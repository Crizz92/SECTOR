using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour {

    [SerializeField]
    private UnityEvent _onTriggerEnter = new UnityEvent();
    [SerializeField]
    private UnityEvent _onTriggerLeave = new UnityEvent();
    [SerializeField]
    private UnityEvent _onTriggerStay = new UnityEvent();

    [SerializeField]
    private bool _disableOnActivation = true;
    [SerializeField]
    private bool _activated = false;

    public bool Activated
    {
        set
        {
            _activated = value;
            _col.enabled = _activated;
        }
        get { return _activated; }
    }

    private Collider _col;


    void Start()
    {
        _col = GetComponent<Collider>();
        _col.isTrigger = true;
        _col.enabled = _activated;
    }

    void OnTriggerEnter(Collider col)
    {
        _onTriggerEnter.Invoke();
        Activated = false;
    }

    void OnTriggerStay(Collider col)
    {
        _onTriggerStay.Invoke();
    }

    void OnTriggerExit(Collider col)
    {
        _onTriggerLeave.Invoke();
    }

}
