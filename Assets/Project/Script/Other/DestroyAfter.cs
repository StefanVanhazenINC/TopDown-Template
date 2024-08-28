using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy;
    private void Start()
    {
        Invoke("Destroy", _timeToDestroy);
    }

    public void Destroy() 
    {
        Destroy(gameObject);
    }


}
