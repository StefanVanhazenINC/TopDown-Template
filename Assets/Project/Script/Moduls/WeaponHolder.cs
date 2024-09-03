using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class WeaponHolder : MonoBehaviour
{
    [Header("================================")]
    [Header("Main Setting")]
    [SerializeField] protected Weapon _weapon;
    [SerializeField] protected Transform _rotationPivor;
    [SerializeField] protected bool _isPlayer;

   
    protected bool _isShootInput;
    protected bool _isReloadingInput;
    protected bool _isBlockShootInput ;//блокировка стрельбы 
    protected bool _blockAimming;

    [Header("================================")]
    [Header("Events")]
    public UnityEvent SwitchWeaponEvent;
    public UnityEvent RestoreAmmoEvent;
    public UnityEvent UseWeaponEvent;
    public UnityEvent ReloadingWeaponEvent;
    public UnityEvent StartReloadingWeaponEvent;
    public UnityEvent EndReloadingWeaponEvent;
    public UnityEvent SetupWeaponEvent;
    public UnityEvent<Weapon, Weapon> TakeWeaponEvent;


    protected bool _flipVisual;

    private bool _isShoot;

    private InputController _controller;

    public bool IsPlayer { get => _isPlayer; set => _isPlayer = value; }
  
    public Weapon CurrentWeapon { get => _weapon; set => _weapon = value; }

    protected virtual void Awake()
    {
        _controller = GetComponent<InputController>();
    }
    protected virtual void Start()
    {
        _controller.OnAttackEvent.AddListener(UseWeapon);
        _controller.OnChangeWeaponEvent.AddListener(SwitchWeapon);
        _controller.OnReloadingWeaponEvent.AddListener(ReloadingWeapon);


        SetupWeapon(_weapon);
    }
    public virtual void Update() 
    {
        LogicUpdate();
    }
    private void LogicUpdate() 
    {
        if (_isShoot)
        {
            _weapon.UseWeapon();
        }
        else
        {
            _weapon.CancelUseWeapon();
        }

        //CheckToFlip();
    }
    public virtual void SetupWeapon(Weapon weapon) 
    {
        if (weapon) 
        {
            _flipVisual = weapon.FlipBody;
            weapon.SetupWeapon(this);

        }
    }
    public virtual void UseWeapon(bool isUse) 
    {
        _isShoot = isUse;
    }
    public virtual void ReloadingWeapon() 
    {
        _weapon.TryReloading();
    }
    public virtual void SwitchWeapon()
    {
      
    }

    public void CheckToFlip() 
    {
        if (_weapon) 
        {
            if (_flipVisual)
            {
                if (_rotationPivor.localRotation.eulerAngles.y > 0 && _rotationPivor.localRotation.eulerAngles.y < 180)
                {
                    if (_weapon)
                        _weapon.Body.localRotation = Quaternion.Euler(180, 0, 0);
                }
                else
                {
                    if (_weapon)
                        _weapon.Body.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        
        
    }
}
