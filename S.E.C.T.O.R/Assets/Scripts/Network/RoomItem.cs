using UnityEngine;
using UnityEngine.Networking.Match;

public class RoomItem : MonoBehaviour {

    private string _roomName = "default";
    public MatchInfoSnapshot _matchDesc;

    public void Setup(MatchInfoSnapshot matchDesc)
    {
        _matchDesc = matchDesc;
    }
}
