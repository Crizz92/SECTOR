using UnityEngine;
using System.Collections;

public class FromToMove : MonoBehaviour {

    private Vector3 _startPosition;
    [SerializeField]
    private Transform _targetPosition;
    private bool _move = false;
    [SerializeField]
    private float _speed;
    private float _currentProgessionRatio = 0.0f;
    [SerializeField]
    private bool _invert = false;

    void Awake()
    {
        _startPosition = transform.position;
    }
	// Update is called once per frame
	void Update () {
	    if(_move)
        {
            _currentProgessionRatio += Time.deltaTime * _speed * 0.1f;
            if(_currentProgessionRatio >= 1.0f)
            {
                _currentProgessionRatio = 1.0f;
                _move = false;
                return;
            }
            transform.position = Vector3.Lerp(_startPosition, _targetPosition.position, _currentProgessionRatio);
        }
	}

    public void StartMovement()
    {
        _move = true;
    }
    public void Loop()
    {

    }
    public void LoopBetween()
    {

    }
}
