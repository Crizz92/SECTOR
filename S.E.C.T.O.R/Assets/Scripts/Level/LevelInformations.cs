using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInformations : MonoBehaviour {

    public string _adventureName;
    public Dictionary<string, string> _textTranslation = new Dictionary<string, string>();
    [SerializeField]
    private Vector2 _mapSize;
    public Vector2 MapSize
    {
        get { return _mapSize; }
    }

    [SerializeField]
    private Transform _customOrigin;
    public Transform CustomOrigin
    {
        get { return _customOrigin; }
    }
    [SerializeField]
    private Transform _topRight;
    public Transform TopRight
    {
        get { return _topRight; }
    }
    private Vector2 _levelSize;

    public void InitializeLevel()
    {
        EntityManager.Initialize();
        EntityManager.Activate();
        Settings._currentLevelTranslation = XMLParser.ParseXMLFile(_adventureName, Settings._language);
        LevelScenario _levelScenario = FindObjectOfType<LevelScenario>();
        if (_levelScenario) _levelScenario.Initialize();
        FindObjectOfType<LocationIndicator>().Initialize();
        _levelSize = _topRight.position - _customOrigin.position;
        Minimap.Instance.Display(true);
        FindObjectOfType<InGameCamera>().Initialize();
    }

    [SerializeField]
    private CheckPoint[] _checkPointList;

    public CheckPoint GetCheckpoint()
    {
        CheckPoint lastCheckPointUnlock = null;
        for(int i = 0; i < _checkPointList.Length; i++)
        {
            if (_checkPointList[i] && _checkPointList[i]._unlocked)
            {
                lastCheckPointUnlock = _checkPointList[i];
            }
            if(_checkPointList[i] && !_checkPointList[i]._unlocked)
            {
                return lastCheckPointUnlock;
            }
        }
        return lastCheckPointUnlock;
    }

    private Vector3 PositionRelativeToCustomOrigin(Vector3 position)
    {
        return position - _customOrigin.position;
    }
    
    public Vector2 PositionRatioInLevel(Vector3 position)
    {
        Vector3 positionRelativeToCustomOrigin = PositionRelativeToCustomOrigin(position);
        float ratioX = positionRelativeToCustomOrigin.x / _levelSize.x;
        float ratioY = positionRelativeToCustomOrigin.y / _levelSize.y;
        return new Vector2(ratioX, ratioY);
    }

}
