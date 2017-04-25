using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DronesHealthBar : UIElement {

    [SerializeField]
    private GameObject _healthbar;
    public GameObject HealthBar
    {
        get { return _healthbar; }
    }
    [SerializeField]
    private Image _health;
    private Stats _stats;
    private DronesManager _dronesManager;
    private bool _visible = false;

	
	// Update is called once per frame
	void Update ()
    {
        if(_visible && _health && _dronesManager)
        {
            _health.fillAmount = _dronesManager._stats.HealthRatio;
        }
	}
    public void Initialize(DronesManager dronesManager)
    {
        _dronesManager = dronesManager;
    }
    public void Show()
    {
        _healthbar.SetActive(true);
        _visible = true;
    }
    public void Hide()
    {
        _healthbar.SetActive(false);
        _visible = false;
    }
}
