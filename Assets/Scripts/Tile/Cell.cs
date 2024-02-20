using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
    [SerializeField] private Color _whiteCellColor, _blackCellColor;

    public bool IsEmpty
    {
        get
        {
            Chess chess = transform.GetComponentInChildren<Chess>();
            if (chess != null)
                return false;
            else
                return true;
        }        
    }

    public void Initialize(bool isBlackCell)
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.material.color = isBlackCell ? _blackCellColor : _whiteCellColor;
    }
}
