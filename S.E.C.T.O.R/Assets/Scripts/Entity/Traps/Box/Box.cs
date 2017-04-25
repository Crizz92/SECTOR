using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Box : PhysicalEntity, IElectricSensibility
{
    [SerializeField]
    private float _timeBeforeRecover = 3.0f;
    private float _recoverCooldown = 0.0f;
    private int _healthRecoveredSec = 50;
    protected Material _material;
    public Texture _desintegrateTex;
    public Texture _lookupDesintegrateTex;
    [SerializeField]
    private List<LightController> _lightControllers = new List<LightController>();

    #region IElectricSensibility
    protected bool isElectrocuted = false;
    public bool IsElectrocuted
    {
        get { return isElectrocuted; }
    }
    #endregion

    public override void Start()
    {
        base.Start();

        _stats = GetComponent<Stats>();
        _material = GetComponent<Renderer>().material;
    }
    public override void Sleep()
    {
        base.Sleep();
        _collider.enabled = false;
        _rigidbody.Sleep();
        foreach (LightController currentLight in _lightControllers)
        {
            if(currentLight) currentLight.Disable();
        }
    }

    public override void WakeUp()
    {
        base.WakeUp();
        _collider.enabled = true;
        EntityRigidbody.drag = 0.05f;
        _rigidbody.WakeUp();
        foreach (LightController currentLight in _lightControllers)
        {
            if(currentLight) currentLight.Enable();
        }
    }
    public override void Update()
    {
        base.Update();
        if (!_killed)
        {

            _recoverCooldown -= Time.deltaTime;
            _material.SetColor("_Color", Color.Lerp(Color.red, Color.white, _stats.HealthRatio));

            if (_recoverCooldown <= 0)
            {
                _recoverCooldown = 0;
                _stats.RefundHealth(_healthRecoveredSec * Time.deltaTime);
            }
        } 
    }
    IEnumerator Desintegration()
    {
        float desintegrateRatio = 0;
        while(desintegrateRatio<1)
        {
            desintegrateRatio += Time.deltaTime * 0.5f;
            _material.SetFloat("_AlphaInt", desintegrateRatio);
            yield return null;
        }
        _collider.enabled = false;
        _rigidbody.Sleep();
        EntityManager.DestroyEntity(this);
    }

    public override void TakeDamage(Entity entity, float damage)
    {
        base.TakeDamage(entity, damage);
        _recoverCooldown = _timeBeforeRecover;

        if (_stats.HealthRatio <= 0)
        {
            _killed = true;
            _material.shader = Shader.Find("S.E.C.T.O.R/PBR_AlphaTest_Objects_Desintegrate");
            _material.SetTexture("_AlphaTex", _desintegrateTex);
            _material.SetTexture("_LUT", _lookupDesintegrateTex);
            _material.SetColor("_EmberColor", Color.blue);
            StartCoroutine("Desintegration");
        }
    }
    public void Electrocute()
    {
        if(!_killed)
        {
            isElectrocuted = true;
            StartCoroutine("LightOut");
        }
    }
    public void Reboot()
    {
        if(!_killed)
        {
            isElectrocuted = false;
            StartCoroutine("LightIn");
        }
    }

    IEnumerator LightOut()
    {
        float lightRatio = 1;
        Color currentColor = _material.GetColor("_EmissionColor");
        while(lightRatio > 0 && !_killed)
        {
            lightRatio -= Time.deltaTime;
            if (lightRatio < 0) lightRatio = 0;
            _material.SetColor("_EmissionColor", currentColor * lightRatio);
            yield return null;
        }
    }
    IEnumerator LightIn()
    {
        float lightRatio = 0;
        Color currentColor = _material.GetColor("_EmissionColor");
        while (lightRatio < 1 && !_killed)
        {
            lightRatio += Time.deltaTime;
            if (lightRatio > 1) lightRatio += 1;
            _material.SetColor("_EmissionColor", Color.white * 4.0f * lightRatio);
            yield return null;
        }
    }
}
