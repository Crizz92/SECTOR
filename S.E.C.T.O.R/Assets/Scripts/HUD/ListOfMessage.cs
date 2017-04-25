using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

 [Serializable]
public struct MessageInfo
{
    public PopupMessage _popupPrefab;
    public string _messageName;
    public float _durationTime;
    public UnityEvent _eventToDo;
}

public class ListOfMessage : MonoBehaviour {

    [SerializeField]
    private MessageInfo[] _messageInfoList;
    private PopupMessage _currentPopup;
    private int _currentMessageIndex = 0;
    private bool _activated = false;
    public void Activate()
    {
        _activated = true;
        NextPopup();
    }

    void NextPopup()
    {
        if(_currentPopup)
        {
            PopupManager.DestroyPopup(_currentPopup);
        }
        if (_currentMessageIndex < _messageInfoList.Length)
        {
            MessageInfo currentMessage = _messageInfoList[_currentMessageIndex];
            _currentPopup = PopupManager.CreatePopupMessage(currentMessage._popupPrefab, currentMessage._messageName);
            currentMessage._eventToDo.Invoke();
            Invoke("NextPopup", currentMessage._durationTime);
            _currentMessageIndex++;
        }
        else
        {
            _activated = false;
        }
    }
}
