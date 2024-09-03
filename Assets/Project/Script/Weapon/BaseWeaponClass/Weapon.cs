using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    #region Variable
    [Header ("Base Weapon Data")]
    [SerializeField] private Sprite _iconWeapon;
    [SerializeField] private GameObject _armLeft;
    [SerializeField] private GameObject _armRight;
    [SerializeField] private Transform _body;





    protected bool _isPlayer = false;
    protected bool _isReloading;
    protected WeaponHolder _weaponHolder;
    #endregion

    #region Getter Setter

 
    public abstract bool WeaponReady{ get; }
    public virtual bool InfinityAmmo { get;  }
    public bool FlipBody { get => _flipBody; set => _flipBody = value; }
    public bool IsReloading { get => _isReloading; set => _isReloading = value; }
    public Sprite Sprite { get => _iconWeapon; set => _iconWeapon = value; }
    public abstract Transform BaseShootDir { get; }
    public Transform Body { get => _body;  }
    #endregion

    #region Events
    [Header("Events")]
    public UnityEvent<float> ReloadingWeaponEvent;
    public UnityEvent UseWeaponEvent;
    public UnityEvent CancelUseWeaponEvent;
    public UnityEvent EndReloadingWeaponEvent;
    public UnityEvent SwitchWeaponEvent;//OnSwitchWeapon
    #endregion

    #region Virtual Method
    public virtual void SetupWeapon(WeaponHolder weaponHolder) 
    {
        _weaponHolder = weaponHolder;
        _isPlayer = _weaponHolder.IsPlayer;

        

    }
    public virtual void SetupWeapon(WeaponHolder weaponHolder,bool fulMag)
    {
        _weaponHolder = weaponHolder;
        _isPlayer = _weaponHolder.IsPlayer;
       

    }
    #endregion

    #region Abstract Method
    public abstract void UseWeapon();
    public abstract void CancelUseWeapon();
    public abstract void TryReloading();
    public abstract void QuickReloading();
    public abstract void SwitchWeapon();
    public abstract void SetStockAndMagAmmo((int, int) ammo);
    public abstract void RestoreAmmo(int precent);
    public abstract (int, int) GetStockAndMagAmmo();
    public abstract (int, int) GetStockAndMagMax();
    #endregion
}
