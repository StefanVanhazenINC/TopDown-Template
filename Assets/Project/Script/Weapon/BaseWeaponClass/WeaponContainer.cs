using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace TopDown_Template
{
    public class WeaponContainer : MonoBehaviour, IInteractable
    {
        #region Variable
        [SerializeField] protected Weapon _weapon;
        [SerializeField] protected SpriteRenderer _render;

        [SerializeField] protected UnityEvent _selectedEvent;
        [SerializeField] protected bool _isActiveContainer = true;

        protected CharacterWeaponHolder _holder;
        private Weapon _backWeapon;
        private (int, int) _ammo = (-1, -1);
        #endregion

        #region Getter Setter
        public bool IsActiveContainer { get => _isActiveContainer; set => _isActiveContainer = value; }
        public Transform GetTransform => transform;
        public (int, int) Ammo { get => _ammo; set => _ammo = value; }
        public Weapon BackWeapon { get => _backWeapon; set => _backWeapon = value; }
        #endregion

        #region UnityCallback
        protected virtual void Start()
        {
            SetVisual();
        }
        #endregion

        #region WeaponContainer Method
        public void Interact(CharacterWeaponHolder holder)
        {
            TakeWeaponCotainer(holder);
        }
        public bool CompareWeapon(List<Weapon> compareWeapon, out int index) //сравнить jоружие в стеше и в контайнере 
        {
            index = -1;
            for (int i = 0; i < compareWeapon.Count; i++)
            {
                if (compareWeapon[i].name == _weapon.name)
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }
        public bool HoverObject(bool isHover, CharacterInteractable interactable)
        {
            return true;
        }
        public void Interact(CharacterInteractable interactable)
        {
            TakeWeaponCotainer(interactable.WeaponHolder);
        }
        #endregion

        #region WeaponContainer Virtual Method
        public virtual void SetVisual()
        {
            if (_backWeapon)
            {
                _render.sprite = _backWeapon.Sprite;
            }
            else
            {
                _render.sprite = _weapon.Sprite;
            }

        }
        public virtual void TakeWeaponCotainer(CharacterWeaponHolder holder)
        {
            if (IsActiveContainer)
            {
                Weapon currentWeapon = _backWeapon == null ? _weapon : _backWeapon;
                if (holder.ExchangeWeapon(currentWeapon, _ammo, out Weapon backWeapon))
                {
                    _holder = holder;
                    _selectedEvent?.Invoke();
                    if (backWeapon)
                    {
                        if (_backWeapon)
                        {
                            Destroy(_backWeapon.gameObject);
                        }
                        _ammo = backWeapon.GetStockAndMagAmmo();
                        backWeapon.transform.SetParent(transform);
                        backWeapon.transform.position = Vector3.zero;
                        backWeapon.gameObject.SetActive(false);
                        _backWeapon = backWeapon;
                        SetVisual();
                    }
                    else
                    {
                        OffAfterUse();
                    }

                }
            }

        }
        public virtual void OffAfterUse()
        {
            Destroy(gameObject);
        }
        #endregion

    }
}
