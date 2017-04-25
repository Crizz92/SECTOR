using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuoyancyForce : MonoBehaviour {

    public float _archimedeanForce = 10.0f;
    public float _archimedeanForceDeepMulitiplicator = 3.0f;
    public float _enterWaterForce = 5.0f;
    public Vector3 _flowDirection = Vector3.zero;
    public float _flowForce = 2.0f;

    private BoxCollider _waterCollider;
    private float _waterSurfaceHeight = 0.0f;
    private float _waterFloorHeight = 0.0f;

    private List<BuoyantAgent> _buoyantAgentList = new List<BuoyantAgent>();

    void Awake()
    {
        if (!_waterCollider) _waterCollider = GetComponent<BoxCollider>();
        _waterSurfaceHeight = _waterCollider.bounds.max.y;
        _waterFloorHeight = _waterCollider.bounds.min.y;
    }


    protected virtual void FixedUpdate()
    {
        foreach(BuoyantAgent buoyantAgent in _buoyantAgentList)
        {
            foreach(WeightPoint weightPoint in buoyantAgent.WeightPointList)
            {

                if (_waterCollider.bounds.Contains(weightPoint._worldPosition) && !weightPoint._inWater)
                {

                    weightPoint._inWater = true;
                    Vector3 enterWaterForce = (Vector3.up * _enterWaterForce) * ((60.0f - weightPoint._weight) / 40.0f);
                    float agentVelocityRatio = Mathf.Clamp01(buoyantAgent.AgentRigidbody.velocity.magnitude / 10.0f);
                    enterWaterForce *= agentVelocityRatio;
                    buoyantAgent.AgentRigidbody.AddForceAtPosition(enterWaterForce, weightPoint._worldPosition, ForceMode.Force);
                }
                else weightPoint._inWater = false;

                Vector3 force = Vector3.up * ((40.0f - weightPoint._weight) / 40.0f) * _archimedeanForce;
                float relativeDeep = Mathf.Clamp(_waterSurfaceHeight - weightPoint._worldPosition.y, 0.0f, Mathf.Infinity);
                float forceRatio = relativeDeep / _archimedeanForceDeepMulitiplicator;
                force *= forceRatio;
                buoyantAgent.AgentRigidbody.AddForceAtPosition(force, weightPoint._worldPosition, ForceMode.Force);
            }
            //if(_waterCollider.bounds.Contains(buoyantAgent.transform.position))
            //{
            //    if(!buoyantAgent._inWater)
            //    {
            //        //buoyantAgent.AgentRigidbody.velocity = buoyantAgent.AgentRigidbody.velocity * 0.5f;
            //        buoyantAgent._inWater = true;
            //    }
            //    //Vector3 force = Vector3.up * ((40.0f - buoyantAgent.AgentRigidbody.mass) / 40.0f) * _archimedeanForce;
            //    //float relativeDeep = Mathf.Clamp(_waterSurfaceHeight - buoyantAgent.transform.position.y, 0.0f, Mathf.Infinity);
            //    //float forceRatio = relativeDeep / _archimedeanForceDeepMulitiplicator;
            //    //force *= forceRatio;
            //    //force -= Physics.gravity;
            //    //buoyantAgent.AgentRigidbody.AddForce(force, ForceMode.Acceleration);
            //
            //}
            //else
            //{
            //    buoyantAgent._inWater = false;
            //}
        }
    }

    void OnTriggerEnter(Collider col)
    {
        BuoyantAgent buoyantAgent = col.GetComponent<BuoyantAgent>();
        if(buoyantAgent)
        {
            _buoyantAgentList.Add(buoyantAgent);
        }
    }
    void OnTriggerExit(Collider col)
    {
        BuoyantAgent buoyantAgent = col.GetComponent<BuoyantAgent>();
        if (buoyantAgent)
        {
            _buoyantAgentList.Remove(buoyantAgent);
        }
    }

}
