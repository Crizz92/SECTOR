using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuTab : MonoBehaviour {

    public MainMenu _mainMenu;
    public string _name = "default";
    [SerializeField]
    private List<UIElement> _UIElement;
    [SerializeField]
    private GameObject _firstButtonSelected;
    public GameObject FirstButtonSelected
    {
        get { return _firstButtonSelected; }
    }
    public Transform _viewPosition;
    public MenuTab _previousMenuTab;

    public void Initialize()
    {
        if (!_firstButtonSelected)
        {
            foreach(UIElement uiElement in _UIElement)
            {
                UISelectable selectable = (UISelectable)uiElement;
                if(selectable)
                {
                    _firstButtonSelected = selectable.gameObject;
                }
            }
        }
    }

    public virtual void OnArriveTab()
    {

    }
    public virtual void OnLeaveTab()
    {

    }
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisableAllButton()
    {
        foreach (UIElement uiElement in _UIElement)
        {
            UISelectable selectable = uiElement as UISelectable;
            if (selectable)
            {
                if(selectable.SelectableComponent) selectable.SelectableComponent.interactable = false;
            }
        }
    }
    public void ActivateAllButton()
    {
        foreach (UIElement uiElement in _UIElement)
        {
            UISelectable selectable = uiElement as UISelectable;
            if (selectable)
            {
                if (selectable.SelectableComponent) selectable.SelectableComponent.interactable = true;
            }
        }
    }

    public Selectable FindButton(string buttonName)
    {
        foreach (UIElement uiElement in _UIElement)
        {
            UISelectable selectable = uiElement as UISelectable;
            if (selectable)
            {
                if (selectable.SelectableComponent) return selectable.SelectableComponent;
            }
        }
        return null;
    }
    
    public virtual void GoBack()
    {
        _mainMenu.GoToTab(_previousMenuTab._name);
    }
}
