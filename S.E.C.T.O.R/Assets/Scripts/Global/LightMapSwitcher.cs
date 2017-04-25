using UnityEngine;

using System.Linq;

public class LightMapSwitcher : MonoBehaviour
{
    [Header("Near = dir |||||| Far = Light")]
    public Texture2D[] DayNear;
    public Texture2D[] DayFar;
    public Texture2D[] NightNear;
    public Texture2D[] NightFar;

    private LightmapData[] dayLightMaps;
    private LightmapData[] nightLightMaps;

    void Start()
    {
        if ((DayNear.Length != DayFar.Length) || (NightNear.Length != NightFar.Length))
        {
            Debug.Log("In order for LightMapSwitcher to work, the Near and Far LightMap lists must be of equal length");
            return;
        }

        // Sort the Day and Night arrays in numerical order, so you can just blindly drag and drop them into the inspector
        DayNear = DayNear.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        DayFar = DayFar.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        NightNear = NightNear.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        NightFar = NightFar.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();

        // Put them in a LightMapData structure
        dayLightMaps = new LightmapData[DayNear.Length];
        for (int i = 0; i < DayNear.Length; i++)
        {
            dayLightMaps[i] = new LightmapData();
            dayLightMaps[i].lightmapDir = DayNear[i];
            dayLightMaps[i].lightmapLight = DayFar[i];
        }

        nightLightMaps = new LightmapData[NightNear.Length];
        for (int i = 0; i < NightNear.Length; i++)
        {
            nightLightMaps[i] = new LightmapData();
            nightLightMaps[i].lightmapDir = NightNear[i];
            nightLightMaps[i].lightmapLight = NightFar[i];
        }
    }

    #region Publics
    public void SetToDay()
    {
        LightmapSettings.lightmaps = dayLightMaps;
    }

    public void SetToNight()
    {
        LightmapSettings.lightmaps = nightLightMaps;
    }
    #endregion

    #region Debug
    void OnGUI()
    {
        if (GUILayout.Button("switch"))
        {
            if(day)
            { 
                SetToDay();
                day = !day;
            }
            else
            {
                SetToNight();
                day = !day;
            }
        }
    }

    private bool day = true;
    #endregion
}