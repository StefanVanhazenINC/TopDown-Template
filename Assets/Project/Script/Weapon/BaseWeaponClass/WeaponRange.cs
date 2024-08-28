using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
    using UnityEditor;
#endif
public enum WeaponType
{
    Auto,
    Semi,
    Burst
}
[RequireComponent(typeof(BulletSpawner))]
public class WeaponRange : Weapon
{
    #region Variable
    [Header("Weapon Range Data - Projectile ")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _force = 1;
    [SerializeField] private int _projectileInShoot = 1;

    [SerializeField] private bool _randomSpeed;
    [SerializeField] private float _speedProjectile = 10;
    [SerializeField] private float _minSpeedProjectile = 10 ;
    [SerializeField] private float _maxSpeedProjectile = 15 ;

    [SerializeField] private bool _randomLifeTime;
    [SerializeField] private float _lifeTimeProjectile  =  5;
    [SerializeField] private float _minLifeTimeProjectile =  5;
    [SerializeField] private float _maxLifeTimeProjectile =  10;
    [SerializeField] private Transform[] _shootDir;

    [Header("Weapon Range Data - Fire Setting ")]
    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private WeaponType _weaponType;


    [Header("Weapon Range Data - Ammo Data")]
    [SerializeField] private float _timeToReloading = 1;
    [SerializeField] private bool _infinityAmmo = true;
    [SerializeField] private int _maxAmmoValue = 100;
    [SerializeField] private bool _infinityMag;
    [SerializeField] private int _magSize = 7;


    [Header("Component")]
    [SerializeField] private BulletSpawner _bulletSpawner;
    [SerializeField] private Animator _animtor;

    #endregion

    #region Private Variable
    private int _currentMaxAmmo;
    private int _currentMagSize;
    private float _lastShootTime = -100f;
    private IEnumerator _reloadingProcess;
    private Vector2 _workSpace;
    #endregion

    #region Getter Setter
    private bool _isReadyShoot;
    public override bool WeaponReady
    {
        get
        {
            if (Time.time > _lastShootTime + _fireRate && _isReadyShoot)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public override Transform BaseShootDir => _shootDir[0];
    public override bool InfinityAmmo { get => _infinityAmmo; }
    public float FireRate { get => _fireRate; set => _fireRate = value; }
    public float TimeToReloading { get => _timeToReloading; set => _timeToReloading = value; }
 
    #endregion


    public UnityEvent<Transform> _shootEvent;


    #region Unity CallBack

    private void OnValidate()
    {
        _bulletSpawner ??= GetComponent<BulletSpawner>();
    }
    private void OnDisable()
    {
        if (_reloadingProcess != null)
        {
            StopCoroutine(_reloadingProcess);
        }
    }
    private void OnEnable()
    {
        if (_isReloading) 
        {
            _isReloading = false;
            TryReloading();
        }
    }
    #endregion
    #region Weapon Function
    public override void SetupWeapon(WeaponHolder weaponHolder)
    {
        base.SetupWeapon(weaponHolder);
        _isReadyShoot = true;
        _currentMaxAmmo = _maxAmmoValue ;
        _currentMagSize = _magSize ;
    }
    public override void SetupWeapon(WeaponHolder weaponHolder, bool fullMag)
    {
        base.SetupWeapon(weaponHolder, fullMag);
        _isReadyShoot = true;
    }
    public override void UseWeapon()
    {
        if (!isActiveAndEnabled) return;
        if (_currentMagSize <= 0 && !_infinityMag && !_isReloading)
        {
            _isReadyShoot = false;
            TryReloading();
        }
        if (Time.time > _lastShootTime + _fireRate  && _isReadyShoot)
        {
            _currentMagSize--;
            UseWeaponEvent?.Invoke();
            _weaponHolder.UseWeaponEvent?.Invoke();

            _lastShootTime = Time.time;
            if (_weaponType == WeaponType.Semi)
            {
                _isReadyShoot = false;
            }
            for (int i = 0; i < _shootDir.Length; i++)
            {
                for (int j = 0; j < _projectileInShoot; j++)
                {
                    _shootEvent?.Invoke(_shootDir[i]);
                    _workSpace = new Vector3(_shootDir[i].position.x, _shootDir[i].position.y, _shootDir[i].position.z);
                    if (_bulletSpawner)
                    {
                        float t_speed = _randomSpeed ? Random.Range(_minSpeedProjectile, _maxSpeedProjectile) : _speedProjectile;
                        float t_liteTime = _randomSpeed ? Random.Range(_minLifeTimeProjectile, _maxLifeTimeProjectile) : _lifeTimeProjectile;
                        _bulletSpawner.SpawnBullet(_workSpace, _shootDir[i].rotation, _damage, _force, t_speed, t_liteTime);
                    }
                }
                
            }

        }

    }
    public override void CancelUseWeapon()
    {
        if ((_currentMagSize > 0 && !_isReloading) || _infinityMag)
        {
            _isReadyShoot = true;
        }
    }
    public override void TryReloading()
    {
        if (!_isReloading && _currentMagSize < _magSize  && _currentMaxAmmo>0)
        {
            _isReadyShoot = false;
            _isReloading = true;
            _reloadingProcess = (ReloadingProcces());
            StartCoroutine(_reloadingProcess);
        }
    }
    private IEnumerator ReloadingProcces()
    {
        _weaponHolder.ReloadingWeaponEvent?.Invoke();
        _weaponHolder.StartReloadingWeaponEvent?.Invoke();
        ReloadingWeaponEvent?.Invoke(_timeToReloading);
        yield return new WaitForSeconds(_timeToReloading );
        QuickReloading();
       
    }
    public override void QuickReloading() 
    {
        int mag;
        if (_magSize <= _currentMaxAmmo)
        {
           
            mag = _magSize - _currentMagSize ;
            if (!_infinityAmmo)
            {
                _currentMaxAmmo -= mag;
            }
            mag = _magSize;
        }
        else
        {
            mag = _currentMaxAmmo;
            _currentMaxAmmo = 0;
        }

        _currentMagSize = mag;
        _isReadyShoot = true;
        _isReloading = false;
        _weaponHolder.EndReloadingWeaponEvent?.Invoke();
        EndReloadingWeaponEvent?.Invoke();
    }
    public override void SwitchWeapon() 
    {
        _animtor.SetTrigger("SetWeapon");
        _isReloading = false;
        CancelUseWeapon();
        SwitchWeaponEvent?.Invoke();
        if (_reloadingProcess != null)
        {
            StopCoroutine(_reloadingProcess);
        }
    }
    public override void SetStockAndMagAmmo((int, int) ammo)
    {
        _currentMaxAmmo = ammo.Item1;
        _currentMagSize = ammo.Item2;
    }
    public override void RestoreAmmo(int precent) 
    {
        int tempAmmo = (_maxAmmoValue) / precent;

        _currentMaxAmmo += tempAmmo;
        if (_currentMaxAmmo >= _maxAmmoValue )
        {
            _currentMaxAmmo= _maxAmmoValue ;
        }
        _weaponHolder.RestoreAmmoEvent?.Invoke();
    }
    public override (int, int) GetStockAndMagAmmo()
    {
        return (_currentMaxAmmo , _currentMagSize );
    }
    public override (int, int) GetStockAndMagMax()
    {
        return (_maxAmmoValue, _magSize );
    }
    #endregion


#if UNITY_EDITOR
    [System.Serializable]
    [CustomEditor(typeof(WeaponRange))]
    public class WeaponRangeEditor : Editor
    {

        private string[] tabs = { "Base Weapon Data", "Weapon Range Data - Projectile", "Weapon Range Data - Fire Setting", "Weapon Range Data - Ammo Data", "Events", "Components" };
        private int currentTab = 0;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            WeaponRange TargetScript = target as WeaponRange;

            EditorGUILayout.BeginVertical();
            currentTab = GUILayout.Toolbar(currentTab, tabs);
            EditorGUILayout.Space(5f);
            EditorGUILayout.EndVertical();

            if (currentTab >= 0 || currentTab < tabs.Length)
            {
                switch (tabs[currentTab])
                {
                    case "Base Weapon Data":
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_iconWeapon"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_armLeft"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_armRight"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_body"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_flipBody"));
                        break;
                    case "Weapon Range Data - Projectile":

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_damage"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_force"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_projectileInShoot"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_randomSpeed"));
                        if (TargetScript._randomSpeed)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minSpeedProjectile"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxSpeedProjectile"));

                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("_speedProjectile"));
                        }

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_randomLifeTime"));
                        if (TargetScript._randomLifeTime)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minLifeTimeProjectile"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxLifeTimeProjectile"));
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("_lifeTimeProjectile"));
                        }

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_shootDir"));
                        break;
                    case "Weapon Range Data - Fire Setting":
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_fireRate"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_weaponType"));
                        break;
                    case "Weapon Range Data - Ammo Data":
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_timeToReloading"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_infinityAmmo"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxAmmoValue"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_infinityMag"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_magSize"));
                        break;
                    case "Events":
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("ReloadingWeaponEvent"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("UseWeaponEvent"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("CancelUseWeaponEvent"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("EndReloadingWeaponEvent"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("SwitchWeaponEvent"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_shootEvent"));
                        break;
                    case "Components":
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bulletSpawner"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_animtor"));
                        break;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

}


