using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class FakeHeightObject : MonoBehaviour
{


    [Header("FakeHeightSetting - Body")]
    [SerializeField] private Transform _tBody;
    [SerializeField] private Transform _tShadow ;
    [SerializeField] private Transform _groudnPosition ;
    [SerializeField] private UnityEvent _onGroundHit;
    [SerializeField] private bool _isGroundStart;
    [Header("=========================")]
    [Header("Shadow Setting")]
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _multiplayShadow;
    [Header("=========================")]
    [Header("VelocitySetting")]
    [SerializeField] private float _verticalPushVelocity;


    [Header("=========================")]
    [Header("Bounce")]
    [SerializeField] private bool _bounceEnable;
    [SerializeField] private float _divisionBounce = 2;
    
    [Header("=========================")]
    [Header("ParametrVelocity")]
    [SerializeField] private float _gravity = -10;
   


    [SerializeField] private bool _isStop;
    [SerializeField] private bool _isStopHeight;

    private float _verticalVelocity;
    private float _lastVerticalVelocity;
    private Vector2 _defaultSizeShadow;
    [SerializeField] private bool _isGround;

    private Vector2 _groundVelocity;
    private bool _enableGroundVelocity;

    public bool IsStop { get => _isStop; set => _isStop = value; }
    public float Gravity { get => _gravity; set => _gravity = value; }
    public bool IsStopHeight { get => _isStopHeight;
        set 
        {
            _isStopHeight = value;
            if (!value && _tShadow) 
            {
                _isStop = false;
                _verticalVelocity = 0;
            }
        }  
    }

    public UnityEvent OnGroundHit { get => _onGroundHit; set => _onGroundHit = value; }

    //параметр отвечающий за растояние на котором нвчинается уменьшаться основной объект если он слишком высоко 
    //параметр отвечающий за растояние на котором нвчинается уменьшаться теневой объект если основной объект он слишком высоко 
    private void Start()
    {
        _isGround = _isGroundStart;
        if (_tShadow) 
        {
            _defaultSizeShadow = _tShadow.transform.localScale;
        }
    }
    private void FixedUpdate()
    {
        
        if (_isStopHeight && _tShadow)
        {
            float distanceToShadow = _tBody.position.y - _groudnPosition.position.y;
            if (distanceToShadow >= _maxDistance)
            {
                _isStop = true;
            }
        }
        UpdatePosition();
        CheckGroundHit();
        CheckShadow();
    }

    private void CheckShadow()
    {
        if (_tShadow )
        {
            if (!_isGround)
            {
                float distanceToShado = _tBody.position.y - _tShadow.position.y;

                //
                float precent = 1 - distanceToShado / _maxDistance;
                if (precent > 1)
                {
                    precent = 1;
                }
                if (precent<0) 
                {
                    precent = 0.1f;
                }
                _tShadow.transform.localScale = _defaultSizeShadow * (precent * _multiplayShadow);
            }
            else 
            {
                _tShadow.transform.localScale = _defaultSizeShadow;

            }

        }
        

    }

    public void Initialize(float verticalVelocity) 
    {
        _isGround = false;
        //_groundVelocity = groundVelocity;
        _verticalVelocity = verticalVelocity;
        _lastVerticalVelocity = _verticalVelocity;
    }
    public void Initialize(float verticalVelocity,bool enable, Vector3 groundVelocity)
    {
        _isGround = false;
        //_groundVelocity = groundVelocity;
        _verticalVelocity = verticalVelocity;
        _lastVerticalVelocity = _verticalVelocity;
        _groundVelocity = groundVelocity;
        _enableGroundVelocity = enable;
    }
    public void Initialize(bool stop) 
    {
        _isStopHeight =  stop;
        _isGround = false;
        _verticalVelocity = _verticalPushVelocity;
        _lastVerticalVelocity = _verticalVelocity;
    }
    public void Initialize()
    {
        _isGround = false;
        //_groundVelocity = Vector3.forward * _groundPushVelocity;
        _verticalVelocity = _verticalPushVelocity;
        _lastVerticalVelocity = _verticalVelocity;
    }
    [ContextMenu("DropToGround")]
    public void DropToGround() 
    {
        _isGround = false;
        _verticalVelocity = 0;
        _tBody.localPosition = new Vector3(0, _maxDistance);
    }
    private void UpdatePosition()
    {
        if (!_isGround)
        {
            if (!_isStop) 
            {
                //Debug.Log(_verticalVelocity);
                _verticalVelocity += _gravity * Time.deltaTime;
                _tBody.localPosition += new Vector3(0, _verticalVelocity) * Time.deltaTime;

                if (_enableGroundVelocity)
                {
                    transform.localPosition += new Vector3(_groundVelocity.x, _groundVelocity.y) * Time.deltaTime;
                }
            }
        }
    }
    private void CheckGroundHit()
    {
        if (_tBody.position.y < _groudnPosition.position.y && !_isGround)
        {
            _isGround = true;
            GroundHit();
        }
    }
    private void Bounce(float divisionFactor) 
    {
        Initialize(_lastVerticalVelocity / divisionFactor);
    }
    private void GroundHit()
    {
        
        _tBody.localPosition = new Vector3(_tBody.localPosition.x, _groudnPosition.localPosition.y);
        if (_enableGroundVelocity)
        {
            transform.localPosition = new Vector3(_tBody.position.x, _groudnPosition.position.z, _tBody.position.z);
        }
        if (_bounceEnable)
        {
            if (_lastVerticalVelocity > 2.5f)
            {
                Debug.Log(_lastVerticalVelocity > 1.3);
                Bounce(_divisionBounce);
            }
            else
            {
                _onGroundHit.Invoke();
            }
        }
        else 
        {
            _onGroundHit.Invoke();
        }


    }
}
