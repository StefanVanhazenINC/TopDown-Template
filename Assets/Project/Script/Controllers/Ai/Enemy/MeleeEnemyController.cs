using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
namespace TopDown_Template
{
    public class MeleeEnemyController : AiController
    {
        private NavMeshAgent _agent;
        private Vector2 _lookDirection;

        public override void Spawn()
        {
            base.Spawn();
            _agent = GetComponent<NavMeshAgent>();
            _lastAttack = -100;
        }
        private void Start()
        {
            Spawn();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (_target)
            {
                OnMoveEvent?.Invoke(_target.transform.position);
                _lookDirection = _target.transform.position;
            }
            else
            {
                _lookDirection = _agent.desiredVelocity + transform.position;
            }
            LookEvent?.Invoke(_lookDirection);
            Attack();
        }
        public void Attack()
        {
            if (_target != null)
            {
                if (Time.time >= _lastAttack + _attackDelay)
                {
                    if (Vector2.Distance(_target.transform.position, transform.position) <= _radiusAttack)
                    {
                        if (CheckAngle(_target.transform.position, _angleAttack))
                        {
                            _timerAttack += Time.deltaTime;
                            if (_timerAttack >= _durationEnemyInFront)
                            {
                                _lastAttack = Time.time;
                                _timerAttack = 0;
                                OnAttackEvent?.Invoke(true);
                                return;
                            }
                        }
                        else
                        {
                            _timerAttack = 0;
                        }
                    }
                }
            }
            OnAttackEvent?.Invoke(false);

        }
    }
}