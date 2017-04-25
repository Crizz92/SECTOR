using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class PopupElement : MonoBehaviour {

    [SerializeField]
    private EPopupType _type = EPopupType.Custom;
    public Image _imageComp;
    public bool _disappearOnInput = true;
    public float _timeRemaining = 5.0f;
    private float _currentTimeRemaining = 0.0f;
    private bool _destroyed = false;
    [SerializeField]
    private UnityEvent _eventAfterMessage;

    public virtual void Initialize(UnityAction eventToDo)
    {
        _currentTimeRemaining = _timeRemaining;
        _eventAfterMessage.AddListener(eventToDo);
    }

    protected virtual void Update()
    {
        if(!_destroyed)
        {
            if(!_disappearOnInput)
            {
                _currentTimeRemaining -= Time.deltaTime;
                if (_currentTimeRemaining <= 0.0f)
                {
                    _currentTimeRemaining = 0.0f;
                    PopupManager.DestroyPopup(this);
                    _destroyed = true;
                    _eventAfterMessage.Invoke();
                }
            }
            else
            {
               // if (GameManager.GlobalPlayerInput.ButtonX())
               // {
               //     PopupManager.DestroyPopup(this);
               //     _destroyed = true;
               //     _eventAfterMessage.Invoke();
               // }
            }
        }
    }

}
