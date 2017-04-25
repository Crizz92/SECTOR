using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class CableLineRender : MonoBehaviour {

    public Transform _pos1;
    public Transform _pos2;
    public LineRenderer _lineRenderer;
    [SerializeField]
    private bool _enabled = true;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        if (!_pos1 && !_pos2) _lineRenderer.enabled = false;
    }
	// Update is called once per frame
	void Update () {

        if(_enabled)
        {
            if (_pos1) _lineRenderer.SetPosition(0, _pos1.position);
            if (_pos2) _lineRenderer.SetPosition(1, _pos2.position);
        }

	}

    public void Deactivate()
    {
        _lineRenderer.enabled = false;
        _enabled = false;
    }

    public void Activate()
    {
        _lineRenderer.enabled = true;
        _enabled = true;
    }
}
