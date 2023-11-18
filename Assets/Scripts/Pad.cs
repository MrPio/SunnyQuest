using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    private enum Directions
    {
        Up,
        Down,
        Left,
        Right,
        Jump
    }

    [SerializeField] private Directions _direction;
    private Pacman _pacman;

    private void Start()
    {
        _pacman = GameObject.FindWithTag("Pacman").GetComponent<Pacman>();
    }

    private void OnMouseDown()
    {
        if (_direction == Directions.Down)
            _pacman.input.y = -1;
        else if (_direction == Directions.Up)
            _pacman.input.y = 1;
        else if (_direction == Directions.Left)
            _pacman.input.x = -1;
        else if (_direction == Directions.Right)
            _pacman.input.x = 1;
        else if (_direction == Directions.Jump)
            _pacman.inputJump = true;
    }

    private void OnMouseUp()
    {
        if (_direction is Directions.Down or Directions.Up)
            _pacman.input.y = 0;
        else if (_direction is Directions.Left or Directions.Right)
            _pacman.input.x = 0;
        else if (_direction == Directions.Jump)
            _pacman.inputJump = false;
    }

    private void OnMouseExit()
    {
        if (_direction is Directions.Down or Directions.Up)
            _pacman.input.y = 0;
        else if (_direction is Directions.Left or Directions.Right)
            _pacman.input.x = 0;
        else if (_direction == Directions.Jump)
            _pacman.inputJump = false;
    }
}