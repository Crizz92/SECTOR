using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class InteractifElement : MonoBehaviour, IInteractable, ISpecificColor
{
    [SerializeField]
    private Sprite _button;
    [SerializeField]
    protected Image _imagePrefab;
    protected Image _buttonIndicator;
    [SerializeField]
    protected DroneType _interactWith = DroneType.Both;

    [SerializeField]
    protected UnityEvent _onInteractEvent;

    [SerializeField]
    protected float _interactionRadius = 5.0f;
    public float InteractionRadius
    {
        get { return _interactionRadius; }
    }

    [SerializeField]
    protected bool _interactable = true;
    public bool Interactable
    {
        get { return _interactable; }
        set
        {
            _interactable = value;
        }
    }
    protected int _interactorInRange = 0;
    protected Camera _camera;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactionRadius);
    }

    public virtual void Initialize()
    {
        _buttonIndicator = UIManager.Instance.CreateImage(_imagePrefab, EPopupType.Ingame);
        _buttonIndicator.sprite = _button;
        _buttonIndicator.gameObject.SetActive(false);
        _camera = FindObjectOfType<InGameCamera>().GetComponent<Camera>();
        if(_interactWith != DroneType.Both)
        {
            CoopManager.Instance.GetDroneInfo(_interactWith);
        }
        AdaptColor();
    }

    protected virtual void Update()
    {
        if (_buttonIndicator && _buttonIndicator.IsActive()) _buttonIndicator.transform.position = _camera.WorldToScreenPoint(transform.position);
    }
    #region IInteractable
    public virtual void Interact(Drone drone)
    {
        if (drone.DroneType == _interactWith || _interactWith == DroneType.Both)
        {
            _onInteractEvent.Invoke();
            _interactable = false;
        }
    }
    public virtual void InRange(Drone drone)
    {
        if (drone.DroneType == _interactWith || _interactWith == DroneType.Both)
        {
            if (_interactorInRange < 1) _buttonIndicator.gameObject.SetActive(true);
            _interactorInRange++;
        }
    }
    public virtual void OutOfRange(Drone drone)
    {
        if (drone.DroneType == _interactWith || _interactWith == DroneType.Both)
        {
            _interactorInRange--;
            if (_interactorInRange < 1) _buttonIndicator.gameObject.SetActive(false);
        }
    }
    #endregion
    #region ISpecificColor
    public virtual void AdaptColor()
    {

    }
    #endregion
    public void DestroyButtonIndicator()
    {
        if(_buttonIndicator)
        {
            Destroy(_buttonIndicator.gameObject);
        }
    }

}
