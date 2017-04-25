using UnityEngine;
using System.Collections;

public class LightmapManager : MonoBehaviour {

    [SerializeField]
    public LightmapData[] _lightmapsTest;
    private LightmapData[] _lightmap;
	// Use this for initialization
	void Start () {
         _lightmap = LightmapSettings.lightmaps;
        DeactivateLightmap();
        //Invoke("DeactivateLightmap", 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void ActivateLightmap()
    {
        LightmapSettings.lightmaps = _lightmap;
        Invoke("DeactivateLightmap", 3.0f);
    }

    void DeactivateLightmap()
    {
        LightmapSettings.lightmaps = new LightmapData[0];
        Invoke("ActivateLightmap", 3.0f);

    }
}
