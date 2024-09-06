using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TopDownController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace TopDown_Template
{
    public class CrossHair : MonoBehaviour
    {
        #region Variable
        [SerializeField] private CharacterWeaponHolder _weaponHolder;
        [SerializeField] private Transform _crossHairTransfrom;
        [SerializeField] private RectTransform _parentCanvas;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private float _sizeCrossHair = 10;
        [SerializeField] private float _punchForce;
        [SerializeField] private Image[] _crossImage;

        [SerializeField] private Color _notReady;

        [SerializeField] private float _speedRotation;


        private float _speedReturn;
        private RectTransform _rectTransform;
        private Vector2 _defaultSize;
        private Tween _tween;
        private Tween _tweenRot;
        private WeaponRange _currentWeapon;
        private InputController _inputController;
        #endregion

        #region Unity Callback
        private void Start()
        {
            Construct();
        }
        #endregion

        #region CrossHair Method
        private void Construct()
        {
            Cursor.visible = false;
            _inputController = GetComponent<InputController>();
            _inputController.PointPositionEvent.AddListener(UpdatePosition);

            _weaponHolder.SwitchWeaponEvent.AddListener(SetSize);
            _weaponHolder.UseWeaponEvent.AddListener(FireGun);
            _weaponHolder.ReloadingWeaponEvent.AddListener(Reloading);
            _rectTransform = _crossHairTransfrom.GetComponent<RectTransform>();
            SetSize();
        }
        private void UpdatePosition(Vector2 mousePosition)
        {

            Vector2 t_pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas, mousePosition, _uiCamera, out t_pos);
            _rectTransform.anchoredPosition = t_pos;
        }
        private void SetSize()
        {
            if (_weaponHolder.CurrentWeapon as WeaponRange)
            {
                if (_tween != null)
                {
                    _tween.Pause().Kill();
                }
                if (_tweenRot != null)
                {
                    _tweenRot.Pause().Kill();
                }

                _rectTransform.sizeDelta = new Vector2(_sizeCrossHair, _sizeCrossHair);
                _defaultSize = _rectTransform.sizeDelta;
                ColorChage();
                _rectTransform.eulerAngles = Vector3.zero;
                _currentWeapon = (WeaponRange)_weaponHolder.CurrentWeapon;
                _speedReturn = _currentWeapon.FireRate;
            }

        }
        private void ColorChage()
        {
            for (int i = 0; i < _crossImage.Length; i++)
            {
                _crossImage[i].color = Color.white;
            }
        }
        public void FireGun()
        {
            for (int i = 0; i < _crossImage.Length; i++)
            {
                _crossImage[i].color = _notReady;
            }
            if (_tween != null)
            {
                _tween.Pause().Kill();
            }

            _rectTransform.sizeDelta = _defaultSize * _punchForce;
            _tween = _rectTransform.DOSizeDelta(_defaultSize, _speedReturn).OnComplete(ColorChage);

        }
        private void Reloading()
        {
            if (_tween != null)
            {
                _tween.Pause().Kill();
            }
            if (_tweenRot != null)
            {
                _tweenRot.Pause().Kill();
            }
            _rectTransform.sizeDelta = _defaultSize;

            for (int i = 0; i < _crossImage.Length; i++)
            {
                _crossImage[i].color = _notReady;
            }


            _tweenRot = _rectTransform.DORotate(new Vector3(0, 0, 360), _currentWeapon.TimeToReloading, RotateMode.FastBeyond360).OnComplete(ColorChage);


        }
        #endregion
    }
}
