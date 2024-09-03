using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Effecter : MonoBehaviour
{

    [SerializeField] private bool _enableFlash;
    [SerializeField] private bool _enableSqueeze;
    [SerializeField] private bool _enableShake;
    [SerializeField] private bool _enableBlob;

    [Header("Setting Flash")]
    [SerializeField] private float _durationFlash;
    [SerializeField] private Color _colorFlash;


    [Header("==========================")]
    [Header("Setting Squeeze")]
    [SerializeField] private float _xSizeTarget;
    [SerializeField] private float _ySizeTarget;
    [SerializeField] private float _durationSqueeze;//половина на сжатие и на возврат
    [SerializeField,Range(0.1f,0.9f)] private float _rationInToOut;
    private float _durationInSqueeze;
    private float _durationOutSqueeze;

    [Header("==========================")]
    [Header("Setting Shake")]
    [SerializeField] private float _durationShake;
    [SerializeField] private float _strenghtShake;
    [SerializeField] private int   _vibration;

    [Header("==========================")]
    [Header("Setting Blob")]
    [SerializeField] private float _durationBlob;
    [SerializeField] private Vector3 _scaleBlob;


    [Header("==========================")]
    [Header("Component")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private UnityEvent _endFlash;

    private bool _isSqueeze;
    private bool _isFlash;
    private bool _isShake;
    private bool _isBlob;
    private Vector3 _defautSize;
    private Vector3 _defautSizeSqueeze;
    private Color _defaultColor;

    private Tween _tweenFlash;
    private Tween _tweenSqueeze;
    private Tween _tweenBlob;
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
        if (_spriteRender) 
        {
            _defautSizeSqueeze = _spriteRender.transform.localScale;
        }
        if (_durationSqueeze > 0 ) 
        {
            _durationInSqueeze = _durationSqueeze / 2;
            _durationOutSqueeze = _durationSqueeze / 2;
        }
    }

    public void StartSqueeze() 
    {
        if (_isSqueeze) return;
        _isSqueeze = true;
        Sequence SqueezeSequency = DOTween.Sequence();
        SqueezeSequency.Append(_spriteRender.transform.DOScale(new Vector2(_xSizeTarget, _ySizeTarget), _durationInSqueeze));
        SqueezeSequency.Append(_spriteRender.transform.DOScale(new Vector2(_defautSizeSqueeze.x, _defautSizeSqueeze.y), _durationOutSqueeze)).OnComplete(() => _isSqueeze = false); ;
        //_tweenSqueeze = _spriteRender.transform.DOScale(new Vector2(_xSizeTarget, _ySizeTarget), _durationInSqueeze).OnComplete(EndSqueeze);

    }
   
    public void StartShake() 
    {
        if (_isShake) return;
        _isShake = true;
        _tweenShake = _spriteRender.transform.DOShakeScale(_durationShake, _strenghtShake, _vibration).OnComplete(ReturnToDefaultShake);
    }
    public void StartFlash() 
    {
        if (_isFlash) return;
        _isFlash = true;
        _tweenFlash = _spriteRender.DOColor(_colorFlash, _durationFlash).OnComplete(ReturnToDefaultFlash);


    }
    public void StartBlob() 
    {
        if (_isBlob) return;
        _isBlob = true;
        Sequence blobSequency = DOTween.Sequence();
        blobSequency.Append(_spriteRender.transform.DOScale(_scaleBlob, _durationBlob)); 
        blobSequency.Append(_spriteRender.transform.DOScale(Vector3.one, _durationBlob)).OnComplete(() => _isBlob=false); 
        //_tweenBlob = _spriteRender.transform.DOScale(_scaleBlob,_durationBlob);
    }
    public void ReturnToDefaultShake() 
    {
        _tweenShake.Pause();
        _spriteRender.transform.localScale = _defautSize;
        _isShake = false;
    }
    public void ReturnToDefaultFlash() 
    {
        _tweenFlash = _spriteRender.DOColor(_defaultColor, _durationFlash).OnComplete(_endFlash.Invoke);
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


#if UNITY_EDITOR
    [System.Serializable]
    [CustomEditor(typeof(Effecter))]
    public class EffecterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Effecter TargetScript = target as Effecter;


            EditorGUILayout.PropertyField(serializedObject.FindProperty("_spriteRender"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_endFlash"));
            

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_enableFlash"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_enableSqueeze"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_enableShake"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_enableBlob"));

      


            if (TargetScript._enableFlash)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_durationFlash"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_colorFlash"));
            }
            if (TargetScript._enableSqueeze) 
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_xSizeTarget"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_ySizeTarget"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_durationSqueeze"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_rationInToOut"));
            }
            if (TargetScript._enableShake)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_durationShake"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_strenghtShake"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_vibration"));
            }
            if (TargetScript._enableBlob)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_durationBlob"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_scaleBlob"));
            }
            serializedObject.ApplyModifiedProperties();
        }

    }
#endif
}
