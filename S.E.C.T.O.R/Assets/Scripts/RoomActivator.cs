using UnityEngine;
using System.Collections;

public class ActivityManager : MonoBehaviour {

    private Entity _entity;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ActiveArea")
        {
            
        }
    }
    
}
