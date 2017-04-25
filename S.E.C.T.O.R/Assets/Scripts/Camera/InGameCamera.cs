using UnityEngine;
using System.Collections.Generic;

public class InGameCamera : MonoBehaviour {

    [SerializeField]
    private List<Transform> _targetList;
    private DronesManager _dronesManager;

    [SerializeField]
    private float _minDistance = 10.0f;

    [SerializeField]
    private float _cameraZoomBorder = 20.0f;

    [SerializeField]
    private float _cameraLerpSpeed = 2.0f;

    [SerializeField]
    private float _targetDistance;
    [SerializeField]
    private Vector3 _targetPosition;
    private Camera _camera;
    public Vector3 InLevelPosition
    {
        get
        {
            Vector3 inLevelPosition = transform.position;
            inLevelPosition.z = 0.0f;
            return inLevelPosition;
        }
    }

    private float _fovV = 0.0f;
    private float _fovH = 0.0f;
    private float _sinHalfFOV = 0.0f;

    private bool _focused = true;

    // Use this for initialization
    public void Initialize() {
        _camera = GetComponent<Camera>();
        _fovV = _camera.fieldOfView;
        _fovH = _camera.fieldOfView * _camera.aspect;
        _sinHalfFOV = Mathf.Sin(Mathf.Deg2Rad * (_fovV * 0.5f));
        _dronesManager = FindObjectOfType<DronesManager>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(_focused)
        {
            CenterCameraPos();
            CameraDistanceCorrection();
        }
	}

    private void CenterCameraPos()
    {
        Vector3 playersPosSum = Vector3.zero;
        for (int i = 0; i < _targetList.Count; i++)
        {
            playersPosSum += _targetList[i].position;
        }

        Vector3 playersAveragePos = Vector3.zero;
        if(_targetList.Count>0)
        {
            playersAveragePos = playersPosSum / _targetList.Count;
        }
        playersAveragePos.z = transform.position.z;
        _targetPosition = playersAveragePos;
        Vector3 currentPlayerPos = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _cameraLerpSpeed);
        currentPlayerPos.z = transform.position.z;
        transform.position = currentPlayerPos;
    }

    private void CameraDistanceCorrection()
    {
        if (_targetList.Count > 0)
        {
            float horizontalMultiplicator = 2.0f;
            float zDist = _minDistance;

            float smallestX = _targetList[0].position.x;
            float highestX = _targetList[0].position.x;
            float smallestY = _targetList[0].position.y;
            float highestY = _targetList[0].position.y;

            for (int i = 0; i < _targetList.Count; i++)
            {
                if (_targetList[i].position.x < smallestX) smallestX = _targetList[i].position.x;
                if (_targetList[i].position.x > highestX) highestX = _targetList[i].position.x;
                if (_targetList[i].position.y < smallestY) smallestY = _targetList[i].position.y;
                if (_targetList[i].position.y > highestY) highestY = _targetList[i].position.y;
            }
            
            float highestXDistance = highestX - smallestX + _cameraZoomBorder;
            float highestYDistance = highestY - smallestY + _cameraZoomBorder;

            if (highestXDistance < 0) highestXDistance = 0;
            if (highestYDistance < 0) highestYDistance = 0;


            float distZHorizontalFOV = (highestXDistance * 0.5f) / Mathf.Tan(Mathf.Deg2Rad * (_fovH * 0.4f));

            float distZVerticalFOV = (highestYDistance * 0.5f) / Mathf.Tan(Mathf.Deg2Rad * (_fovV * 0.5f));

            float zDistCalculated = 0.0f;
            if (distZHorizontalFOV > distZVerticalFOV)
            {
                zDistCalculated = distZHorizontalFOV;
                horizontalMultiplicator = 2.0f;
            }
            else
            {
                zDistCalculated = distZVerticalFOV;
                horizontalMultiplicator = 1.0f;
            }

            if (zDist < zDistCalculated) zDist = zDistCalculated;

            zDist += 5.0f *  Mathf.Clamp(((zDist - _minDistance) / _minDistance),0.0f,Mathf.Infinity) * horizontalMultiplicator;

            float currentDistance = Mathf.Lerp(transform.position.z, -zDist, Time.deltaTime * _cameraLerpSpeed);

            Vector3 correctedPosition = transform.position;
            correctedPosition.z = currentDistance;
            transform.position = correctedPosition;
        }
    }

    public void FreeCamera()
    {
        _focused = false;
    }

    public void FocusBack()
    {
        _focused = true;
    }

    public void AddTarget(Transform target)
    {
        _targetList.Add(target);
    }
    public void RemoveTarget(Transform target)
    {
        _targetList.Remove(target);
    }
    public void RemoveAllTarget()
    {
        _targetList.Clear();
    }
    public void FocusAlone(Transform target)
    {
        RemoveAllTarget();
        AddTarget(target);
    }

    public void TargetPlayers()
    {
        RemoveAllTarget();
        Debug.Log(_dronesManager);
        if (_dronesManager)
        {
            AddTarget(FindObjectOfType<DronesManager>().DroneP1.transform);
            AddTarget(FindObjectOfType<DronesManager>().DroneP2.transform);
        }
    }
    
}
