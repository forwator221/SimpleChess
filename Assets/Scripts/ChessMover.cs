using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class ChessMover : MonoBehaviour
{
    [SerializeField] private InputAction press, screenPos;
    [SerializeField] private LayerMask _cellMask, _chessMask;
    private bool _isDragging;
    private Camera _camera;

    private Chess _targetChess;
    private Cell _startCell;
    private Cell _targetCell;

    private Vector3 _touchPos;

    private Vector3 _worldPos
    {
        get
        {
            float z = _camera.WorldToScreenPoint(transform.position).z;
            return _camera.ScreenToWorldPoint(_touchPos + new Vector3(0, 0, z));
        }
    }
    private bool IsClickedOnChess
    {
        get
        {
            Ray ray = _camera.ScreenPointToRay(_touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _chessMask))
            {    
                _targetChess = hit.transform.GetComponent<Chess>();
                _startCell = _targetChess.GetComponentInParent<Cell>();
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        press.Enable();
        screenPos.Enable();
               
        _camera = Camera.main;

        screenPos.performed += _ => { _touchPos = _.ReadValue<Vector2>(); };
        press.performed += _ => 
        {           
            if (IsClickedOnChess) StartDraggingChess(); 
        };
        press.canceled += _ => 
        {            
            StopDraggingChess();
        };
    }

    private void Update()
    {
        if (_isDragging)
        {
            _targetCell = GetCell();
        }
    }

    private void StartDraggingChess()
    {
        StartCoroutine(DragChess());
    }

    private void StopDraggingChess()
    {
        _isDragging = false;
        DropChess();
    }

    private Cell GetCell()
    {
        Ray ray = _camera.ScreenPointToRay(_touchPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _cellMask))
        {
            Cell cell = hit.transform.GetComponent<Cell>();
            return cell;
        }

        return _startCell;
    }

    private IEnumerator DragChess()
    {
        _isDragging = true;
        _targetCell = null;
        _targetChess.transform.parent = null;

        Vector3 offset = _targetChess.transform.position - _worldPos;

        while (_isDragging)
        {
            _targetChess.transform.position = _worldPos + offset;            
            yield return null;
        }             
    }

    private void DropChess()
    {
        if (_targetCell.IsEmpty)
        {
            _targetChess.transform.parent = _targetCell.transform;
            _targetChess.transform.position = _targetCell.transform.position;
        }
    }
}
