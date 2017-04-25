using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectable : UIElement
{
    private Selectable _selectable;
    public Selectable SelectableComponent
    {
        get { return _selectable; }
    }



    public override void Initialize()
    {
        base.Initialize();
        _selectable = GetComponent<Selectable>();
    }

}
