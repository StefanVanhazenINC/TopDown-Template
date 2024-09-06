using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown_Template
{
    public class AmmoPack : PickUpObject
    {
        #region Variable 
        [SerializeField, Range(1, 4f)] private int _precentRestoreAmmo;
        #endregion

        #region PickUpObject override Method
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
        #endregion
    }
}
