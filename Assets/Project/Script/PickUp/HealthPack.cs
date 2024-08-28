using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HealthPack : PickUpObject
{
    [SerializeField] private int _valueHeal;
    public override bool PickUp(CharacterInteractable character) 
    {
        if (character.PlayerHealth.TryTakeHeal(_valueHeal))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
   
}
