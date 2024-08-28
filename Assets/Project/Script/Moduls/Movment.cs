using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopDownController
{
    [RequireComponent (typeof(Rigidbody2D))]
    public class Movment : MonoBehaviour
    {
        [SerializeField] private Transform[] _needRotation;
        public Rigidbody2D RB { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
        public bool CanSetVelocity { get; set; }

        private Vector2 _workSpace;

        private InputController _controller;

        private void Awake()
        {
            FacingDirection = 1;
            RB = GetComponent<Rigidbody2D>();
            CanSetVelocity = true;
            _controller = GetComponent<InputController>();
        }
        private void Start() 
        {
            _controller.OnMoveEvent.AddListener(SetVelocity);
        }
        public void Update() 
        {
            CurrentVelocity = RB.velocity;
        }

        #region Set Function
        public void SetVelocityZero()
        {
            RB.velocity = Vector2.zero;
            SetFinalVelocity();
        }
      
        public void SetVelocity(float velocity, Vector2 direction)
        {
            _workSpace = direction * velocity;
            
            SetFinalVelocity();
        }
        public void SetVelocity( Vector2 velocity)
        {
            _workSpace = velocity;
          
            SetFinalVelocity();
        }

        private void SetFinalVelocity()
        {
            if (CanSetVelocity)
            {
                RB.velocity = _workSpace;
                CurrentVelocity = _workSpace;
                //CheckIfShouldFlip(Mathf.CeilToInt(CurrentVelocity.x));
            }
        }
        #endregion
        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != FacingDirection)
            {
                Flip();
            }
        }
        public void Flip()
        {
            FacingDirection *= -1;
            for (int i = 0; i < _needRotation.Length; i++)
            {
                _needRotation[i].Rotate(0, 180, 0, 0);
            }
        }
    }
}
