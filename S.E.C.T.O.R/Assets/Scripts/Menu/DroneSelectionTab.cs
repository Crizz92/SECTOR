using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DroneSelectionTab : MenuTab {
    [SerializeField]
    private List<Image> _renderTexturePreview;
    [SerializeField]
    private MaterialModifier _currentDronePreview;
    [SerializeField]
    private ColorPickerList _colors;
    [SerializeField]
    private DroneSelectionManager _selectionManager;

    public override void OnArriveTab()
    {
        base.OnArriveTab();
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _currentDronePreview.SetEmissiveColor(_colors.CurrentSelectedColor() * 3.0f);
        _currentDronePreview.MaterialModified.SetColor("_ColorDrone", _colors.CurrentSelectedColor());
    }

    public void SwitchDrone()
    {
        _selectionManager.InvertDrone();
        if(_selectionManager._invertedPlayer)
        {
            _renderTexturePreview[0].gameObject.SetActive(false);
            _renderTexturePreview[1].gameObject.SetActive(true);
            _currentDronePreview = _selectionManager._dronePreview[1];
        }
        else
        {
            _renderTexturePreview[1].gameObject.SetActive(false);
            _renderTexturePreview[0].gameObject.SetActive(true);
            _currentDronePreview = _selectionManager._dronePreview[0];
        }

    }

    public void ValidateSelection()
    {

    }

    public override void GoBack()
    {
        _selectionManager.GoBack();
    }

    public Color GetColor()
    {
        return _colors.CurrentSelectedColor();
    }
}
