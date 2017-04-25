using UnityEngine;
using System.Collections;

public class Laser : Trap, IElectricSensibility {

    [SerializeField]
    private Transform _startPosition;
    private LineRenderer _laserRenderer;
    private Material _laserRendererMat;
    private float _startLaserAlphaTint;
    private Entity _target = null;
    public bool _static = true;
    [SerializeField]
    private int _maxRange = 30;
    private ConstantRotation _constantRotation;

    private float _nextDamageSec = 0.0f;
    [SerializeField]
    private AdvancedParticleSystem _mark;
    [SerializeField]
    private AdvancedParticleSystemGroup _laserFXGroup;

    #region IElectricSensibility
    [SerializeField]
    protected bool isElectrocuted = false;
    public bool IsElectrocuted
    {
        get { return isElectrocuted; }
    }
    #endregion

    public override void Awake()
    {
        base.Awake();
    }
    public override void Initialize()
    {
        base.Initialize();
        _laserRenderer = GetComponent<LineRenderer>();
        _laserRendererMat = _laserRenderer.material;
        _startLaserAlphaTint = _laserRendererMat.GetColor("_TintColor").a;
        _laserRenderer.SetPosition(0, _startPosition.position);
        _constantRotation = GetComponentInChildren<ConstantRotation>();
        _laserFXGroup.Initialize();
        if (isElectrocuted)
        {
            if(_constantRotation) _constantRotation.TurnByPercentSpeed(0.0f);
            Color color = _laserRendererMat.GetColor("_TintColor");
            color.a = 0.0f;
            _laserRendererMat.SetColor("_TintColor", color);
            _laserFXGroup.EmissionRateBasedOnPercent(0.0f);
        }
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        if (!_sleeping && !isElectrocuted)
        {
            bool dealDamage = false;
            _nextDamageSec += Time.deltaTime;
            if (_nextDamageSec >= _stats._attackspeed)
            {
                dealDamage = true;
            }
            LayerMask layer = 1 << LayerMask.NameToLayer("Environment") | 1 << LayerMask.NameToLayer("Player");
            _laserRenderer.SetPosition(0, _startPosition.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, _startPosition.forward, out hit, _maxRange, layer))
            {
                Vector3 toHit = (hit.point - _startPosition.position).normalized;
                _laserRenderer.SetPosition(1, hit.point);
                _mark.transform.position = hit.point - (toHit *0.1f);
                _mark.transform.rotation = Quaternion.LookRotation(hit.normal);
                Entity entity = hit.collider.GetComponent<Drone>();
                if (entity && dealDamage && !isElectrocuted)
                {
                    _mark.EmissionRateBasedOnPercent(0.0f);
                    entity.TakeDamage(this, _stats._damage);
                }
                else _mark.EmissionRateBasedOnPercent(100.0f);
                entity = hit.collider.GetComponent<Box>();
                if (entity && dealDamage && !isElectrocuted) entity.TakeDamage(this, _stats._damage);
            }
            else
            {
                _laserRenderer.SetPosition(1, _startPosition.position + (_startPosition.forward * _maxRange));
            }
        }
	}

    public override void Sleep()
    {
        base.Sleep();
        if (_constantRotation) _constantRotation.Sleep();
    }

    public override void WakeUp()
    {
        base.WakeUp();
        if (_constantRotation) _constantRotation.WakeUp();
    }

    #region IElectricSensibility
    public void Electrocute()
    {
        if (!_killed)
        {
            isElectrocuted = true;
            
            StartCoroutine("LightOut");
        }
    }
    public void Reboot()
    {
        if (!_killed)
        {
            isElectrocuted = false;
            StartCoroutine("LightIn");
        }
    }

    IEnumerator LightOut()
    {
        float lightRatio = 1;
        Color color = _laserRendererMat.GetColor("_TintColor");

        while (lightRatio > 0 && !_killed)
        {
            lightRatio -= Time.deltaTime;
            if (lightRatio < 0) lightRatio = 0;
            _constantRotation.TurnByPercentSpeed(lightRatio * 100.0f);
            color.a = _startLaserAlphaTint * lightRatio;
            _laserRendererMat.SetColor("_TintColor", color);
            _laserFXGroup.EmissionRateBasedOnPercent(lightRatio * 100.0f);

            yield return null;
        }
    }
    IEnumerator LightIn()
    {
        float lightRatio = 0;
        Color color = _laserRendererMat.GetColor("_TintColor");


        while (lightRatio < 1 && !_killed)
        {
            lightRatio += Time.deltaTime;
            if (lightRatio > 1) lightRatio += 1;
            _constantRotation.TurnByPercentSpeed(lightRatio * 100.0f);
            color.a = _startLaserAlphaTint * lightRatio;
            _laserRendererMat.SetColor("_TintColor", color);
            _laserFXGroup.EmissionRateBasedOnPercent(lightRatio * 100.0f);
            yield return null;
        }
    }
    #endregion
}
