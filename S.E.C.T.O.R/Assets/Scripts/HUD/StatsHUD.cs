using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsHUD : MonoBehaviour {

    private Image _healthbar;
    private Stats _stats;

	// Use this for initialization
	void Start () {
        _healthbar = GetComponent<Image>();
        _stats = FindObjectOfType<DronesManager>()._stats;

        if(_healthbar && _stats) _healthbar.fillAmount = _stats.HealthRatio;
	}
	
	// Update is called once per frame
	void Update () {
        if(_healthbar && _stats) _healthbar.fillAmount = _stats.HealthRatio;
    }
}
