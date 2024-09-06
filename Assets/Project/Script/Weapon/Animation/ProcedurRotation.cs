using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProcedurRotation : MonoBehaviour
{
    #region Variable
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _duration;
    [SerializeField] private float _offsetAngle;
    [SerializeField] private float _startAngle;
    [SerializeField] private Transform _animationTransform;
    private int _direction = 1;
    private bool _endRotation = true;

    public UnityAction _endAnimationEvent;
    #endregion

    #region Getter Setter
    public float Duration { get => _duration; set => _duration = value; }
    #endregion

    #region ProcedurRotation Method
    private void SwitchDirection()
    {
        _direction *= -1;
    }
    public void StartRotation()
    {
        if (!_endRotation) return;
        SwitchDirection();
        if (_direction == 1)
        {
            _animationTransform.eulerAngles = new Vector3(0, 0, _startAngle);
        }
        else
        {
            _animationTransform.eulerAngles = new Vector3(180, 0, _startAngle);
        }
        StartCoroutine(AnimationProcesed());
    }
    private IEnumerator AnimationProcesed()
    {
        Vector3 startRotation;
        Vector3 endRotation;
        float elapsedTime = 0;
        float delta = 0;
        _endRotation = false;
        float endAngle = _animationTransform.eulerAngles.z + (_offsetAngle);

        if (_direction == 1)
        {

            startRotation = new Vector3(0, 0, _animationTransform.eulerAngles.z); ;
            endRotation = new Vector3(0, 0, endAngle);
        }
        else
        {
            startRotation = new Vector3(0, 180, _animationTransform.eulerAngles.z); ;
            endRotation = new Vector3(0, 180, endAngle);
        }

        while (elapsedTime < _duration)
        {
            delta = elapsedTime / _duration;
            _animationTransform.eulerAngles = Vector3.Lerp(startRotation, endRotation, _curve.Evaluate(delta));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _endRotation = true;
    }
    public void EndRotation()
    {
        SwitchDirection();
        _endRotation = true;
        _endAnimationEvent?.Invoke();
    }
    #endregion

}
