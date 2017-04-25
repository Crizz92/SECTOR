using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : Singleton<Minimap> {

    private bool _displayed = false;
    public bool Displayed
    {
        get { return _displayed; }
    }

    private Vector2 _mapSize;
    [SerializeField]
    private Vector2 _minimapSize;
    [SerializeField]
    private GameObject _mask;
    [SerializeField]
    private Transform _minimap;
    [SerializeField]
    private Image _indicatorPrefab;
    private LevelInformations _levelInformations;
    private DronesManager _dronesManager;
    private InGameCamera _ingameCamera;

    protected override void Awake()
    {
        base.Awake();
        _mask.SetActive(false);
    }

    void Update()
    {
        if(_displayed)
        {
            UpdateMapPosition();
        }
    }

    public void Display(bool display)
    {
        _displayed = display;
        if(display) Initialize();
        _mask.SetActive(display);
    }

    public void Initialize()
    {
        _levelInformations = FindObjectOfType<LevelInformations>();
        _dronesManager = FindObjectOfType<DronesManager>();
        _ingameCamera = FindObjectOfType<InGameCamera>();
        MinimapIndicator[] minimapIndicatorList = FindObjectsOfType<MinimapIndicator>();
        _minimapSize.x = _minimap.GetComponent<Image>().rectTransform.sizeDelta.x;
        _minimapSize.y = _minimap.GetComponent<Image>().rectTransform.sizeDelta.y;

        _mapSize = _levelInformations.MapSize;

        for(int i = 0; i < minimapIndicatorList.Length; i++)
        {
            MinimapIndicator currentMinimapIndicator = minimapIndicatorList[i];
            if(currentMinimapIndicator)
            {
                currentMinimapIndicator.Initialize();
            }
        }

    }
    public Image AddIndicator(MinimapIndicator indicator)
    {
        return AddIndicator(indicator, Vector3.zero);
    }
    public Image AddIndicator(MinimapIndicator indicator, Vector3 worldPosition)
    {
        Image imageIndicator = null;
        if (indicator)
        {
            imageIndicator = Instantiate(_indicatorPrefab);
            imageIndicator.rectTransform.sizeDelta = new Vector2(indicator.Size, indicator.Size);
            imageIndicator.sprite = indicator.Indicator;
            imageIndicator.transform.SetParent(_minimap);
            
            Vector3 positionOnMinimap = Vector3.zero;
            Vector2 positionRatioInLevel = _levelInformations.PositionRatioInLevel(worldPosition);
            positionOnMinimap.x = positionRatioInLevel.x * _minimapSize.x;
            positionOnMinimap.y = positionRatioInLevel.y * _minimapSize.y;

            imageIndicator.transform.localPosition = positionOnMinimap;
        }
        return imageIndicator;
    }
    public void UpdateIndicatorPosition(MinimapIndicator indicator)
    {
        UpdateIndicatorPosition(indicator, Vector3.zero);
    }
    public void UpdateIndicatorPosition(MinimapIndicator indicator, Vector3 worldPosition)
    {
        Vector3 positionOnMinimap = Vector3.zero;
        Vector2 positionRatioInLevel = _levelInformations.PositionRatioInLevel(worldPosition);
        positionOnMinimap.x = positionRatioInLevel.x * _minimapSize.x;
        positionOnMinimap.y = positionRatioInLevel.y * _minimapSize.y;

        indicator.MinimapTarget.transform.localPosition = positionOnMinimap;
    }

    void UpdateMapPosition()
    {
        Vector3 mapPosition = Vector3.zero;

        Vector2 centerRatioInLevel = _levelInformations.PositionRatioInLevel(_ingameCamera.InLevelPosition);
        mapPosition.x = _minimapSize.x * centerRatioInLevel.x;
        mapPosition.y = _minimapSize.y * centerRatioInLevel.y;
        
        _minimap.localPosition = -mapPosition;
    }

}
