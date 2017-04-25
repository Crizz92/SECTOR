using UnityEngine;
using System.Collections;

public class MovingWalkway : MonoBehaviour {

    public float _speed;
    public Vector3 _direction;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider col)
    {
        Entity entity = col.GetComponent<Entity>();
        if(entity)
        {
            entity.transform.Translate(_direction * _speed * Time.deltaTime);
        }
    }
}
