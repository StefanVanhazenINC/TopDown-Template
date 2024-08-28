using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public void Interact(CharacterInteractable interactable);
    public bool HoverObject(bool isHover, CharacterInteractable interactable);
    public Transform GetTransform { get; }
 
}
