using UnityEngine;
using System.Collections;

public class MessageTrigger : MonoBehaviour {

    [SerializeField]
    private PopupMessage _popupMessage;

    [SerializeField]
    private string[] _messageList;

    [SerializeField]
    private float _timeBetweenMessage;

    private Collider _collider;
    private bool _activated = false;

    void Awake()
    {
        if (!_collider) _collider = GetComponent<Collider>();
        if(_collider) _collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.GetComponent<Drone>() && !_activated)
        {
            _activated = true;
            StartCoroutine("PopMessage");
        }
    }

    IEnumerator PopMessage()
    {
        for(int i = 0; i < _messageList.Length; i++)
        {
            PopupMessage popupCreated = PopupManager.CreatePopupMessage(_popupMessage, Vector3.one * 300.0f, _messageList[i]);
            yield return new WaitForSeconds(_timeBetweenMessage);
            PopupManager.DestroyPopup(popupCreated);
        }
    }
}
