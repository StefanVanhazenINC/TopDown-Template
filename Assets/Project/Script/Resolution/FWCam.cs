using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FWCam : MonoBehaviour
{
    public int fullWidthUnits = 14;

    void Start()
    {
        // Force fixed width
        float ratio = (float)Screen.height / (float)Screen.width;
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = (float)fullWidthUnits * ratio / 2.0f;
    }
}
