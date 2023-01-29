using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Donkey : MonoBehaviour
{
    [SerializeField] private AnimationCurve SlidingCurve;
    [SerializeField] private Animator animator;
    [SerializeField] private float Speed = 1;
    private Pacman _pacman;
    
    private Dictionary<InventoryManager.Difficulty, float> _difficultyFactor =
        new()
        {
            { InventoryManager.Difficulty.Hard, 1.35f },
            { InventoryManager.Difficulty.Medium, 1f },
            { InventoryManager.Difficulty.Easy, 0.75f }
        };
    
    private float _time;
    private Vector2 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _pacman = GameObject.FindGameObjectWithTag("Pacman").GetComponent<Pacman>();
    }

    public void Initialize()
    {
        Speed *= _difficultyFactor[InventoryManager.GetInstance.GameDifficulty];
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
            print("*** damage donkey ***");
        }
    }

    public void Hit()
    {
        animator.SetTrigger("Die");
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}