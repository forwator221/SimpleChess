using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ChessPlate _plate;
    [SerializeField] private InputController _input;

    private void Start()
    {
        _plate.Initialize();
        //_input.Initialize();
    }
}
