using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopDown_Template
{
    public class AspectrRationUtility : MonoBehaviour
    {
        #region Variable
        [SerializeField] private float _targetWidth = 16f;
        [SerializeField] private float _targetHeight = 9f;


        private float _lastHeight = 0;
        private float _lastWidth = 0;
        private Camera _camera;
        #endregion

        #region Unity Callback
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }
        public void Update()
        {
            AspectRation();
        }
        #endregion

        #region AspectrRationUtility Method
        private void AspectRation()
        {

            float targetaspect = _targetWidth / _targetHeight;

            float windowaspect = (float)Screen.width / (float)Screen.height;

            float scaleheight = windowaspect / targetaspect;



            if (scaleheight < 1.0f && (Screen.width != _lastHeight || Screen.height != _lastWidth))
            {
                _lastHeight = Screen.height;
                _lastWidth = Screen.width;

                Rect rect = _camera.rect;

                rect.width = 1.0f;
                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;

                _camera.rect = rect;
            }
            else
            {
                float scalewidth = 1.0f / scaleheight;

                Rect rect = _camera.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                _camera.rect = rect;
            }
        }
        #endregion
    }
}