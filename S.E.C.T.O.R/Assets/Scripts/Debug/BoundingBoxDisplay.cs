using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxDisplay : MonoBehaviour {

    private Collider col;

    void OnDrawGizmos()
    {
        col = GetComponent<Collider>();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, col.bounds.size);
    }

}
