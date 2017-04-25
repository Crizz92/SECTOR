using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorPicker : UISelectable {

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }
    [SerializeField]
    private Toggle _toggle;


    [SerializeField]
    private Color _color;
    public Color Color
    {
        get { return _color; }
    }
    public bool IsSelected
    {
        get { return _toggle.isOn; }
    }
}
