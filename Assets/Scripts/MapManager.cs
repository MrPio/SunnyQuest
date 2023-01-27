using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static List<Tilemap> levelTilemaps = new();

    public GameObject grid;
    public List<GameObject> levels;
    [SerializeField] private GameObject _gate;

    private float lastSpawn;
    private Grid gridComponent;
    private readonly List<GameObject> _instantiatedLevels = new();
    private readonly InventoryManager _inventoryManager = InventoryManager.getInstance;

    private void Awake()
    {
        gridComponent = grid.GetComponent<Grid>();
        foreach (var g in GameObject.FindGameObjectsWithTag("EditorOnly"))
            Destroy(g);
        NewLevel();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (transform.position.x + CamManager.camWidth > lastSpawn)
        {
            NewLevel();

            var currentLevelSize = _inventoryManager.LevelsSize[_inventoryManager.CurrentLevel - 1];
            // REMOVING OLD LEVELS
            for (var i = _instantiatedLevels.Count - 1; i >= 0; --i)
            {
                if (_instantiatedLevels[i].transform.position.x + (currentLevelSize.x * gridComponent.cellSize.x) <
                    transform.position.x)
                {
                    print($"*** Removed level {i} ***");
                    Destroy(_instantiatedLevels[i]);
                    _instantiatedLevels.RemoveAt(i);
                    levelTilemaps.RemoveAt(i);
                }
            }
        }
    }

    private void NewLevel()
    {
        var currentLevel = ++_inventoryManager.CurrentLevel;
        var newLevel = Instantiate(levels[currentLevel - 1], grid.transform);
        var currentLevelSize = _inventoryManager.LevelsSize[currentLevel - 1];
        levelTilemaps.Add(newLevel.GetComponent<Tilemap>());
        newLevel.transform.SetPositionAndRotation(
            position: new Vector2(lastSpawn, 0),
            rotation: Quaternion.identity
        );
        _instantiatedLevels.Add(newLevel);

        // GATE SPAWNING
        if (!_inventoryManager.RestLevels.Contains(currentLevel))
        {
            var gate = Instantiate(
                original: _gate,
                position: new Vector2(
                    lastSpawn + currentLevelSize.x * gridComponent.cellSize.x - CamManager.camWidth / 2f, 0),
                rotation: Quaternion.identity
            );
            _inventoryManager.Gates.Add(gate);
        }

        lastSpawn += (currentLevelSize.x * gridComponent.cellSize.x);
        if (currentLevel > 1)
            _inventoryManager.SpawnedLevelsTotalSize +=
                _inventoryManager.LevelsSize[currentLevel - 2].x * gridComponent.cellSize.x;
        print($"*** Added level ***");
    }
}