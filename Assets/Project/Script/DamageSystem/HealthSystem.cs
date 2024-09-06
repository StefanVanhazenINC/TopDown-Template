using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TopDown_Template
{
    [System.Serializable]
    public class EventsHelth
    {
        public UnityEvent ChangeHealth;
        public UnityEvent TakeHealEvent;
        public UnityEvent ChangeArmor;

        public UnityEvent TakeDamageEvent;
        public UnityEvent<DamageInfo> TakeDamageInfo;

        public UnityEvent<DamageInfo> DeathDamageInfo;
        public UnityEvent DeathEvent;
    }
    public class HealthSystem : MonoBehaviour, IDamageable
    {
        #region Setting
        [Header("Setting Health")]
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _maxArmor;
        [SerializeField] private bool _isPlayer;
        [Space]

        [Header("Setting Event")]
        [SerializeField] private EventsHelth _events;
        [Space]

        [Header("Setting Invictable")]
        [SerializeField] private bool _isInvictable;
        [SerializeField] private float _frameInvictable;
        #endregion

        #region Private Variable
        private int _health;
        private int _armor;
        private bool _isDead = false;
        private bool _invictableFrame;//временное бесмертие 
        private float _lastTimeDamage = -100;
        #endregion


        #region Getter Setter

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                if (CurrentHealthIsMax)
                {
                    _health = _maxHealth;
                }
                if (_health <= 0)
                {
                    _health = 0;
                }
                _events.ChangeHealth?.Invoke();

            }
        }
        public int Armor
        {
            get => _armor;
            set
            {
                _armor = value;
                if (CurrentHealthIsMax)
                {
                    _armor = _maxArmor;
                }
                if (_armor <= 0)
                {
                    _armor = 0;
                }
                _events.ChangeHealth?.Invoke();

            }

        }
        public bool CurrentHealthIsMax
        {
            get
            {
                if (_health >= _maxHealth)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool CanTakeDamage
        {
            get
            {
                if (!_isDead && !_invictableFrame && (Time.time >= _lastTimeDamage + _frameInvictable))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {

            }
        }
        public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        public bool InvictableFrame
        {
            get => _invictableFrame;
            set
            {
                _invictableFrame = value;
            }
        }
        public bool IsPlayer { get => _isPlayer; }
        public Vector3 Position => transform.position;
        public bool IsDead => _isDead;

        public EventsHelth EventsHelth { get => _events; set => _events = value; }
        public bool IsInvictable { get => _isInvictable; set => _isInvictable = value; }
        #endregion
        #region HealthSystem  Virtual  Method
        public virtual void Awake()
        {
            InitialSetting();

        }
        public virtual void InitialSetting()
        {
            _health = _maxHealth;
            _armor = _maxArmor;
            _events.ChangeHealth?.Invoke();
            _events.ChangeArmor?.Invoke();
        }
        public virtual bool TryTakeArmor(int value)
        {
            if (_armor < _maxArmor)
            {
                TakeArmor(value);
                return true;
            }
            return false;

        }
        public virtual void TakeArmor(int value)
        {
            _armor += value;
            _events.ChangeArmor?.Invoke();
        }
        public virtual bool TryTakeHeal(int value)
        {
            if (!IsDead && !CurrentHealthIsMax)
            {
                TakeHeal(value);
                return true;
            }
            return false;
        }
        public virtual void TakeHeal(int value)
        {
            Health += value;
            _events.ChangeHealth?.Invoke();
            _events.TakeHealEvent?.Invoke();

        }
        public virtual bool TryTakeDamage(DamageInfo info)
        {
            if (CanTakeDamage /*&& info.IsPlayer != _isPlayer*/  && !_isInvictable)
            {
                TakeDamage(info);
                return true;
            }
            return false;
        }
        public virtual void TakeDamage(DamageInfo info)
        {
            _lastTimeDamage = Time.time;
            int t_Damage = info.Damage;
            if (_armor > 0)
            {
                t_Damage -= _armor;
                Armor -= info.Damage;

            }
            if (t_Damage > 0)
            {
                Health -= t_Damage;
            }
            _events.TakeDamageEvent?.Invoke();
            _events.TakeDamageInfo?.Invoke(info);
            DeathHeandler();

        }
        public virtual void DeathHeandler()
        {
            if (_health <= 0)
            {
                Death();
            }
        }
        public virtual void Death()
        {
            _events.DeathEvent?.Invoke();
            _isDead = true;
        }
        #endregion
    }
}