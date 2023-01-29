using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Animator _animator;
    [SerializeField] private Pacman _pacman;

    List<int> _levelsDuration = new()
    {
        55,
        60,
        55,
        55,
        999, // 5
        45,
        40,
        35,
        30,
        999, // 10
        35,
        30,
        25,
        170,
        200, // 15
        175,
        160,
        999,
        160,
        220, // 20
        180,
        60,
        20,
        120,
        120, // 25
        100,
        100,
        80,
        80,
        50
    };


    private float _elapsed, _lastCheck;
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;
    private bool _shortage;
    private int level = 0;

    private Dictionary<InventoryManager.Difficulty, float> _difficultyFactor =
        new()
        {
            { InventoryManager.Difficulty.Hard, 0.7f },
            { InventoryManager.Difficulty.Medium, 1f },
            { InventoryManager.Difficulty.Easy, 1.5f }
        };

    private void Start()
    {
        NewLevel();
    }

    public void NewLevel()
    {
        _elapsed = 0;
        _lastCheck = 0;
        ++level;
    }

    private void FixedUpdate()
    {
        _elapsed += Time.fixedDeltaTime;
        if (_elapsed - _lastCheck < .08f)
            return;
        _lastCheck = _elapsed;

        var totDuration = _levelsDuration[level - 1]*_difficultyFactor[_inventoryManager.GameDifficulty];
        var remaining = totDuration - _elapsed;

        // TIME'S UP!
        if (remaining <= 0f)
        {
            if (_inventoryManager.CurrentLevel != _inventoryManager.LevelsSize.Count)
            {
                _timerText.text = "Time's up!";
                _pacman.Hit(999);
                Destroy(gameObject);
            }
            else
            {
                _timerText.text = "You Win!";
                _pacman.Win();
            }
        }

        _timerText.text = (remaining).ToString("F1").Replace(',', '.');

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