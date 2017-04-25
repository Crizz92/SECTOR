using UnityEngine;
using System.Collections;

public class Teleportation : Skill {
    
    public float _maxTeleportDistance = 20.0f;
    [SerializeField]
    private ParticleSystem _teleportFlashPrefab;
    private ParticleSystem _teleportFlash;

    protected override void Awake()
    {
        base.Awake();
        _teleportFlash = Instantiate(_teleportFlashPrefab);
        //_teleportFlash.transform.SetParent(_owner.transform);
        _teleportFlash.transform.position = _owner.transform.position - (Vector3.forward * 1.0f);
    }

    protected override void Update()
    {
        base.Update();

    }

    public override void Activate()
    {
        if(_unlocked)
        {
            Vector3 teleportationPoint = _owner.transform.position + (_owner.transform.up * _maxTeleportDistance);

            if (CheckTeleportPoint(teleportationPoint))
            {
                base.Activate();
                _teleportFlash.transform.position = _owner.transform.position;
                _teleportFlash.Play();
                _owner.TrailEnabled(false);
                _owner.transform.position = teleportationPoint;
                _owner.TrailEnabled(true);
            }
        }
    }

    bool CheckTeleportPoint(Vector3 position)
    {
        Vector3 raycastPoint = position - (Vector3.forward * 5);

        //RaycastHit hit;
        LayerMask layerToIgnore = ~(1 << LayerMask.NameToLayer("OnlyWithPlayer"));
        if (Physics.CheckSphere(position, 0.0f, layerToIgnore))
        {
            return false;
        }
        else return true;
        //if(Physics.Raycast(raycastPoint, Vector3.forward, out hit, 20.0f, layerToIgnore))
        //{
        //    if(hit.collider.tag == "Background") return true;
        //}
        //Physics.Chec
        //return false;
    }
}
