using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InGameMenu : Singleton<InGameMenu> {

    [SerializeField]
    private List<MenuTab> _menuList = new List<MenuTab>();

    [SerializeField]
    private MenuTab _mainPauseMenu;

    private MenuTab _currentMenuTab;
    private EventSystem _eventSystem;


    public void Initialize()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        foreach(MenuTab menuTab in _menuList)
        {
            menuTab.gameObject.SetActive(false);
        }
    }

    public void OpenPauseMenu()
    {
        GameManager.PauseGame();
        _mainPauseMenu.ActivateAllButton();
        _currentMenuTab = _mainPauseMenu;
        _currentMenuTab.Initialize();
        _currentMenuTab.gameObject.SetActive(true);
        _eventSystem.SetSelectedGameObject(_currentMenuTab.FirstButtonSelected);
    }

    public void ClosePauseMenu()
    {
        GameManager.UnpauseGame();
        _currentMenuTab.DisableAllButton();
        _currentMenuTab.gameObject.SetActive(false);
        _currentMenuTab = null;
    }

    public void GoToTab(MenuTab menuTab)
    {
        foreach(MenuTab tab in _menuList)
        {
            if(menuTab == tab)
            {
                _currentMenuTab.DisableAllButton();
                _currentMenuTab.gameObject.SetActive(false);

                menuTab.gameObject.SetActive(true);
                _currentMenuTab = menuTab;
                return;
            }
        }
    }

    public void BackToMainMenu()
    {
        FindObjectOfType<DronesHealthBar>().Hide();
        FindObjectOfType<Minimap>().Display(false);
        FindObjectOfType<DronesManager>().DeactivateHUD();
        FindObjectOfType<LevelScenario>().DisableTargetIndicator();
        EntityManager.Clear();
        PopupManager.DestroyAllPopup();
        GameManager.BackToMainMenu();
        ClosePauseMenu();
    }
}