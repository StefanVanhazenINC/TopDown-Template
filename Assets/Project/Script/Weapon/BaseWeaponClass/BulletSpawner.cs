using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _visualProjectTile;
    [SerializeField] private float _sizeProjecTile = 1;
    [SerializeField] private LayerMask _hitLayer;
    public void SpawnBullet(Vector2 positionSpawn,Quaternion direction,
        int damage, float force, float speed, float lifeTime) 
    {
        Projectile t_projectTile = PoolManager.Instance.GetObjectInPool();
        t_projectTile.transform.rotation = direction;
        t_projectTile.transform.position = positionSpawn;
        t_projectTile.SetProjecTile(damage, force, speed, lifeTime, _sizeProjecTile, _hitLayer, _visualProjectTile);
    }
    
}
