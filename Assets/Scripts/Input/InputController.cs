using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField, Range(0.01f, 1f)] private float _zoomingSpeed = 0.1f;
    [SerializeField, Range(0.1f, 10f)] private float _rotatingSpeed = 4f;
    [SerializeField, Range(0.1f, 2f)] private float _touchSensitivity = 0.9f;

    [SerializeField] private float _cameraZoomOutMin = 1;
    [SerializeField] private float _cameraZoomOutMax = 8;

    private TouchInput _input;
    private Coroutine _zoomCoroutine;
    private Coroutine _rotateCoroutine;
    private Camera _camera;

    private bool _isRotating;

    private Vector2 _touchPosition;

    public Vector2 TouchPosition => _touchPosition;   

    private void OnEnable()
    {
        _input = new TouchInput();
        _camera = Camera.main;

        _input.Touch.DoubleTouchContact.started += _ => DoubleTouchStart();
        _input.Touch.DoubleTouchContact.canceled += _ => DoubleTouchEnd(); 

        _input.Enable();
    }    

    private void OnDisable()
    {
        _input.Disable();
    }

    private void DoubleTouchEnd()
    {
        _isRotating = false;
        StopCoroutine(_zoomCoroutine);
        StopCoroutine(_rotateCoroutine);
    }

    private void DoubleTouchStart()
    {
        _zoomCoroutine = StartCoroutine(DoubleTouchZoom());
        _rotateCoroutine = StartCoroutine(DoubleTouchRotation());
    }

    private IEnumerator DoubleTouchZoom()
    {
        float previousDistance = 0f;
        float currentDistance = 0f;

        Vector2 currentFirstTouchPos = Vector2.zero;
        Vector2 currentSecondTouchPos = Vector2.zero;

        while (true)
        {
            currentFirstTouchPos = _input.Touch.SingleTouch.ReadValue<Vector2>();
            currentSecondTouchPos = _input.Touch.DoubleTouch.ReadValue<Vector2>();

            currentDistance = Vector2.Distance(currentFirstTouchPos,currentSecondTouchPos);           

            if (currentDistance > previousDistance)
            { 
                Zoom(_zoomingSpeed * -1);
            }
            else if (currentDistance < previousDistance)
            {
                Zoom(_zoomingSpeed);
            }

            previousDistance = currentDistance;

            yield return null;
        }
    }

    private IEnumerator DoubleTouchRotation()
    {
        _isRotating = true;

        while (_isRotating)
        {
            Rotate(_rotatingSpeed);
            yield return null;
        }
    }

    private void Zoom(float zoomingSpeed)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - zoomingSpeed, _cameraZoomOutMin, _cameraZoomOutMax);
    }

    private void Rotate(float rotatingSpeed)
    {
        _camera.transform.Rotate(Vector3.forward, rotatingSpeed * Time.deltaTime);
    }
}
