using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TopDown_Template
{
    public class WeaponCardUi : MonoBehaviour
    {
        #region Variable
        [SerializeField] private TMP_Text _ammoCounter;
        [SerializeField] private TMP_Text _nameWeapon;
        [SerializeField] private Image _iconWeapon;
        #endregion

        #region WeaponCardUi Method
        public void SetCard(Sprite iconWeapon, string nameWeapon, string ammoMag, string ammoStock)
        {
            _iconWeapon.sprite = iconWeapon;
            _iconWeapon.SetNativeSize();
            _nameWeapon.text = nameWeapon;
            _ammoCounter.text = ammoMag + "/" + ammoStock;
        }
        public void UpdateAmmo(string ammoMag, string ammoStock)
        {
            _ammoCounter.text = ammoMag + "/" + ammoStock;
        }
        #endregion
    }
}
