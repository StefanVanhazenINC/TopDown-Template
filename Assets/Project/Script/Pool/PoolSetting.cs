
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class PoolSetting<T>
{
    [SerializeField] private T _objectReference;
    private Transform _parentPoolObject;
    //private ObjectPool<T> _objectPool;
}
