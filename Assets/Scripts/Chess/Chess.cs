using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Chess : MonoBehaviour
{
    [SerializeField] private Color _whiteChessColor, _blackChessColor;

    public void Initialise(bool isBlackChess)
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.material.color = isBlackChess ? _blackChessColor : _whiteChessColor;
    }
}
