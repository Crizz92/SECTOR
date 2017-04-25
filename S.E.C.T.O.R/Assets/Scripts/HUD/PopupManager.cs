using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public enum PopupType
{
    ArmyMessage = 0,
    P11Message,
    P12Message,
}

public static class PopupManager
{

    public static List<PopupElement> _popupList = new List<PopupElement>();
    public static Transform _globalPosition;
    public static Vector3 _center;
    public static UIManager _uiManager;
    public static Transform _popupContainer;

    public static void Initialize()
    {
        _uiManager = GameObject.FindObjectOfType<UIManager>();
        _popupContainer = GameObject.Find("InGamePopup").transform;
        _center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);
    }

    public static PopupElement CreatePopup(PopupElement popup, Vector3 position)
    {
        PopupElement currentPopup = GameObject.Instantiate(popup) as PopupElement;
        currentPopup.transform.SetParent(_popupContainer);
        currentPopup.transform.position = position;
        currentPopup.Initialize(null);
        _popupList.Add(currentPopup);
        return currentPopup;
    }
    public static PopupMessage CreatePopupMessage(PopupMessage popup, string textName)
    {
        return CreatePopupMessage(popup, UIManager.Instance.GlobalPopupPosition.position, textName);
    }
    public static PopupMessage CreatePopupMessage(PopupMessage popup, Vector3 position, string textName)
    {
        PopupMessage currentPopup = GameObject.Instantiate(popup) as PopupMessage;
        currentPopup.transform.SetParent(_popupContainer);
        currentPopup.transform.position = position;
        currentPopup.Initialize(textName, null);
        _popupList.Add(currentPopup);
        return currentPopup;
    }
    public static PopupMessage CreatePopupMessage(PopupMessage popup, Vector3 position, string textName, UnityAction eventToDo)
    {
        PopupMessage currentPopup = GameObject.Instantiate(popup) as PopupMessage;
        currentPopup.transform.SetParent(_popupContainer);
        currentPopup.transform.position = position;
        currentPopup.Initialize(textName, eventToDo);
        _popupList.Add(currentPopup);
        
        return currentPopup;
    }
    public static void DestroyPopup(PopupElement popup)
    {
        _popupList.Remove(popup);
        GameObject.Destroy(popup.gameObject);
    }

    public static void DestroyAllPopup()
    {
        foreach(PopupElement element in _popupList)
        {
            if(element)
            {
                GameObject.Destroy(element.gameObject);
            }
        }
    }
}
