using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    public bool _unlocked = false;
    public Transform _spawnA;
    public Transform _spawnB;

    public Vector3 SpawnPosition(int index)
    {
        Vector3 spawnPosition = (index > 0) ? _spawnA.position : _spawnB.position;
        return spawnPosition;
    }

    public void Unlock()
    {
        _unlocked = true;
    }
}
