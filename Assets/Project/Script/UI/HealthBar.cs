using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace TopDown_Template
{
    public class HealthBar : MonoBehaviour
    {
        #region Variable
        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private Image _healthBar;

        [SerializeField] private UnityEvent _onHealthBarChange;
        #endregion
        #region UnityCallback
        public void Start()
        {
            Consctuct(_healthSystem);
        }
        #endregion

        #region  HealthBar Method
        private void Consctuct(HealthSystem healthPlayer)
        {
            healthPlayer.EventsHelth.ChangeHealth.AddListener(ChangeHealthBar);
        }
        private void ChangeHealthBar()
        {
            _healthBar.fillAmount = (float)_healthSystem.Health / (float)_healthSystem.MaxHealth;
            _onHealthBarChange?.Invoke();
        }
        #endregion
    }
}
