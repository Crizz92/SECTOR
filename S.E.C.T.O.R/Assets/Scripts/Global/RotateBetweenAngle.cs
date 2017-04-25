using UnityEngine;
using System.Collections;

public class RotateBetweenAngle : MonoBehaviour {

    public float _minAngleX;
    public float _maxAngleX;
    public float _minAngleY;
    public float _maxAngleY;
    public float _minAngleZ;
    public float _maxAngleZ;
    private Vector3 _minAngleXYZ;
    private Vector3 _maxAngleXYZ;

    private Vector3 _currentAngleXYZ;
    public Vector3 _directionXYZ;
    public Vector3 _rotationSpeed;
    public float _gizmosSize = 2.0f;
    public bool _invertGizmos = false;
    private int _gizmosDirection = 1;

    void OnDrawGizmos()
    {
        if(_invertGizmos)
        {
            _gizmosDirection = -1;
        }
        if(!Application.isPlaying)
        {
            transform.localEulerAngles = new Vector3(_minAngleX, _minAngleY, _minAngleZ);
            Vector3 worldRotation = transform.eulerAngles;
            _minAngleXYZ = new Vector3(_minAngleX + worldRotation.x, _minAngleY + worldRotation.y, _minAngleZ + worldRotation.z);
            _maxAngleXYZ = new Vector3(_maxAngleX + worldRotation.x, _maxAngleY + worldRotation.y, _maxAngleZ + worldRotation.z);
        }

        Debug.Log(_minAngleXYZ.z);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(_minAngleXYZ.x, _minAngleXYZ.y, _minAngleXYZ.z) * (_gizmosDirection * Vector3.forward * _gizmosSize));
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(_maxAngleXYZ.x, _minAngleXYZ.y, _minAngleXYZ.z) * (_gizmosDirection * Vector3.forward * _gizmosSize));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(_minAngleXYZ.x, _minAngleXYZ.y, _minAngleXYZ.z) * (_gizmosDirection * Vector3.forward * _gizmosSize));
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(_minAngleXYZ.x, _maxAngleXYZ.y, _minAngleXYZ.z) * (_gizmosDirection * Vector3.forward * _gizmosSize));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(_minAngleXYZ.x, _minAngleXYZ.y, _minAngleXYZ.z) * (_gizmosDirection * Vector3.right * _gizmosSize));
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(_minAngleXYZ.x, _minAngleXYZ.y, _maxAngleXYZ.z) * (_gizmosDirection * Vector3.right * _gizmosSize));
 
    }
	void Start () {
        Vector3 worldRotation = transform.eulerAngles;
        _minAngleXYZ = new Vector3(_minAngleX + worldRotation.x, _minAngleY + worldRotation.y, _minAngleZ + worldRotation.z);
        _maxAngleXYZ = new Vector3(_maxAngleX + worldRotation.x, _maxAngleY + worldRotation.y, _maxAngleZ + worldRotation.z);

        _currentAngleXYZ = new Vector3(_minAngleX, _minAngleY, _minAngleZ);
        _directionXYZ = new Vector3(1, 1, 1);
        SetRotation(_currentAngleXYZ);
	}
	
	// Update is called once per frame
	void Update () {

        if (_currentAngleXYZ.x > _maxAngleX) _directionXYZ.x = -1;
        else if (_currentAngleXYZ.x < _minAngleX) _directionXYZ.x = 1;
        if (_currentAngleXYZ.y > _maxAngleY) _directionXYZ.y = -1;
        else if (_currentAngleXYZ.y < _minAngleY) _directionXYZ.y = 1;
        if (_currentAngleXYZ.z > _maxAngleZ) _directionXYZ.z = -1;
        else if (_currentAngleXYZ.z < _minAngleZ) _directionXYZ.z = 1;
        _currentAngleXYZ += new Vector3(_rotationSpeed.x * _directionXYZ.x, _rotationSpeed.y * _directionXYZ.y, _rotationSpeed.z * _directionXYZ.z) * Time.deltaTime;

        SetRotation(_currentAngleXYZ);

	}

    void SetRotation(Vector3 rotation)
    {
        transform.localEulerAngles = rotation;
    }
}
