using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DroneType
{
    P11 = 0,
    P12,
    Both,
};

public class Drone : PhysicalEntity {

    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected GameObject _droneRender;

    public Animator DroneAnimator
    {
        get { return _animator; }
    }
    [SerializeField]
    protected DroneType _droneType = DroneType.P11;
    public DroneType DroneType
    {
        get { return _droneType; }
    }
    [SerializeField]
    protected DronesManager _dronesManager;
    protected Light _spotlight;
    protected Skill _skill;
    [SerializeField]
    protected TrailInfo _trail;
    [SerializeField]
    protected EnergyBar _energyBarPrefab;
    protected EnergyBar _energyBar;
    [SerializeField]
    protected TorchLight _torchlight;
    [SerializeField]
    protected Light _globalLight;

    // Component
    #region Component
    [SerializeField]
    protected MaterialModifier _materialModifier;
    protected PlayerMotor _playerMotor;
    public PlayerMotor PlayerMotor
    {
        get { return _playerMotor; }
    }
    protected PlayerInput _playerInput;
    public PlayerInput PlayerInput
    {
        get { return _playerInput; }
    }
    protected PlayerController _playerController;
    public PlayerController PlayerController
    {
        get { return _playerController; }
    }
    protected InteractifElement _currentInteractiveFocused = null;
    #endregion

    #region Skills
    // Skill Attributes
    [SerializeField]
    protected Skill _skill1;
    [SerializeField]
    protected Skill _skill2;
    [SerializeField]
    protected Skill _skill3;
    [SerializeField]
    protected Skill _skill4;
    #endregion


    protected bool _moving = false;

    public Color _playerColor;
    public float _energyRegenSpeed = 5.0f;

    public virtual void Initialize(DronesManager dronesManager)
    {
        base.Initialize();
        _dronesManager = dronesManager;
        if (!_spotlight) _spotlight = GetComponentInChildren<Light>();
        if (!_playerInput) _playerInput = GetComponent<PlayerInput>();
        if (!_playerMotor) _playerMotor = GetComponent<PlayerMotor>();
        if (!_animator) _animator = GetComponent<Animator>();
        if (!_skill) _skill = GetComponent<Skill>();
        if (!_trail) _trail = GetComponentInChildren<TrailInfo>();
        if(!_energyBar)
        {
            Transform inGameUI = GameObject.Find("InGamePopup").transform;
            _energyBar = Instantiate(_energyBarPrefab);
            _energyBar.Initialize(transform);
            _energyBar.transform.SetParent(inGameUI);
        }
        _materialModifier.Initialize();
        PlayerInformations playerInfo = CoopManager.Instance.GetDroneInfo(_droneType);
        _materialModifier.SetEmissiveColor(playerInfo._color * 3.0f);
        _trail.SetColor(playerInfo._color);
        _playerController = GetComponent<PlayerController>();
        _torchlight.On = false;
    }

    public override void Start () {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();
	}

	public override void Update () {
        if(!GameManager.IsInPause)
        {
            _animator.SetBool("Moving", _playerMotor.Moving);
            CheckSkills();
            UpdateUIInfo();
            CheckInteraction();
            _stats.RefundEnergy(_energyRegenSpeed * Time.deltaTime);
            TorchLight();
            
        }
    }

    protected void UpdateUIInfo()
    {
        if(_energyBar) _energyBar.EnergyRatio = _stats.EnergyRatio;
    }

    public override void TakeDamage(Entity entity, float damage)
    {
        _dronesManager.TakeDamage(entity, damage);
    }
    public override void RefundHealth(float health)
    {
        _dronesManager.RefundHealth(health);
    }

    #region Skill Management
    public virtual void UnlockSkill(string skillName)
    {
        Skill skill = null;
        if(_skill1 && _skill1._name == skillName)
        {
            skill = _skill1;
        }
        if (_skill2 && _skill2._name == skillName)
        {
            skill = _skill2;
        }
        if (_skill3 && _skill3._name == skillName)
        {
            skill = _skill3;
        }
        if (_skill4 && _skill4._name == skillName)
        {
            skill = _skill4;
        }
        if(skill)
        {
            skill.Unlocked = true;
            
        }
    }
    protected virtual void CheckSkills()
    {
        if(_playerInput.ButtonX())
        {
            if(_skill1)
            {
                _skill1.Activate();
            }
        }
        if (_playerInput.ButtonY())
        {
            if(_skill2)
            {
                _skill2.Activate();
            }
        }
    }
    #endregion

    private void TorchLight()
    {
        if(_torchlight.enabled && _playerInput.RightJoystickDown())
        {
            Debug.Log("Displayed 2");
            _torchlight.On = !_torchlight.On;
        }
    }

    public void TrailEnabled(bool enabled)
    {
        _trail.TrailEnabled(enabled);
    }
    public void Disconnect()
    {
        DisableCompletly();
        _rigidbody.useGravity = true;
    }
    public void DisableCompletly()
    {
        _materialModifier.SetEmissivePowerRatio(0.0f);
        LampEnabled(false);
        _playerMotor.enabled = false;
        _playerController.enabled = false;
    }
    public void EnableCompletly(bool instant)
    {
        if(instant)
        {
            _materialModifier.SetEmissivePowerRatio(1.0f);
            LampEnabled(true);
            _playerMotor.enabled = true;
            _playerController.enabled = true;
        }
        else
        {
            StartCoroutine("DroneActivation");
        }
        _rigidbody.useGravity = false;
    }
    public void LampEnabled(bool enabled)
    {
        _spotlight.enabled = true;
    }

    IEnumerator DroneActivation()
    {

        float ratio = 0.0f;
        while(ratio <1.0f)
        {
            ratio += Time.deltaTime;
            _materialModifier.SetEmissivePowerRatio(ratio);
            yield return null;
        }
        _playerMotor.enabled = true;
        _playerController.enabled = true;
    }

    void CheckInteraction()
    {
        InteractifElement nearestInteractive = EntityManager.InteractableInRange(transform.position);
        if(_currentInteractiveFocused != nearestInteractive)
        {
            if (_currentInteractiveFocused) _currentInteractiveFocused.OutOfRange(this);
            _currentInteractiveFocused = nearestInteractive;
            if(_currentInteractiveFocused)
            {
                _currentInteractiveFocused.InRange(this);
            }
        }
        if(_playerInput.ButtonA() && _currentInteractiveFocused)
        {
            _currentInteractiveFocused.Interact(this);
        }
    }

    public void Display(bool display)
    {
        
        _droneRender.SetActive(display);
        _collider.enabled = display;
        TrailEnabled(display);
        _globalLight.enabled = display;
        _torchlight.On = display;
        Debug.Log("Played ");
        _torchlight.enabled = display;
    }


    public void DestroyEnergyBar()
    {
        Destroy(_energyBar.gameObject);
    }
}


