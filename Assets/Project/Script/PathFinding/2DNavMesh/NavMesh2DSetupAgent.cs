using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopDown_Template
{
    public class NavMesh2DSetupAgent : MonoBehaviour
    {
        #region Unity Callback
        void Start()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        #endregion


    }
}