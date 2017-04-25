using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private MenuCamera _camera;

    [SerializeField]
    private MenuTab _startTab;
    [SerializeField]
    private List<MenuTab> _menuTabList;
    private MenuTab _currentMenuTab = null;
    [SerializeField]
    private EventSystem _eventSystem;

    public void Initialize()
    {
        _eventSystem = UIManager.Instance.UIEventSystem;
        for (int i = 0; i < _menuTabList.Count; i++)
        {
            _menuTabList[i].DisableAllButton();
            _menuTabList[i]._mainMenu = this;
        }
        _startTab.ActivateAllButton();
        _currentMenuTab = _startTab;
        GoToTab(_startTab._name);
    }
	// Use this for initialization
	//void Start () {
 //       _eventSystem = UIManager.Instance.UIEventSystem;
	//    for(int i = 0; i < _menuTabList.Count; i++)
 //       {
 //           _menuTabList[i].DisableAllButton();
 //           _menuTabList[i]._mainMenu = this;
 //       }
 //       _startTab.ActivateAllButton();
 //       _currentMenuTab = _startTab;
 //       GoToTab(_startTab._name);
	//}
	
	// Update is called once per frame
	void Update ()
    {
        PlayerInput globalInput = GameManager.GlobalPlayerInput;
        if (!_eventSystem.currentSelectedGameObject && globalInput.AnInputWasPressed)
        {
            _eventSystem.SetSelectedGameObject(_currentMenuTab.FirstButtonSelected);
        }
        if (_eventSystem.currentSelectedGameObject)
        {
            Selectable currentSelected = _eventSystem.currentSelectedGameObject.GetComponent<Selectable>();
            Selectable nextSelected = null;
            if (globalInput.BottomArrowDown()) nextSelected = currentSelected.FindSelectableOnDown();
            if (globalInput.TopArrowDown()) nextSelected = currentSelected.FindSelectableOnUp();
            if (globalInput.RightArrowDown()) nextSelected = currentSelected.FindSelectableOnRight();
            if (globalInput.LeftArrowDown()) nextSelected = currentSelected.FindSelectableOnLeft();
            if (nextSelected) _eventSystem.SetSelectedGameObject(nextSelected.gameObject);
        }
        if (globalInput.ButtonB())
        {
            if (_currentMenuTab._previousMenuTab)
            {
                //GoToTab(_currentMenuTab._previousMenuTab._name);
                _currentMenuTab.GoBack();
            }
        }

	}

    public void OpenMenuTab(string name)
    {
        for(int i = 0; i < _menuTabList.Count; i++)
        {
            MenuTab menuTab = _menuTabList[i];
            if(menuTab._name == name)
            {
                if(_currentMenuTab)
                {
                    _currentMenuTab.gameObject.SetActive(false);
                }
                menuTab.gameObject.SetActive(true);
                _currentMenuTab = menuTab;
                
                return;
            }
        }
    }
    //public void SetOpenTab(string name)
    //{
    //    for (int i = 0; i < _menuTabList.Count; i++)
    //    {
    //        MenuTab menuTab = _menuTabList[i];
    //        if (menuTab.name == name)
    //        {
    //            _currentMenuTab.DisableAllButton();
    //            menuTab.ActivateAllButton();
    //            _currentMenuTab = menuTab;
    //            //_camera.MoveTo(_currentMenuTab._viewPosition.position, _currentMenuTab.transform.forward);
    //            _eventSystem.SetSelectedGameObject(_currentMenuTab.FirstButtonSelected.gameObject);
    //        }
    //    }
    //}
    public void CloseCurrentMenuTab(string name)
    {
        _currentMenuTab.gameObject.SetActive(false);
        _currentMenuTab = null;
    }

    public void DisableTab(string name)
    {
        for(int i = 0; i < _menuTabList.Count; i++)
        {
            MenuTab menuTab = _menuTabList[i];
            if(menuTab.name == name)
            {
                menuTab.DisableAllButton();
            }
        }
    }

    public void GoToTab(string name)
    {
        for(int i = 0; i < _menuTabList.Count; i++)
        {
            MenuTab menuTab = _menuTabList[i];
            if(menuTab.name == name)
            {
                _currentMenuTab.DisableAllButton();
                menuTab.ActivateAllButton();
                _currentMenuTab = menuTab;
                _camera.MoveTo(_currentMenuTab._viewPosition.position, _currentMenuTab.transform.forward);
                _eventSystem.SetSelectedGameObject(_currentMenuTab.FirstButtonSelected.gameObject);
            }
        }
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void StartResearch()
    {
        MatchMakerManager.StartResearch();
    }
    public void LaunchCoop(string level)
    {
        GameManager.SetGameMod(EGameMod.Coop);
        SceneLoader.Instance.LoadScene(level);
    }

    public void SaveSettings()
    {
        foreach(MenuTab menuTab in _menuTabList)
        {
            if(menuTab._name == "Settings")
            {

            }
        }
    }
}
