using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Animator _animator;
    [SerializeField] private Pacman _pacman;

    [SerializeField] List<int> _levelsDuration = new()
    {
        300, 300, 240, 240, 240, 220, 220, 220, 220, 220, 200, 200, 200, 200, 200, 180, 180, 160, 160, 160, 140, 140,
        140, 120, 120, 100, 100, 80, 80, 50
    };


    private float _levelStart, _elapsed;
    private readonly InventoryManager _inventoryManager = InventoryManager.getInstance;
    private bool _shortage = false;

    private void Start()
    {
        NewLevel();
    }

    public void NewLevel()
    {
        _levelStart = Time.timeSinceLevelLoad;
        _elapsed = 0;
    }

    private void FixedUpdate()
    {
        _elapsed += Time.fixedDeltaTime;
        var totDuration = _levelsDuration[_inventoryManager.CurrentLevel - 1];
        var remaining = totDuration - _elapsed;

        if (remaining <= 0f)
        {
            _timerText.text = "Time's up!";
            _pacman.Hit(999);
        }
        
        _timerText.text = remaining.ToString("F2");
        
        // WARNING ANIMATION
        if (!_shortage && remaining < totDuration * 0.25f)
        {
            _shortage = true;
            _animator.SetTrigger("Shortage");
        }
        else if (_shortage && remaining >= totDuration * 0.25f)
        {
            _shortage = false;
            _animator.SetTrigger("StopShortage");
        }
    }
}