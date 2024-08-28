using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NavMeshGoalToTarget : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private NavMeshObstacle _obstacle;
    
    [SerializeField] private float _moveThreshold;//минимальная сокрость после которой засчитывается остановка
    [SerializeField] private float _timeToStationary;

    private float _lastTimeStop;
    private bool _isMoving;
    private bool _isStopped;
    private bool _isFirstStopped;

    public NavMeshAgent Agent { get => _agent;  }



    private void Update()
    {
        CheckStoppedAgent();
    }
    public void MoveToOffset(Vector2 offset)
    {
        if (_obstacle)
        {
            StartCoroutine(DelayAgentEnable());
        }
        _isStopped = false;
        _isFirstStopped = false;
        _isMoving = true;
        if (_agent.enabled)
        {
            _agent.Move(offset);
        }
    }
    
    public void Move(Vector2 targetPosition) 
    {
        
        if (Vector2.Distance(targetPosition, transform.position) >= _agent.stoppingDistance)
        {
            if (_obstacle)
            {
                StartCoroutine(DelayAgentEnable());
            }
            _isStopped = false;
            _isFirstStopped = false;
            _isMoving = true;
            if (_agent.enabled)
            {
                _agent.SetDestination(targetPosition);
            }
        }
       
    }
    private void CheckStoppedAgent() 
    {
        if (_agent.velocity.magnitude <= _moveThreshold && _isStopped == false)
        {
            if (!_isStopped)
            {
                if (_isMoving == true && _isFirstStopped == false)
                {
                    _isFirstStopped = true;
                    _lastTimeStop = Time.time;
                }
                if (Time.time >= _lastTimeStop + _timeToStationary)
                {
                    if (_obstacle)
                    {
                        _agent.enabled = false;
                        _obstacle.enabled = true;
                    }
                    _isStopped = true;
                    _isMoving = false;
                }
            }
        }
    }
    
    private IEnumerator DelayAgentEnable()
    {
        _obstacle.enabled = false;
        yield return null;
        _agent.enabled = true;
    }
}
