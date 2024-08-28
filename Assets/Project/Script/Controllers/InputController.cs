using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    #region Events
    [HideInInspector] public UnityEvent<Vector2> OnMoveEvent;
    [HideInInspector] public UnityEvent<Vector2> LookEvent;
    [HideInInspector] public UnityEvent<Vector2> PointPositionEvent;
    [HideInInspector] public UnityEvent<bool> OnDashEvent;
    [HideInInspector] public UnityEvent OnInteractionEvent;

    [HideInInspector] public UnityEvent<bool> OnAttackEvent;
    [HideInInspector] public UnityEvent OnChangeWeaponEvent;
    [HideInInspector] public UnityEvent OnReloadingWeaponEvent;
    [HideInInspector] public UnityEvent<bool> SwitchDeviceInput;

    #endregion
}
