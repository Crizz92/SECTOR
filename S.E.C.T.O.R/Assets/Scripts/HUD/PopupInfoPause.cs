using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupInfoPause : PopupElement {

    protected override void Update()
    {
        base.Update();
        if(FindObjectOfType<DronesManager>().DroneP1.PlayerInput.ButtonX() ||
                    FindObjectOfType<DronesManager>().DroneP1.PlayerInput.ButtonX())
        {
            GameManager.UnpauseGame();
            PopupManager.DestroyPopup(this);
        }
    }

}
