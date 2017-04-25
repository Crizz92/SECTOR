using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class PopupMessage : PopupElement {

    [SerializeField]
    private Text _textComp;

    public virtual void Initialize(string textName, UnityAction eventToDo)
    {
        base.Initialize(eventToDo);
        _textComp.text = Settings._currentLevelTranslation[textName];
    }


}
