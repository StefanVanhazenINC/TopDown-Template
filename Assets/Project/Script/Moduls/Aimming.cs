using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopDownController
{
    public class Aimming : MonoBehaviour
    {
        [SerializeField] private Transform _armPivot;
        [SerializeField] private Transform _handleArmPivot;
        [SerializeField] private float _rotationSpeed = 10;

        private InputController _controller;
        private float _speedRotation;
        public bool CanAimming { get; set; }
        public int FacingDirection { get; private set; }
        
        public Vector2 AimPosition{ get; set; }
        public Vector2 MousePosition{ get; set; }



        private void Awake()
        {
            CanAimming = true;
            FacingDirection = 1;
            _controller = GetComponent<InputController>();

        }
        private void Start()
        {
            _controller.LookEvent.AddListener(Aim);
        }
        public void Aim(Vector2 target,Transform armPivot,float rotationSpeed) 
        {
            if (CanAimming)
            {
                Vector2 directioMouseLook =(Vector2)armPivot.position - target;
                float angle = Mathf.Atan2(directioMouseLook.y, directioMouseLook.x) * Mathf.Rad2Deg;
                angle+=180;
                CheckIfShouldFlip(target.x, armPivot.position.x);
                Quaternion lerpAngle = Quaternion.Lerp(armPivot.rotation, Quaternion.Euler(0, armPivot.rotation.y, angle), Time.deltaTime * rotationSpeed);
                armPivot.localRotation = lerpAngle;
            }
        }
        public void Aim(Vector2 target)
        {
            MousePosition = target;
           // target = _camera.ScreenToWorldPoint(target);
            AimPosition = target;
            _speedRotation = _rotationSpeed;
        }
        private void ProccesAim(Vector2 target, float rotationSpeed) 
        {
            if (CanAimming)
            {
                Vector2 directioMouseLook = (Vector2)_armPivot.position - target;
                float angle = Mathf.Atan2(directioMouseLook.y, directioMouseLook.x) * Mathf.Rad2Deg;
                angle += 180;
                Quaternion lerpAngle = Quaternion.Lerp(_armPivot.rotation, Quaternion.Euler(0, _armPivot.rotation.y, angle), Time.deltaTime * rotationSpeed);
                _armPivot.localRotation = lerpAngle;
            }
        }
        public void CheckIfShouldFlip(float targetX,float pivotX) 
        {
            if (targetX > pivotX)
            {
                FacingDirection = 1;
                _handleArmPivot.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                FacingDirection = -1;
                _handleArmPivot.localRotation = Quaternion.Euler(180, 0, 0);

            }
        }
        public void Update()
        {
          
            if (CanAimming) 
            {
                ProccesAim(AimPosition, _speedRotation);
            }
        }
      
    }
}