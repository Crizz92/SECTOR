using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {

    [HideInInspector]
    public Vector3 _destination;
    [HideInInspector]
    public Vector3 _lookDirection;
    private bool _reachedDestination;
    public float _transitionSpeed = 1.0f;

    // Use this for initialization
    void Start () {
        _destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if(!_reachedDestination)
        {
            transform.position = Vector3.Lerp(transform.position, _destination, Time.deltaTime * _transitionSpeed);
            Vector3 toDestination = _destination - transform.position;
            Quaternion _directionToLook = Quaternion.Euler(-Quaternion.FromToRotation(Vector3.right, _lookDirection).eulerAngles.x, Quaternion.FromToRotation(Vector3.forward, _lookDirection).eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, _directionToLook, Time.deltaTime * _transitionSpeed);
            if (toDestination.magnitude <= 0.1f) _reachedDestination = true;
        }    
	}
    public void MoveTo(Vector3 newDestination, Vector3 lookDirection)
    {
        _reachedDestination = false;
        _destination = newDestination;
        _lookDirection = lookDirection;
    }
}
