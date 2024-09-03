using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent[] _startAnimation;
    [SerializeField] private UnityEvent[] _endAnimation;


    public void StartAnimationEvent(int index) 
    {
        _startAnimation[index]?.Invoke();
    }
    public void EndAnimationEvent(int index) 
    {
        _endAnimation[index]?.Invoke();
    }
}
