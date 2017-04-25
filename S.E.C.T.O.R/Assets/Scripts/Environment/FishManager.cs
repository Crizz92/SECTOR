using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Water))]
public class FishManager : MonoBehaviour {

    [SerializeField]
    private List<Fish> _fishPrefabs;

    [SerializeField]
    private int _numberFish = 10;

    private Water _water;
    public Water Water
    {
        get { return _water; }
    }

    [SerializeField]
    private Transform _spawnPosition;
    private List<Fish> _fishList = new List<Fish>();

    void Awake()
    {
        _water = GetComponent<Water>();
        for(int i = 0; i < _numberFish; i++)
        {
            Fish fish = Instantiate(_fishPrefabs[Random.Range(0, _fishPrefabs.Count)], _spawnPosition.position, Quaternion.identity);
            fish.Initialize(this);
            fish.transform.localScale = Vector3.one * Random.Range(0.2f, 1.0f);
            fish.transform.SetParent(transform);
            _fishList.Add(fish);
        }
    }
}
