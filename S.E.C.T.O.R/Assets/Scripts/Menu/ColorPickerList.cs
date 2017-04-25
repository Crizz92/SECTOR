using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerList : MonoBehaviour {

    [SerializeField]
    private UIColorPicker[] _colorPickerList;
    [SerializeField]
    private ToggleGroup _toggleGroup;

    public Color CurrentSelectedColor()
    {
        for(int i = 0; i < _colorPickerList.Length; i++)
        {
            if(_colorPickerList[i])
            {
                if(_colorPickerList[i].IsSelected)
                {
                    return _colorPickerList[i].Color;
                }
            }
        }
        return Color.white;
    }
}
