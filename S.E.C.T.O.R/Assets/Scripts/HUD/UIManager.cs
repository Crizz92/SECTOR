using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public enum EPopupType
{
    Popup = 0,
    Ingame,
    Overlay,
    ZoneName,
    Custom,
}

public class UIManager : Singleton<UIManager> {

    [SerializeField]
    private EventSystem _eventSystem;
    public EventSystem UIEventSystem
    {
        get { return _eventSystem; }
    }

    
    [SerializeField]
    private Transform _zoneInfoPosition;
    public Transform ZoneInfoPosition
    {
        get { return _zoneInfoPosition; }
    }
    [SerializeField]
    private Transform _dialoguePosition;
    public Transform DialoguePosition
    {
        get { return _dialoguePosition; }
    }
    [SerializeField]
    private Transform _globalPopupPosition;
    public Transform GlobalPopupPosition
    {
        get { return _globalPopupPosition; }
    }
    [SerializeField]
    private Transform _warningPosition;
    public Transform WarningPosition
    {
        get { return _warningPosition; }
    }

    public void Initialize()
    {
        if (!_eventSystem) _eventSystem = FindObjectOfType<EventSystem>();
        DontDestroyOnLoad(_eventSystem.gameObject);
        UIElement[] uiElementList = FindObjectsOfType<UIElement>();
        for(int i = 0; i < uiElementList.Length; i++)
        {
            if(uiElementList[i])
            {
                uiElementList[i].Initialize();
            }
        }
        InGameMenu.Instance.Initialize();
    }

    public void CreatePopup(PopupElement popup, Vector3 position)
    {
        PopupManager.CreatePopup(popup, position);
    }
    public void CreatePopupMessage(PopupMessage popup, Vector3 position, string textName)
    {
        PopupManager.CreatePopupMessage(popup, position, textName);
    }

    public Image CreateImage(Image image, EPopupType elementType)
    {
        Image createdImage = Instantiate(image);
        createdImage.transform.SetParent(_globalPopupPosition.transform);
        return createdImage;
    }

    private Vector3 PositionByType(EPopupType type)
    {
        switch (type)
        {
            case EPopupType.Popup:
                break;
            case EPopupType.Ingame:
                break;
            case EPopupType.Overlay:
                break;
            case EPopupType.ZoneName:
                break;
            case EPopupType.Custom:
                break;
            default:
                break;
        }
        return Vector3.zero;
    }


}
