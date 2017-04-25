using UnityEngine;
using System.Collections;

public class SpikeTrap : Trap {

    enum SpikeMod
    {
        Warning = 0,
        Out,
        In,
    }

    public bool _automatic = true;
    public float _occurTimeSec = 4.0f;
    public float _warningTimeSec = 1.0f;
    public float _remainingTimeSec = 2.0f;
    private float _nextAction = 0.0f;
    private SpikeMod _mod = SpikeMod.In;
    [SerializeField]
    private Animator _spikeAnimator;
    [SerializeField]
    private Spike _trapSpikes;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        if(!_spikeAnimator) _spikeAnimator = GetComponentInChildren<Animator>();
        if(!_trapSpikes) _trapSpikes = GetComponentInChildren<Spike>();
        if(_trapSpikes) _trapSpikes.Initialize(this);
    }
    public override void Update()
    {
        base.Update();

        if (!_sleeping)
        {
            _nextAction += Time.deltaTime;
            switch (_mod)
            {
                default:
                case SpikeMod.In:
                    {
                        if (_nextAction >= _warningTimeSec)
                        {
                            _mod = SpikeMod.Warning;
                            _spikeAnimator.SetBool("Incoming", true);
                            _nextAction = 0.0f;
                        }
                        break;
                    }
                case SpikeMod.Warning:
                    {
                        if (_nextAction >= _occurTimeSec)
                        {
                            _mod = SpikeMod.Out;
                            _spikeAnimator.SetBool("Out", true);
                            _spikeAnimator.SetBool("Incoming", false);
                            _nextAction = 0.0f;
                            _trapSpikes.CanDealDamage(true);
                        }
                        break;
                    }
                case SpikeMod.Out:
                    {
                        if (_nextAction >= _remainingTimeSec)
                        {
                            _mod = SpikeMod.In;
                            _spikeAnimator.SetBool("Out", false);
                            _nextAction = 0.0f;
                            _trapSpikes.CanDealDamage(false);
                        }

                        break;
                    }
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Disable()
    {
        base.Disable();
    }
}
