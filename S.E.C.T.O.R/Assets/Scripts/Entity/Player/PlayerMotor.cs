using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {

    private Vector3 _velocity = Vector3.zero;
    private bool _moving = false;
    public bool Moving
    {
        get { return _moving; }
    }
    private bool _canMove = true;
    public bool CanMove
    {
        get { return _canMove; }
    }

    public float _startSpeed = 10.0f;
    public float _acceleration = 1.0f;
    public float _decceleration = 1.0f;
    public Vector3 _direction = Vector3.zero;
    public float _maxVelocity = 10.0f;
    private Quaternion _rotation = Quaternion.identity;
    public float _rotationSpeed = 3.0f;
    private Rigidbody _rigidbody;

	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    public void AllowMovement(bool allow)
    {
        _canMove = allow;
        if (!_canMove) _rigidbody.velocity = Vector3.zero;
    }

    public void Move(Vector3 direction)
    {
        if(_canMove && direction != Vector3.zero)
        {
            _direction = direction;
            if (_rigidbody && !_moving) _rigidbody.velocity = _direction * _startSpeed;
            _moving = true;
        }
        else
        {
            _moving = false;
        }
    }
    public void Rotate(Quaternion rotation, bool instant)
    {
        _rotation = rotation;
        if(instant)
        {
            transform.rotation = rotation;
        }
    }


    void PerformMovement()
    {
        if(_moving)
        {
            _rigidbody.AddForce(_direction * _acceleration);
            if (_rigidbody.velocity.magnitude > _maxVelocity)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxVelocity;
            }
        }
        else
        {
            _rigidbody.velocity -= _rigidbody.velocity * Time.deltaTime * _decceleration;
            if (_rigidbody.velocity.magnitude <= 0.1f)
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }
    }
    void PerformRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.deltaTime * _rotationSpeed);
    }

}
