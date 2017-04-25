using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInformations : Singleton<GlobalInformations> {

    [SerializeField]
    private List<Color> _playerColorList = new List<Color>();
    public List<Color> PlayerColorList
    {
        get { return _playerColorList; }
    }
}
