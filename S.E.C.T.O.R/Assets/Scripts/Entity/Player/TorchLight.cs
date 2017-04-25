using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour {

    [SerializeField]
    private Light _light;

    private bool _on = false;
    public bool On
    {
        get { return _on; }
        set
        {
            _on = value;
            _light.enabled = _on;
        }
    }

    void Awake()
    {
        On = false;
    }
}
