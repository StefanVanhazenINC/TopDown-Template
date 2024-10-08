using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TopDown_Template
{
    public class WeaponHolder : MonoBehaviour
    {
        #region Variable 
        [Header("Main Setting")]
        [SerializeField] protected Weapon _weapon;
        [SerializeField] protected Transform _rotationPivor;
        [SerializeField] protected bool _isPlayer;


        protected bool _isShootInput;
        protected bool _isReloadingInput;
        protected bool _isBlockShootInput;//���������� �������� 
        protected bool _blockAimming;

        [Header("Events")]
        public UnityEvent SwitchWeaponEvent;
        public UnityEvent RestoreAmmoEvent;
        public UnityEvent UseWeaponEvent;
        public UnityEvent ReloadingWeaponEvent;
        public UnityEvent StartReloadingWeaponEvent;
        public UnityEvent EndReloadingWeaponEvent;
        public UnityEvent SetupWeaponEvent;
        public UnityEvent<Weapon, Weapon> TakeWeaponEvent;



        private bool _isShoot;

        private InputController _controller;
        #endregion

        #region Getter Setter
        public bool IsPlayer { get => _isPlayer; set => _isPlayer = value; }

        public Weapon CurrentWeapon { get => _weapon; set => _weapon = value; }
        #endregion

        #region Unity Callback
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
        #endregion
        #region WeaponHolder Method
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

        }
        #endregion
        #region WeaponHolder Virtual Method
        public virtual void SetupWeapon(Weapon weapon)
        {
            if (weapon)
            {

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
        #endregion



    }
}