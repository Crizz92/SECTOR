using UnityEngine;
using System.Collections;

public interface IInteractable
{
    bool Interactable { get; }
    float InteractionRadius { get; }
    void Interact(Drone drone);
}

 