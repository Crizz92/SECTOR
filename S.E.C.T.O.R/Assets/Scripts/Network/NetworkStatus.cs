using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkStatus : MonoBehaviour {

    [SerializeField]
    private Text _statusText = null;

	void Update () {
        _statusText.text = MatchMakerManager.Status;
	}
}
