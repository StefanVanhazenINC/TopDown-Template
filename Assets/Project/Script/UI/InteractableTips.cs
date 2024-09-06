using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TopDown_Template
{
    public class InteractableTips : MonoBehaviour
    {
        #region Variable
        [SerializeField] private CharacterInteractable _characterInteractable;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private RectTransform _tip;
        #endregion
        #region UnityCallback
        private void Start()
        {
            Construct(_characterInteractable);
        }
        #endregion
        #region  InteractableTips Method
        private void Construct(CharacterInteractable characterInteractable)
        {
            _characterInteractable = characterInteractable;
            _characterInteractable.OnFindObject.AddListener(UpdatePositionTips);
        }
        public void UpdatePositionTips(bool notNull, Vector2 position)
        {
            if (notNull)
            {
                if (!_tip.gameObject.activeSelf)
                {
                    _tip.gameObject.SetActive(true);
                }


                _tip.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
            }
            else
            {
                if (_tip.gameObject.activeSelf)
                {
                    _tip.gameObject.SetActive(false);
                }
            }
        }
        #endregion
    }
}