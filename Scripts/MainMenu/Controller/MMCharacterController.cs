using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMCharacterController : MonoBehaviour
{
    public float _sensitivity;

    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;


    void Start()
    {
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;
    }

    void Update()
    {
        if (_isRotating)
        {
            _mouseOffset = (Input.mousePosition - _mouseReference);
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
            transform.Rotate(_rotation);
            _mouseReference = Input.mousePosition;
        }
    }
    void OnMouseDown()
    {
        _isRotating = true;
        _mouseReference = Input.mousePosition;
    }
    void OnMouseUp()
    {
        _isRotating = false;
    }
}