using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

    [SerializeField]
    private GameObject _debugInfo;

    [SerializeField]
    private Text _entity;
    [SerializeField]
    private Text _dynamicLight;
    [SerializeField]
    private Text _dynamicLightShadow;

    private bool _displayed = false;
    [SerializeField]
    public bool Displayed
    {
        get
        {
            return _displayed;
        }
        set
        {
            _displayed = value;
            DisplayDebugInfo(_displayed);
        }
    }
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.GlobalPlayerInput && GameManager.GlobalPlayerInput.ButtonBack()) Displayed = !_displayed;
		if(_displayed)
        {
            _entity.text = "Awake Entity: "+ EntityManager.ActiveEntityCount();
            _dynamicLight.text = "Dyn. Light: "+ EntityManager.ActiveDynamicLightCount();
            _dynamicLightShadow.text = "Dyn. Light Shadow: " + EntityManager.ActiveDynamicLightCastingShadowCount();
        }
	}

    public void DisplayDebugInfo(bool display)
    {
        _debugInfo.SetActive(display);
    }
}
