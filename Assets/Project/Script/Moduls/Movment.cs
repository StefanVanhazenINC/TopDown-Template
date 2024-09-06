using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopDown_Template
{
    [RequireComponent (typeof(Rigidbody2D))]
    public class Movment : MonoBehaviour
    {
        #region Variable 
        private Vector2 _workSpace;
        private InputController _controller;
        #endregion

        #region Getter Setter

        public Rigidbody2D RB { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
        public bool CanSetVelocity { get; set; }

        #endregion

        #region Unity Callback
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
        #endregion

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
            }
        }
        #endregion
       
    }
}
