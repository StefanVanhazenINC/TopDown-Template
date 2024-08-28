using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHitEffector : MonoBehaviour
{
    public GameObject _hitParticle;
    public void SetParticle(Vector2 point,Vector3 direction) 
    {
        if (_hitParticle) 
        {
            Debug.Log(point);
            GameObject particle = Instantiate(_hitParticle, point, Quaternion.identity);
            //particle.transform.forward = Vector3.Reflect(direction, Vector2.right);
            particle.transform.forward = -direction;
        }
    }
}
