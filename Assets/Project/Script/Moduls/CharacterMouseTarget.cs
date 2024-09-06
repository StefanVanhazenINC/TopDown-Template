using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace TopDown_Template {
    public class CharacterMouseTarget : MonoBehaviour
    {
        #region Variable
        [SerializeField] private Transform _target;
        [SerializeField] private float _threshold;

        private Vector2 _mousePosition;
        private Vector3 _workSpaceVec3;
        private InputController _controller;

        private Camera _camera;
        #endregion
        #region Unity Callback
        private void Awake()
        {
            _controller = GetComponent<InputController>();
            _camera = Camera.main;

        }
        private void Start()
        {
            _controller.LookEvent.AddListener(ReadMousePosition);
        }
        private void FixedUpdate()
        {
            MovePosition();
        }
        #endregion
        #region CharacterMouseTarget  Method
        private void ReadMousePosition(Vector2 position)
        {
            _mousePosition = position;
        }
        private void MovePosition()
        {
            _workSpaceVec3 = ((Vector2)transform.position + _mousePosition) / 2f;

            _workSpaceVec3.x = Mathf.Clamp(_workSpaceVec3.x, -_threshold + transform.position.x, _threshold + transform.position.x);
            _workSpaceVec3.y = Mathf.Clamp(_workSpaceVec3.y, -_threshold + transform.position.y, _threshold + transform.position.y);

            _target.position = _workSpaceVec3;
        }
        #endregion
    }

}
