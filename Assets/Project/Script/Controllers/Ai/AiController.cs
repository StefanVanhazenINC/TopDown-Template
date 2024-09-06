
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TopDown_Template 
{ 
    public class AiController : InputController
    {
        #region Variable

        [SerializeField] protected HealthSystem _health;
        [SerializeField] protected Transform _armPivot;


        [Header("Attack Setting")]
        [SerializeField] protected float _radiusAttack = 3;
        [SerializeField] protected float _attackDelay = 0.5f;//между атаками 
        [SerializeField] protected float _durationEnemyInFront = 0.25f;//сколько цель должна стоять перед врагом 
        [SerializeField] protected float _angleAttack = 30;//атака осуществляется только если цель в определеноом угле 

        [Header("Sensor Setting")]
        [SerializeField] protected float _radiusSearch = 3;
        [SerializeField] protected LayerMask _obstacleLayer;
        [SerializeField] protected LayerMask _targetLayer;


        protected Vector2 _originWorkSpace;
        protected Vector2 _destWorkSpace;
        protected Vector2 _directionWorkSpace;
        protected float _lastAttack = 0;
        protected float _timerAttack = 0;
        protected float _deltaAngleWorkSpace;

        protected HealthSystem _target = null;
        protected Collider2D[] _overlapCheck = new Collider2D[10];

        public delegate void OnDisableCallback(AiController instance);
        public OnDisableCallback Disable;
        #endregion

        #region Untiy Callback
        protected virtual void FixedUpdate()
        {
            SearchTarget();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radiusAttack);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radiusSearch);
        }
        private void OnDisable()
        {
            Disable?.Invoke(this);
        }
        #endregion

        #region  AiController Method
        public virtual void Spawn()
        {
            _health.InitialSetting();
        }
        public void SearchTarget()
        {
            if (_target != null)
            {
                if (_target.IsDead || !_target.gameObject.activeSelf)
                {
                    _target = null;
                }
            }
            if (_target == null)
            {
                _overlapCheck = Physics2D.OverlapCircleAll(transform.position, _radiusSearch, _targetLayer);

                for (int i = 0; i < _overlapCheck.Length; i++)
                {
                    if (_overlapCheck[i] != null)
                    {
                        if (CheckLine(_overlapCheck[i].transform.position))
                        {
                            HealthSystem t_target = _overlapCheck[i].GetComponent<HealthSystem>();
                            if (t_target)
                            {
                                if (!t_target.IsDead)
                                {
                                    _target = t_target;
                                    break;
                                }
                            }

                        }
                    }
                }
            }
        }
        public bool CheckLine(Vector3 targetPosition)
        {

            _originWorkSpace = transform.position;
            _destWorkSpace = targetPosition;

            _directionWorkSpace = _destWorkSpace - _originWorkSpace;

            _destWorkSpace.y = _originWorkSpace.y;
            if (Physics.Linecast(_originWorkSpace, _destWorkSpace, _obstacleLayer))
            {
                Debug.DrawLine(_originWorkSpace, _destWorkSpace, Color.green, 0.4f);
                return false;
            }
            return true;

        }
        public bool CheckAngle(Vector3 targetPosition, float angle)
        {
            _originWorkSpace = transform.position;
            _destWorkSpace = targetPosition;

            _directionWorkSpace = (_destWorkSpace - _originWorkSpace).normalized;

            _deltaAngleWorkSpace = Vector2.Angle(_directionWorkSpace, _armPivot.forward);
            if (_deltaAngleWorkSpace > angle)
            {
                Debug.DrawLine(_originWorkSpace, _destWorkSpace, Color.blue, 0.4f);
                return false;
            }
            return true;
        }

        #endregion 

    }
}





