using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InteractiveButton : MonoBehaviour {

    [SerializeField]
    private Transform _buttonElement;
    [SerializeField]
    private MaterialModifier _buttonMat;
    private Entity[] _interactionList = new Entity[5];

    // Event 
    [SerializeField]
    private UnityEvent _pressedEventList;
    [SerializeField]
    private UnityEvent _unpressedEventList;
    private Animator _buttonAnimator;

    // not required
    private MultipleButtonInteraction _multiButtonInteraction;
    protected bool _pressed = false;
    public bool Pressed
    {
        get { return _pressed; }
    }
	// Use this for initialization
    public void Initialize(MultipleButtonInteraction multiButtonManager)
    {
        _multiButtonInteraction = multiButtonManager;
        _buttonAnimator = _buttonElement.GetComponent<Animator>();
    }
	void Start () {
        _buttonAnimator = _buttonElement.GetComponent<Animator>();	
	}
	
	void Update ()
    {
	    
	}

    void OnTriggerEnter(Collider col)
    {
        Entity entity = col.GetComponent<Entity>();
        if(entity)
        {
            for (int i = 0; i < _interactionList.Length; i++)
            {
                if (_interactionList[i] == null)
                {
                    _interactionList[i] = entity;
                    break;
                }
            }
            _buttonAnimator.SetBool("Pressed", true);

            _pressedEventList.Invoke();
            _pressed = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        Entity entity = col.GetComponent<Entity>();
        if (entity)
        {
            for (int i = 0; i < _interactionList.Length; i++)
            {
                if (_interactionList[i] == entity)
                {
                    _interactionList[i] = null;
                    break;
                }
            }
            _buttonAnimator.SetBool("Pressed", false);
            _unpressedEventList.Invoke();
            _pressed = false;
        }
    }
}
