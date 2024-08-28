using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponContainer : MonoBehaviour, IInteractable
{
    [SerializeField] protected Weapon _weapon;
    [SerializeField] protected SpriteRenderer _render;

    [SerializeField]  protected UnityEvent _selectedEvent;
    [SerializeField] protected bool _isActiveContainer = true;

    protected CharacterWeaponHolder _holder;
    private Weapon _backWeapon;


    private (int, int) _ammo = (-1, -1);
    public bool IsActiveContainer { get => _isActiveContainer; set => _isActiveContainer = value; }

    public Transform GetTransform => transform;

    public (int, int) Ammo { get => _ammo; set => _ammo = value; }
    public Weapon BackWeapon { get => _backWeapon; set => _backWeapon = value; }

    protected virtual void Start()
    {
        SetVisual();
    }
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

    public void Interact(CharacterWeaponHolder holder)
    {
        TakeWeaponCotainer(holder);
    }
    
    //public void OnTriggerEnter(Collider other)
    //{
    //    if (_holder) return;
    //    if (other.TryGetComponent(out CharacterWeaponHolder holder) && IsActiveContainer)
    //    {
    //        Weapon currentWeapon = _backWeapon == null ? _weapon : _backWeapon;
    //        if (holder.ExchangeWeapon(currentWeapon, _ammo, out Weapon backWeapon))
    //        {
    //            _holder = holder;
    //            _selectedEvent?.Invoke();
    //            if (backWeapon)
    //            {
    //                if (_backWeapon)
    //                {
    //                    Destroy(_backWeapon.gameObject);
    //                }
    //                _ammo = backWeapon.GetStockAndMagAmmo();
    //                backWeapon.transform.SetParent(transform);
    //                backWeapon.transform.position = Vector3.zero;
    //                backWeapon.gameObject.SetActive(false);
    //                _backWeapon = backWeapon;
    //                SetVisual();
    //            }
    //            else
    //            {
    //                Destroy(gameObject);
    //            }

    //        }
    //    }
    //}
    //public void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent(out CharacterWeaponHolder holder))
    //    {
    //        if (holder == _holder)
    //        {
    //            _holder = null;
    //        }

    //    }
    //}
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
    public virtual void OffAfterUse() 
    {
        Destroy(gameObject);
    }

    public void Interact(CharacterInteractable interactable)
    {
        TakeWeaponCotainer(interactable.WeaponHolder);
    }

    public bool HoverObject(bool isHover, CharacterInteractable interactable)
    {
        OnHover(isHover,interactable);
        return true;

    }
    public virtual void OnHover(bool isHover, CharacterInteractable interactable) 
    {
    
    }
}
