using UnityEngine;
using System.Collections;

public interface IElectricSensibility
{
    bool IsElectrocuted { get; }

    void Electrocute();
    void Reboot();
}

