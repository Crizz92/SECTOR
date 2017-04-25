using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameHUD : Singleton<IngameHUD> {

    [SerializeField]
    private List<UIElement> _HUDElementList = new List<UIElement>();


    protected override void Awake()
    {
        base.Awake();
        
    }
    public void Disable(string name)
    {
        foreach(UIElement element in _HUDElementList)
        {
            if(element.ElementName == name)
            {
                element.gameObject.SetActive(false);
            }
        }
    }
    public void Enable(string name)
    {
        foreach (UIElement element in _HUDElementList)
        {
            if (element.ElementName == name)
            {
                element.gameObject.SetActive(true);
            }
        }
    }
    public void Activate()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    public void Deactivate()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
