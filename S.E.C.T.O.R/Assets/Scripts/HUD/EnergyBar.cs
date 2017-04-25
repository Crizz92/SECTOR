using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyBar : MonoBehaviour
{

    private Camera _camera;
    private Transform _target;
    [SerializeField]
    private Image _energyBar;
    [SerializeField]
    private Image _background;

    private float _energyRatio;
    public float EnergyRatio
    {
        get { return _energyRatio; }
        set
        {
            _energyRatio = value;
            if (_energyRatio >= 1.0f)
            {
                _energyBar.enabled = false;
                _background.enabled = false;
            }
            else
            {
                _energyBar.enabled = true;
                _background.enabled = true;
            }
        }
    }
    public Vector3 _decal = new Vector3(0, 2, 0);


	public void Initialize (Transform target) {
        _camera = FindObjectOfType<InGameCamera>().GetComponent<Camera>();
        if(!_background) _background = GetComponent<Image>();
        if (!_energyBar) _energyBar = GetComponent<Image>();
        _target = target;

    }
	
	// Update is called once per frame
	void Update () {
        transform.position = _camera.WorldToScreenPoint(_target.position + _decal);
        _energyBar.fillAmount = _energyRatio;
	}

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
