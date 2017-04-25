using UnityEngine;
using System.Collections;



public class SkillChipset : InteractifElement {

    [SerializeField]
    private string _skillName;
    [SerializeField]
    private ParticleSystem _particles;
    [SerializeField]
    private ParticleSystem _glow;
    [SerializeField]
    private MaterialModifier _materialModifier;
    [SerializeField]
    private Light _light;
    [SerializeField]
    private PopupInfoPause _popupPrefab;

    public override void Interact(Drone drone)
    {
        if(drone.DroneType == _interactWith)
        {

            base.Interact(drone);
            drone.UnlockSkill(_skillName);
            OutOfRange(drone);
            Destroy(gameObject);
            PopupManager.CreatePopup(_popupPrefab, PopupManager._center);
            GameManager.PauseGame();
        }
    }

    public override void AdaptColor()
    {
        base.AdaptColor();
        PlayerInformations info = CoopManager.Instance.GetDroneInfo(_interactWith);

        _particles.startColor = info._color;
        _glow.startColor = info._color;
        _materialModifier.SetEmissiveColor(info._color * 3.0f);
        _materialModifier.SetOutlineColor(info._color);
        _light.color = info._color;
    }
}
