using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpread : MonoBehaviour
{
    #region Variable
    [SerializeField] private float _rangeSpread;
    #endregion


    #region WeapobSpread Method
    public void ChageSpread(Transform point)
    {
        point.localEulerAngles = new Vector3(0,0,0) ;
        float angleZ = point.localEulerAngles.y;
        float tempAngel = _rangeSpread ;
        angleZ += Random.Range(-tempAngel, tempAngel);
        point.localEulerAngles = new Vector3(point.localEulerAngles.x, point.localEulerAngles.y, angleZ);
      
    }
    #endregion
}
