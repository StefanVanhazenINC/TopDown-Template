using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDownController
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] private Movment _movment;
        [SerializeField] private CharacterWeaponHolder _weaponHolder;
        [SerializeField] private Aimming _aimming;
        [SerializeField] private CharacterInteractable _interactable;

        [Header("Parametrs")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _timeDash;
        [SerializeField] private float _rotationSpeed;


        [Header("Component")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _armPivot;
        [SerializeField] private Transform _handleWeaponPivot;

        [Header("Input")]
        [SerializeField] private InputAction _directionInput;
        [SerializeField] private InputAction _aimInput;
        [SerializeField] private InputAction _spaceInput;
        [SerializeField] private InputAction _interactInput;
        [SerializeField] private InputAction _actionInput;
        [SerializeField] private InputAction _secondActionInput;
        [SerializeField] private InputAction _specialActionInput;
        [SerializeField] private InputAction _chageWeapon;
        [SerializeField] private InputAction _reloadingWeaponWeapon;

        private Camera _mainCamera;
        //он будет модифицироваться исходя из оружия или чего то еще

        private void OnEnable()
        {
            _directionInput.Enable();
            _spaceInput.Enable();
            _aimInput.Enable();
            _interactInput.Enable();
            _actionInput.Enable();
            _secondActionInput.Enable();
            _specialActionInput.Enable();
            _chageWeapon.Enable();
            _reloadingWeaponWeapon.Enable();
        }

        private void OnDisable()
        {
            _directionInput.Disable();
            _spaceInput.Disable();
            _aimInput.Disable();
            _interactInput.Disable();
            _actionInput.Disable();
            _secondActionInput.Disable();
            _specialActionInput.Disable();
            _chageWeapon.Disable();
            _reloadingWeaponWeapon.Disable();

        }

        private void Awake()
        {
            _movment = GetComponent<Movment>();
            _aimming = GetComponent<Aimming>();
            _weaponHolder = GetComponent<CharacterWeaponHolder>();
            _animator = GetComponent<Animator>();
            //_dashing = GetComponent<DashingCore>();
            if (_movment != null)
            {
                //_directionInput.performed += MoveDirection;
                //_directionInput.canceled += MoveDirection;
            }
            if (_weaponHolder != null)
            {
                _actionInput.started += _weaponHolder.HandleInputAttack;
                _actionInput.canceled += _weaponHolder.HandleInputCancel;

                _chageWeapon.started += _weaponHolder.HandleInputQuickChangeWeapon;
                _reloadingWeaponWeapon.started += _weaponHolder.HandleInputReloading;

            }
            //if (_dashing) 
            //{
            //    _spaceInput.started += Dash;
            //}
            if (_interactable) 
            {
                _interactInput.started += _interactable.Interact;
            }
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_movment != null)
            {
                //_movment.LogicUpdate();
            }
            if (_aimming != null)
            {
                _aimming.AimPosition = _mainCamera.ScreenToWorldPoint(_aimInput.ReadValue<Vector2>());
                _aimming.MousePosition = _aimInput.ReadValue<Vector2>();
                //_aimming.LogicUpdate();
                Aim(_aimInput.ReadValue<Vector2>());
                CheckFlip();
            }
            if (_weaponHolder != null)
            {
               // _weaponHolder.LogicUpdate();
            }
            //if (_dashing) 
            //{
            //    _dashing.LogicUpdate();
            //}
            

        }
        private void FixedUpdate()
        {
            //
            MoveDirection(_directionInput.ReadValue<Vector2>());
        }
        private void Dash(InputAction.CallbackContext context) 
        {
            Vector2 direction = _directionInput.ReadValue<Vector2>();
            //_dashing.UseDash(direction, _dashSpeed, _timeDash);
            //_dashing.PermamentDash(direction,_dashSpeed,_timeDash);
          
        }
        private void MoveDirection(InputAction.CallbackContext context) 
        {
            Vector2 direction = context.ReadValue<Vector2>();
            _movment.SetVelocity(_moveSpeed, direction) ;
        }
        private void MoveDirection(Vector2 direction)
        {
            _movment.SetVelocity(_moveSpeed, direction);
        }
        private void ActiveWeapon(InputAction.CallbackContext context)
        {
            //_weaponHolder.ShoootWeapon();
        }
        private void DeactiveWeapon(InputAction.CallbackContext context)
        {
            //_weaponHolder.CancelWeapon();
        }
        private void Aim(Vector2 mousePosition)
        {
            Vector2 positin = Camera.main.ScreenToWorldPoint(mousePosition);
            _aimming.Aim(positin, _armPivot, _rotationSpeed);
        }
        private void CheckFlip() 
        {
            if (_aimming.FacingDirection == 1)
            {
                _handleWeaponPivot.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _handleWeaponPivot.localRotation = Quaternion.Euler(180, 0, 0);
            }
            _movment.CheckIfShouldFlip(_aimming.FacingDirection);
        }
    }

 }