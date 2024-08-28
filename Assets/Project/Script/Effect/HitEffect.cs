using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class HitEffect : MonoBehaviour
{
    [Header("Setting Flash")]
    [SerializeField] private float _duration;
    [SerializeField] private Color _colorFlash;


    [Header("==========================")]
    [Header("Setting Shape")]
    [SerializeField] private bool _shapeOn;
    [SerializeField] private bool _flashOn = true;
    [SerializeField] private float _strenght;
    [SerializeField] private int _vibration;

    [Header("==========================")]
    [Header("Component")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private UnityEvent _endFlash;

    private bool _isFlash;
    private Vector3 _defautSize;
    private Color _defaultColor;

    private Tween _tweenFlash;
    private Tween _tweenShake;
    //добавить сюда параметр , для постоянного мелькания или просто раз моргнуть 
    private void OnDestroy()
    {
        Destroy();
    }
    private void Start()
    {
        _defaultColor = _spriteRender.color;
        _defautSize = _spriteRender.transform.localScale;
    }
    public void StartFlash()
    {
        if (_isFlash) return;
        _isFlash = true;
        if (_shapeOn)
        {
            _tweenShake = _spriteRender.transform.DOShakeScale(_duration, _strenght, _vibration);
        }
        if (_flashOn && _spriteRender)
        {
            _tweenFlash = _spriteRender.DOColor(_colorFlash, _duration).OnComplete(ReturnDefault);
        }
       
    }

    public void ReturnDefault()
    {
        if (_shapeOn)
        {
            _tweenShake.Pause();
            _spriteRender.transform.localScale = _defautSize ;
        }
        if (_flashOn && _spriteRender)
        {
            _tweenFlash = _spriteRender.DOColor(_defaultColor, _duration).OnComplete(_endFlash.Invoke);
        }
       
        _isFlash = false;
    }

    public void Destroy()
    {
        _tweenFlash.Complete();
        _tweenFlash.Complete();
        _tweenFlash.Pause();
        _tweenFlash.Kill();


        _tweenShake.Complete();
        _tweenShake.Complete();
        _tweenShake.Pause();
        _tweenShake.Kill();
    }
}
