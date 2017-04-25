using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetIndicator : MonoBehaviour {

    [SerializeField]
    private Transform _target;
    public Vector3 _decal = new Vector3(0, 2, 0);


    public void Initialize(Transform target)
    {
        _target = target;

    }
    void Update()
    {
        transform.position = _target.position + _decal;
        transform.rotation = Quaternion.identity;
    }

}
