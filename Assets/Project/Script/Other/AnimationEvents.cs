using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace TopDown_Template
{
    public class AnimationEvents : MonoBehaviour
    {
        #region Variable 
        [SerializeField] private UnityEvent[] _startAnimation;
        [SerializeField] private UnityEvent[] _endAnimation;
        #endregion

        #region AnimationEvents Method
        public void StartAnimationEvent(int index)
        {
            _startAnimation[index]?.Invoke();
        }
        public void EndAnimationEvent(int index)
        {
            _endAnimation[index]?.Invoke();
        }
        #endregion

    }

}