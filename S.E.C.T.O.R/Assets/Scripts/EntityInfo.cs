using UnityEngine;
using System.Collections;

public enum EntityType
{
    Drone,
    Cube,

}

public class EntityInfo : MonoBehaviour {

    public string _name = " ";
    public string _description = " ";
    public EntityType _type = EntityType.Cube;
    public Sprite _image;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
