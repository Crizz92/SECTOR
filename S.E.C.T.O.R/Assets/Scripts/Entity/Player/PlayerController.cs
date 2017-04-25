using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public PlayerInput _playerInput;
    [SerializeField]
    private PlayerMotor _playerMotor;
    private Drone _drone;
    public bool _rotationIndependant = false;
    private Vector3 _currentRotation = Vector3.zero;

    void Start () {
        _playerMotor = GetComponent<PlayerMotor>();
        _drone = GetComponent<Drone>();
        if(_playerInput) _playerInput.Initialize();
	}

    void Update()
    {
        if(_playerInput.ButtonStart())
        {
            FindObjectOfType<InGameMenu>().OpenPauseMenu();
        }


        Vector2 direction = _playerInput.LeftJoystick();
        Quaternion rotation = transform.rotation;
        if(direction.magnitude >= 0.1f)
        {
            if(!_rotationIndependant)
            {
                rotation = Quaternion.Euler(0, 0, Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z);
            }
        }
        else
        {
            direction = Vector2.zero;
        }
        if(_rotationIndependant)
        {
            Vector2 inputRotation = _playerInput.RightJoystick();
            if (inputRotation.magnitude >= 0.1f)
            {
                _currentRotation = inputRotation;
                rotation =  Quaternion.Euler(0, 0, Quaternion.FromToRotation(Vector3.up, _currentRotation).eulerAngles.z);
            }
        }
        _playerMotor.Move(direction);
        _playerMotor.Rotate(rotation, false);



    }
}
