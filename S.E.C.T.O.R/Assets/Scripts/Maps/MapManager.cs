using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    [SerializeField]
    private Transform _chapterSelection;
    [SerializeField]
    private LevelSelectionPanel _levelSelection;

    private List<MapInfo> _mapList = new List<MapInfo>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
