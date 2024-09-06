using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace TopDown_Template
{
    public class ReloadingTips : MonoBehaviour
    {
        #region Variable
        [SerializeField] private CharacterWeaponHolder _weponHolder;
        [SerializeField] private GameObject _reloadingTip;
        #endregion
        #region UnityCallback
        public void Start()
        {
            Construct(_weponHolder);
        }
        #endregion

        #region ReloadingTips Method
        public void Construct(CharacterWeaponHolder weponHolder)
        {
            _weponHolder = weponHolder;
            _weponHolder.UseWeaponEvent.AddListener(CheckReloading);
            _weponHolder.ReloadingWeaponEvent.AddListener(CheckReloading);
            _weponHolder.EndReloadingWeaponEvent.AddListener(CheckReloading);
            _weponHolder.SwitchWeaponEvent.AddListener(CheckReloading);
        }
        private void CheckReloading()
        {
            if (_weponHolder.CurrentWeapon.GetStockAndMagAmmo().Item2 <= 0 || _weponHolder.CurrentWeapon.IsReloading)
            {
                _reloadingTip.SetActive(true);
            }
            else
            {
                _reloadingTip.SetActive(false);
            }
        }
        #endregion
    }
}
