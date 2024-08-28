using System.Collections;
using System.Collections.Generic;
using TopDownController;
using Unity.VisualScripting;
using UnityEngine;

public class AiController : MonoBehaviour
{
    [SerializeField] private NavMeshGoalToTarget _navMesh;
    [SerializeField] private HealthSystem _enemyControll;


    [Header("Attack Setting")]
    [SerializeField] private float _radiusAttack = 3;
    [SerializeField] private float _radiusSearch = 3;
    [SerializeField] private float _microDelayAttack = 0.1f;
    [SerializeField] private float _attackDelay = 0.5f;//между атаками 
    [SerializeField] private float _durationEnemyInFront = 0.25f;//сколько цель должна стоять перед врагом 
    [SerializeField] private float _speedRotation = 50;
    [SerializeField] private float _angleAttack = 30;//атака осуществляется только если цель в определеноом угле 
    [SerializeField] private int _damageValue = 1;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private Transform _armPivot;
    [SerializeField] private Transform _handleWeaponPivot;
    [SerializeField] private Animator _animator;
    
    private HealthSystem _target =null;

    private Collider2D[] _overlapCheck = new Collider2D[10];
    public HealthSystem EnemyControll { get => _enemyControll;}

    public void Spawn() 
    {
        //восстановка здоровья 
        _enemyControll.InitialSetting();
        // задавание цели 
        FacingDirection = 1;
    }

    private void Update()
    {
        SearchTarget();
        if (_target)
        {
            _navMesh.Move(_target.transform.position);
        }
        RotationToTarget();
        CheckFlip();
        Attack();
    }
    private void RotationToTarget()//поворот до цели 
    {
        if (_target)
        {
            Vector2 directioMouseLook = (Vector2)_armPivot.position - (Vector2)_target.transform.position;
            float angle = Mathf.Atan2(directioMouseLook.y, directioMouseLook.x) * Mathf.Rad2Deg;
            angle += 180;
            CheckIfShouldFlip(_target.transform.position.x, _armPivot.position.x);
            Quaternion lerpAngle = Quaternion.Lerp(_armPivot.rotation, Quaternion.Euler(0, _armPivot.rotation.y, angle), Time.deltaTime * _speedRotation);
            _armPivot.localRotation = lerpAngle;
        }
        else
        {
            Vector2 directioMouseLook = (Vector2)_armPivot.position - (Vector2)(_navMesh.Agent.desiredVelocity + transform.position);
            float angle = Mathf.Atan2(directioMouseLook.y, directioMouseLook.x) * Mathf.Rad2Deg;
            angle += 180;
            Vector2 temp = (Vector2)(_navMesh.Agent.desiredVelocity + transform.position);
            CheckIfShouldFlip(temp.x, _armPivot.position.x);
            Quaternion lerpAngle = Quaternion.Lerp(_armPivot.rotation, Quaternion.Euler(0, _armPivot.rotation.y, angle), Time.deltaTime * _speedRotation);
            _armPivot.localRotation = lerpAngle;
        }

      
        //поворачивать в сторону цели , если цели нет, то в сторону движения 
    }
    private float _lastAttack=0;
    private float _timerAttack = 0;
    private void Attack() //атака, если прошел кулдавн на атаку 
    {
        if (_target!=null) 
        {
            if (Time.time>=_lastAttack+_attackDelay)
            {
                if (Vector2.Distance(_target.transform.position,transform.position) <= _radiusAttack)
                {
                    if (CheckAngle(_target.transform.position, _angleAttack))
                    {
                        _timerAttack += Time.deltaTime;
                        if (_timerAttack >= _durationEnemyInFront)
                        {
                            _lastAttack = Time.time;
                            _timerAttack = 0;
                            StartCoroutine(AttackCo());
                        }
                    }
                    else
                    {
                        _timerAttack = 0;
                    }
                }
            }
        }
    }
    private IEnumerator AttackCo() 
    {
        //начало анимации 
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(_microDelayAttack);
        _target.TryTakeDamage(new DamageInfo(_damageValue, _armPivot.right, 0));
    }
    private void SearchTarget()//поиск ближайшей цели  
    {
        //предыдущая цель либо мертва либо выключена 
        if (_target != null)
        {
            if (_target.IsDead || !_target.gameObject.activeSelf)
            {
                _target = null;
            }
        }
        if (_target == null)
        {
            //постоянный поиск цели , а  точнее списка 
            _overlapCheck = Physics2D.OverlapCircleAll(transform.position, _radiusSearch, _targetLayer);

            //проверка списка 
            for (int i = 0; i < _overlapCheck.Length; i++)
            {
                if (_overlapCheck[i] != null)
                {
                    //проверять список на доступность и преграждения с помощью Raycast 
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

    private Vector2 _originWorkSpace;
    private Vector2 _destWorkSpace;
    private Vector2 _directionWorkSpace;
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
    public int FacingDirection { get; private set; }
    public void CheckIfShouldFlip(float targetX, float pivotX)
    {
        if (targetX > pivotX)
        {
            FacingDirection = 1;
        }
        else
        {
            FacingDirection = -1;
        }
    }
    private void CheckFlip()
    {
        if (FacingDirection == 1)
        {
            _handleWeaponPivot.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _handleWeaponPivot.localRotation = Quaternion.Euler(180, 0, 0);
        }
    }

    private float _deltaAngleWorkSpace;
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
    public delegate void OnDisableCallback(AiController instance);
    public OnDisableCallback Disable;
    private void OnDisable()
    {
        Disable?.Invoke(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radiusAttack);
    }
}
