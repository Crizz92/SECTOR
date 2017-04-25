using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
    [SerializeField]
    private Transform _spawnP1;
    [SerializeField]
    private Transform _spawnP2;

    public Vector3 _spawnPositionP1
    {
        get { return _spawnP1.position; }
    }
    public Vector3 _spawnPositionP2
    {
        get { return _spawnP2.position; }
    }
}
