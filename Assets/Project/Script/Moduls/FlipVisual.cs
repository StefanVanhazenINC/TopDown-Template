using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace TopDown_Template
{
    public class FlipVisual : MonoBehaviour
    {
        #region Variable
        [SerializeField] private Transform[] _flipTransform;
        [SerializeField] private Transform[] _flipTransformX;
        [SerializeField] private SpriteRenderer[] _flipSprite;
        [SerializeField] private Transform _pivot;
        private InputController _controller;
        private float _xPosition;
        #endregion
        #region Getter Setter
        public int FacingDirection { get; private set; }
        #endregion
        #region UnityCallback
        private void Awake()
        {
            FacingDirection = 1;
            _controller = GetComponent<InputController>();

        }
        private void Start()
        {
            _controller.LookEvent.AddListener(SetXPosition);
        }
        private void Update()
        {
            CheckIfShouldFlip();
        }
        #endregion
        #region Set Function
        public void SetXPosition(Vector2 position)
        {
            _xPosition = position.x;
        }
        #endregion
        #region FlipVisual Method
        public void CheckIfShouldFlip()
        {

            if (_xPosition > _pivot.transform.position.x)
            {
                FacingDirection = 1;
                for (int i = 0; i < _flipTransform.Length; i++)
                {
                    _flipTransform[i].localRotation = Quaternion.Euler(0, 0, 0);

                }
                for (int i = 0; i < _flipTransformX.Length; i++)
                {
                    _flipTransformX[i].localRotation = Quaternion.Euler(0, 0, 0);
                }
                for (int i = 0; i < _flipSprite.Length; i++)
                {
                    _flipSprite[i].flipX = false;
                }
            }
            else
            {
                FacingDirection = -1;
                for (int i = 0; i < _flipTransform.Length; i++)
                {
                    _flipTransform[i].localRotation = Quaternion.Euler(180, 0, 0);

                }
                for (int i = 0; i < _flipTransformX.Length; i++)
                {
                    _flipTransformX[i].localRotation = Quaternion.Euler(0, 180, 0);
                }
                for (int i = 0; i < _flipSprite.Length; i++)
                {
                    _flipSprite[i].flipX = true;
                }
            }




        }
        #endregion
    }
}