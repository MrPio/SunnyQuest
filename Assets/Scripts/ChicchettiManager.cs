using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ChicchettiManager : MonoBehaviour
{
    [SerializeField] private GameObject chicchetto;
    private float _nextSpawn, _elapsed;
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;

    private Dictionary<InventoryManager.Difficulty, float> _spawnFactor =
        new()
        {
            { InventoryManager.Difficulty.Hard, 1.1f },
            { InventoryManager.Difficulty.Medium, 0.85f },
            { InventoryManager.Difficulty.Easy, 0.55f }
        };

    private void Start()
    {
        SetSpawnTime();
    }

    public void SetSpawnTime()
    {
        var spawnTime = _inventoryManager.ChicchettiRate[_inventoryManager.CurrentLevel - 1] /
                        _spawnFactor[_inventoryManager.GameDifficulty];
        if (_inventoryManager.CrazyLevels.Contains(_inventoryManager.CurrentLevel))
            spawnTime /= 4f;
        _nextSpawn = Random.Range(spawnTime * 0.8f, spawnTime * 1.25f);
    }

    private void FixedUpdate()
    {
        if (_nextSpawn < 0.01f)
            return;
        _elapsed += Time.fixedDeltaTime;
        if (_elapsed >= _nextSpawn && !_inventoryManager.RestLevels.Contains(_inventoryManager.CurrentLevel))
        {
            _elapsed = 0;
            SetSpawnTime();
            Instantiate(chicchetto, gameObject.transform);
        }
    }
}