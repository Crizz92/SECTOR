using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class WeightPoint
{
    public Vector3 _localposition = Vector3.zero;
    [HideInInspector]
    public Vector3 _worldPosition = Vector3.zero;
    [Range(0.0f,40.0f)]
    public float _weight = 0.0f;
    [HideInInspector]
    public bool _inWater = false; 
}

public class BuoyantAgent : MonoBehaviour {

    [SerializeField]
    private Rigidbody _rigidbody;
    public Rigidbody AgentRigidbody
    {
        get { return _rigidbody; }
    }

    [HideInInspector]
    public bool _inWater;


    private Color _lightWeightColor = Color.green;
    private Color _heavyWeightColor = Color.red;


    [SerializeField]
    private List<WeightPoint> _weightPointList = new List<WeightPoint>();
    public List<WeightPoint> WeightPointList
    {
        get { return _weightPointList; }
    }
    private List<Transform> _weightPointInstanceList = new List<Transform>();

	#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (Selection.Contains(gameObject))
        {
            for (int i = 0; i < _weightPointList.Count; i++)
            {
                _weightPointList[i]._worldPosition = transform.position + (transform.rotation * _weightPointList[i]._localposition);
            }

            foreach (WeightPoint weightPoint in _weightPointList)
            {
                Color color = Color.Lerp(_lightWeightColor, _heavyWeightColor, weightPoint._weight / 40.0f);
                Gizmos.color = color;
                Gizmos.DrawSphere(weightPoint._worldPosition, 0.1f);
            }
        }
    }
	#endif

    void Awake()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        for(int i = 0; i < _weightPointList.Count; i++)
        {
            _weightPointList[i]._worldPosition = transform.position + (transform.rotation * _weightPointList[i]._localposition);
        }
    }


     
}
