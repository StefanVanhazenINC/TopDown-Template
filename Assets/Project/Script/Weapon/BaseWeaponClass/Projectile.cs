using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown_Template
{
    public class Projectile : MonoBehaviour
    {
        #region Variable
        private int _damage;
        private float _force;
        private float _speed;
        private float _lifeTime;
        private float _sizeProjecTile;
        private float _curruntLifeTime = 0;
        private LayerMask _hitLayer;

        private GameObject _visual;
        public delegate void OnDisableCallback(Projectile projectile);
        public OnDisableCallback DisableCallback;
        #endregion
        #region Unity Callback
        private void Update()
        {
            HandleCollision();
            transform.Translate(transform.right * _speed * Time.deltaTime, Space.World);
            if (_curruntLifeTime < _lifeTime)
            {
                _curruntLifeTime += Time.deltaTime;
            }
            else
            {
                DestoryProjecTile();
            }
        }
        #endregion
        #region Projectile Method
        public void SetProjecTile(int damage, float force, float speed, float lifeTime, float sizeProjecTile, LayerMask hitLayer, GameObject visual = null)
        {
            _damage = damage;
            _force = force;
            _speed = speed;
            _lifeTime = lifeTime;
            _sizeProjecTile = sizeProjecTile;
            _curruntLifeTime = 0;
            _hitLayer = hitLayer;
            if (visual != _visual)
            {
                if (_visual != null)
                {
                    Destroy(_visual);
                }
                if (visual)
                {
                    _visual = Instantiate(visual, transform.position, transform.rotation, transform);
                }
            }
        }
        private void HandleCollision()
        {
            RaycastHit2D t_hit = Physics2D.CircleCast(transform.position, _sizeProjecTile * transform.localScale.x, transform.right, _sizeProjecTile, _hitLayer);
            if (t_hit)
            {
                if (t_hit.transform.GetComponent<IDamageable>() != null)
                {
                    DamageInfo t_dinfo = new DamageInfo(_damage, transform.right, _force);
                    t_hit.transform.GetComponent<IDamageable>().TakeDamage(t_dinfo);
                }
                DestoryProjecTile();
            }
        }
        private void DestoryProjecTile()
        {
            if (gameObject.activeSelf)
            {
                DisableCallback?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}