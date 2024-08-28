using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

public class DeviceSensor : MonoBehaviour
{

    [SerializeField] private CharacterInputContoller _characterInputContoller;
    [SerializeField] private GameObject _mobileInput;
    [SerializeField] private bool _isGamepad;

    [SerializeField] private TMP_Text _debug;

    private DeviceType _currentSystem;
    public void Awake()
    {
        Application.targetFrameRate = 60;
        SystemDeterm();
        InputDeviceDeterm();
        InputSystem.onDeviceChange +=
        (device, change) =>
        {
            InputDeviceDeterm();
        };
    }
    public void SystemDeterm()
    {
        _currentSystem = SystemInfo.deviceType;
        switch (SystemInfo.deviceType)
        {
            case DeviceType.Handheld:
                _isGamepad = true;
                break;
            case DeviceType.Desktop:
                _isGamepad = false;
                break;
        }
    }
    public void InputDeviceDeterm() 
    {
        var gamepad = Gamepad.current;
        if (gamepad == null )
        {
            _isGamepad = false;
        }
        else if (gamepad is DualShockGamepad)
        {
            _isGamepad = true;

        }
        else if (gamepad is XInputController)
        {
            _isGamepad = true;
        }

        if (_currentSystem == DeviceType.Handheld) 
        {
            if (_isGamepad)
            {
                _mobileInput.SetActive(false);
            }
            else if (!_isGamepad)
            {
                _mobileInput.SetActive(true);
            }
        }
        _characterInputContoller.ChangeDeviceInput(_isGamepad || _currentSystem == DeviceType.Handheld);
        _debug.text = _currentSystem + " " + _isGamepad;


    }
}
