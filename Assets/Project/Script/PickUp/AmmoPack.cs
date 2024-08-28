using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : PickUpObject
{
    [SerializeField, Range(1, 4f)] private int _precentRestoreAmmo;
    public override bool PickUp(CharacterInteractable character)
    {
        if (character.WeaponHolder.RestoreAmmo(_precentRestoreAmmo))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
