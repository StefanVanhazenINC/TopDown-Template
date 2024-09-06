using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
namespace TopDown_Template
{
    public class KnockBack : MonoBehaviour
    {
        #region Variable
        [Header("Setting")]
        [SerializeField] private float _mass = 5;
        [SerializeField] private float _timeSaveImpulse = 0.5f;
        [SerializeField] private bool _stoppedInKick;

        private bool _updateAction;
        private CharacterController _character;
        private NavMeshGoalToTarget _agent;

        private float _timer = 0;
        private Vector3 _velocity;
        #endregion
        #region Unity Callbakc
        void Start()
        {
            _character = GetComponent<CharacterController>();
            if (!_character)
            {
                _agent = GetComponent<NavMeshGoalToTarget>();
            }
        }
        private void FixedUpdate()
        {
            if (_updateAction)
            {
                AddForce();
            }
        }
        #endregion

        #region KnockBack Method
        public void TakeKnockBack(DamageInfo info)
        {
            if (_agent)
            {
                _velocity = info.Direction * (info.Force / _mass);
                _timer = _timeSaveImpulse;
                _updateAction = true;
                if (_stoppedInKick && _agent.Agent.enabled)
                {
                    _agent.Agent.isStopped = true;
                }
            }
            if (_character)
            {
                _velocity = info.Direction * ((info.Force / 10) / _mass);
                _timer = _timeSaveImpulse;
                _updateAction = true;
                _character.Move(_velocity);
            }
        }
        public void TakeKnockBack(Vector3 direcion)
        {


            if (_agent)
            {

                _velocity = direcion * (2 / _mass);
                _timer = _timeSaveImpulse;
                _updateAction = true;
                if (_stoppedInKick && _agent.enabled)
                {
                    _agent.Agent.isStopped = true;
                }
            }
            if (_character)
            {
                _velocity = direcion * ((2 / 10) / _mass);
                _timer = _timeSaveImpulse;
                _updateAction = true;
                _character.Move(_velocity);
            }
        }
        public void StopAddForce()
        {
            if (_updateAction)
            {
                _updateAction = false;
                _timer = 0;
            }
        }
        private void AddForce()
        {
            Vector3 movment = _velocity * Time.fixedDeltaTime;
            if (_timer < 0)
            {
                _updateAction = false;
                if (_agent)
                {
                    if (_stoppedInKick && _agent.Agent.enabled)
                    {
                        _agent.Agent.isStopped = false;
                    }
                }
                _timer = 0;
            }
            else
            {
                _timer -= Time.fixedDeltaTime;

                if (_agent)
                {
                    _agent.MoveToOffset(_velocity);
                }
                if (_character)
                {
                    _character.Move(_velocity);
                }
            }
        }
        #endregion
    }
}