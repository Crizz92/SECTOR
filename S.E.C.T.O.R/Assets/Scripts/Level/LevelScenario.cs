using UnityEngine;
using System.Collections;

public class LevelScenario : MonoBehaviour {

    public virtual void Initialize()
    {

    }

    public virtual void SetTargetIndicator(Transform target)
    {
        LocationIndicator.Instance.Target = target;
    }
    public virtual void DisableTargetIndicator()
    {
        LocationIndicator.Instance.Target = null;
    }

    public virtual void ExitLevel()
    {
        FindObjectOfType<DronesHealthBar>().Hide();
    }
}
