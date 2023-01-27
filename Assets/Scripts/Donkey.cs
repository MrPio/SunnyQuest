using System;
using UnityEngine;

public class Donkey : MonoBehaviour
{
    [SerializeField] private AnimationCurve SlidingCurve;
    [SerializeField] private float Speed = 1;
    private Pacman _pacman;
    
    private float _time;
    private Vector2 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _pacman = GameObject.FindGameObjectWithTag("Pacman").GetComponent<Pacman>();
    }

    private void FixedUpdate()
    {
        _time += Time.fixedDeltaTime* Speed;
        transform.position = _startPosition + Vector2.left * (SlidingCurve.Evaluate(_time));
        _time -= (int)_time;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
        {
            _pacman.Hit();
        }
    }
}