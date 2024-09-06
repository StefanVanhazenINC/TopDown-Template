using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
namespace TopDown_Template
{
    public class HealthPack : PickUpObject
    {
        #region Variable 
        [SerializeField] private int _valueHeal;
        #endregion
        #region PickUpObject override Method

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
        #endregion

    }
}