using UnityEngine;
using System.Collections;

public interface IKillable
{
    void TakeDamage(Stats attacker, int damage);
    void OnDeath(Stats attacker);
}

