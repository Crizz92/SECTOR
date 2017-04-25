using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChapterSelectionPanel : MonoBehaviour {

    [SerializeField]
    private Button _firstSelected;
    [SerializeField]
    private List<Button> _chapterList = new List<Button>();

	// Use this for initialization
	void Start () {
        _firstSelected.onClick.Invoke();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickedChapter(Button buttonClicked)
    {
        foreach(Button button in _chapterList)
        {
            if (button == buttonClicked) button.interactable = false;
            else button.interactable = true;
        }
    }
}
