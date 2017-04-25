using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class MultipleButtonInteraction : MonoBehaviour {

    [SerializeField]
    List<InteractiveButton> _buttonsToCheck;
    private bool _conditionReached = false;
    [SerializeField]
    private UnityEvent _onConditionReached;
    [SerializeField]
    private UnityEvent _onConditionLost;

    void Update()
    {
        foreach(InteractiveButton button in _buttonsToCheck)
        {
            if(!button.Pressed)
            {
                ConditionStateChange(false);
                return;
            }
        }
        ConditionStateChange(true);
        
    }
    void ConditionStateChange(bool conditionReached)
    {
        if(_conditionReached != conditionReached)
        {
            if(conditionReached)
            {
                _onConditionReached.Invoke();
            }
            else
            {
                _onConditionLost.Invoke();
            }
        }
        _conditionReached = conditionReached;
    }
}
