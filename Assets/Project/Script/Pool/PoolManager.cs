 using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Projectile _projecttileReference;
    [SerializeField] private int _maxSizePool = 1000;
    [SerializeField] private Transform _parentPool;
    private ObjectPool<Projectile> _objectPool;
    public static PoolManager Instance;

    private void Awake()
    {
        CreateSingleton();
        CreatePool();
    }
    public Projectile GetObjectInPool() 
    {
        return _objectPool.Get();
    }
    public void CreateSingleton()
    {
        if (Instance==null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    #region ObjectPool
    private void CreatePool() 
    {
        _objectPool = new ObjectPool<Projectile>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, true, 200, _maxSizePool);
    }
    private void ReturnObjectPool(Projectile obj)
    {
        _objectPool.Release(obj);
    }
    private void OnDestroyObject(Projectile obj)
    {
        Destroy(obj);
    }

    private void OnReturnToPool(Projectile obj)
    {
        obj.gameObject.SetActive(false);
    }
    private void OnTakeFromPool(Projectile obj)
    {
        obj.gameObject.SetActive(true);
    }

    private Projectile CreatePooledObject()
    {
        Projectile obj = Instantiate(_projecttileReference, Vector3.zero, Quaternion.identity, _parentPool);
        obj.gameObject.SetActive(false);
        obj.DisableCallback += ReturnObjectPool;
        return obj;

    }
    #endregion
}
