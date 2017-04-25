using UnityEngine;
using System.Collections;

public class ConstantRotation : MonoBehaviour {

    [SerializeField]
    private Vector3 _rotationSpeed = Vector3.zero;
    private Vector3 _currentRotation = Vector3.zero;
    public Vector3 RotationSpeed
    {
        set
        {
            _rotationSpeed = value;
            _currentRotationSpeed = _rotationSpeed;
        }
        get { return _rotationSpeed; }
    }
    private Vector3 _currentRotationSpeed;

    private bool _sleeping = false;
    
    void Awake()
    {
        _currentRotationSpeed = _rotationSpeed;
    }
    // Update is called once per frame
	void Update () {
        if(!_sleeping)
        {
            
            _currentRotation = Time.deltaTime * _currentRotationSpeed;
            transform.Rotate(_currentRotation);
           
        }
	}

    public void Sleep()
    {
        _sleeping = true;
    }

    public void WakeUp()
    {
        _sleeping = false;
    }

    public void TurnByPercentSpeed(float percent)
    {
        float ratio = percent / 100.0f;
        _currentRotationSpeed = _rotationSpeed * ratio; 
    }
}
