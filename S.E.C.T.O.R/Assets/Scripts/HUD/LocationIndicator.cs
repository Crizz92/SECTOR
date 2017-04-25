using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocationIndicator : Singleton<LocationIndicator> {

    private MinimapIndicator _minimapIndicator;

    private Transform _target;
    public Transform Target
    {
        get { return _target; }
        set
        {
            _target = value;
            if (_target)
            {
                _minimapIndicator.Target = _target;
                _minimapIndicator.Display(true);
                _indicatorImage.gameObject.SetActive(true);
            }
            else
            {
                _minimapIndicator.Target = null;
                _minimapIndicator.Display(false);
                _indicatorImage.gameObject.SetActive(false);
            }
        }
    }
    private Camera _camera;
    [SerializeField]
    private Image _indicatorImage;
    [SerializeField]
    private Sprite _innerSprite;
    [SerializeField]
    private Sprite _outerSprite;
    private Vector3 _screenCenter = Vector3.zero;
    [SerializeField]
    private float _innerDecal = 20.0f;

	// Use this for initialization
	public void Initialize () {
        _camera = FindObjectOfType<InGameCamera>().GetComponent<Camera>();
        _minimapIndicator = GetComponent<MinimapIndicator>();
        _screenCenter.x = Screen.width * 0.5f;
        _screenCenter.y = Screen.height * 0.5f;
    }

	void Update () {
        if(_target)
        {
            Vector3 screenTargetPosition = _camera.WorldToScreenPoint(_target.position);
            screenTargetPosition = PositionCorrection(screenTargetPosition);
            _indicatorImage.transform.position = screenTargetPosition;
            Vector3 toTarget = screenTargetPosition - _screenCenter;
            float angle = Helpers.CalculateAngleOnZ(Vector3.up, toTarget);
            _indicatorImage.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
	}

    Vector3 PositionCorrection(Vector3 screenPosition)
    {
        if(screenPosition.x < _innerDecal || screenPosition.x > Screen.width - _innerDecal
            || screenPosition.y < _innerDecal || screenPosition.y > Screen.height - _innerDecal)
        {
            _indicatorImage.sprite = _outerSprite;
        }
        else
        {
            _indicatorImage.sprite = _innerSprite;
        }
        screenPosition.x = Mathf.Clamp(screenPosition.x, _innerDecal, Screen.width - _innerDecal);
        screenPosition.y = Mathf.Clamp(screenPosition.y, _innerDecal, Screen.height - _innerDecal);
        return screenPosition;
    }
    public void Display(bool display)
    {
        _indicatorImage.enabled = display;
    }
}
