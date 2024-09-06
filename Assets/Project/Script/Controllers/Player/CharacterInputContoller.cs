using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace TopDown_Template
{
    public class CharacterInputContoller : InputController
    {
        #region Variable
        [Header("CharacterStats")]
        [SerializeField] private float _moveSpeed;

        [Header("Input Setting")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private GameObject _mobileInput;
        [SerializeField] private Vector2 _thresholdGamePad;
        [SerializeField] private TopDownBase _topDownInput;
        private Vector2 _currentDelta = Vector2.zero;
        private Camera _camera;
        private bool _isGamepad = false;
        private InputAction _movment;
        private InputAction _look;
        private InputAction _attack;
        private InputAction _reloading;
        private InputAction _switchWeapon;
        private InputAction _interact;
        private Vector2 _lookInput;
        private Vector2 _moveInput;
        private Vector2 _newAim;
        private Vector2 _newAimScreenToWorld;

        #endregion
        #region Unity Callback
        private void Awake()
        {

            _topDownInput = new TopDownBase();
            _camera = Camera.main;
        }
        private void OnEnable()
        {
            SettingInput();
        }
        public void Update()
        {
            OnLook();
            OnAttack();

            LookEvent?.Invoke(_newAimScreenToWorld);
        }
        private void FixedUpdate()
        {
            PointPositionEvent?.Invoke(_newAim);
        }
        #endregion

        #region CharacterInputContoller Method
        private void SettingInput()
        {
            _movment = _topDownInput.Player.Move;
            _look = _topDownInput.Player.Look;

            _attack = _topDownInput.Player.Attack;
            _reloading = _topDownInput.Player.Reloading;
            _switchWeapon = _topDownInput.Player.SwitchWeapon;
            _interact = _topDownInput.Player.Interact;


            _movment.Enable();
            _look.Enable();

            _attack.Enable();
            _reloading.Enable();
            _switchWeapon.Enable();
            _interact.Enable();

            _interact.started += OnInteract;
            _switchWeapon.started += OnSwitchWeapon;
            _reloading.started += OnReloading;

            _movment.performed += OnMove;
            _movment.started += OnMove;
            _movment.canceled += OnMove;
        }
        public void OnLook()
        {
            _lookInput = _look.ReadValue<Vector2>();
            if (!_isGamepad)
            {
                _newAim = _lookInput;
                _newAimScreenToWorld = _camera.ScreenToWorldPoint(_newAim);
            }
            if (_isGamepad && (_look.ReadValue<Vector2>().magnitude > 0.9f || (_look.ReadValue<Vector2>().magnitude < 0.5f && _moveInput.magnitude > 0.5f)))
            {
                if (_look.ReadValue<Vector2>().magnitude > 0.9f)
                {
                    _currentDelta = _lookInput.normalized;
                }
                else if (_look.ReadValue<Vector2>().magnitude < 0.5f && _moveInput.magnitude > 0.5f)
                {
                    _currentDelta = _moveInput.normalized;
                }

                _currentDelta.x *= _thresholdGamePad.x;
                _currentDelta.y *= _thresholdGamePad.y;
                _newAim = (Vector2)transform.position + _currentDelta;

                _newAimScreenToWorld = _newAim;
                _newAim = _camera.WorldToScreenPoint(_newAim);
            }
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = _movment.ReadValue<Vector2>().normalized * _moveSpeed;
            OnMoveEvent?.Invoke(_moveInput);

        }
        public void OnAttack()
        {
            if (_attack.ReadValue<float>() > 0.5f)
            {
                OnAttackEvent?.Invoke(true);
            }
            else
            {
                OnAttackEvent?.Invoke(false);
            }
        }
        public void OnReloading(InputAction.CallbackContext context)
        {
            OnReloadingWeaponEvent?.Invoke();
        }
        public void OnSwitchWeapon(InputAction.CallbackContext context)
        {
            OnChangeWeaponEvent?.Invoke();
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            OnInteractionEvent?.Invoke();
        }
        public void ChangeDeviceInput(bool assist)
        {
            _isGamepad = assist;
            SwitchDeviceInput?.Invoke(_isGamepad);
        }
        #endregion
    }
}