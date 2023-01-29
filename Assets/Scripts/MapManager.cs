using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Utilies;

public class MapManager : MonoBehaviour
{
    public static List<Tilemap> levelTilemaps = new();

    public List<GameObject> levels;
    [SerializeField] private GameObject gate;

    private float lastSpawn;
    private Grid gridComponent;
    private readonly List<GameObject> _instantiatedLevels = new();
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;

    private void Awake()
    {
        gridComponent = GetComponent<Grid>();
        foreach (var g in GameObject.FindGameObjectsWithTag("EditorOnly"))
            Destroy(g);
        NewLevel();
    }

    private void FixedUpdate()
    {
        if (CamManager.mainCam.transform.position.x + CamManager.camWidth > lastSpawn)
        {
            NewLevel();

            var currentLevelSize = _inventoryManager.LevelsSize[_inventoryManager.LastSpawnedLevel - 1];
            // REMOVING OLD LEVELS
            for (var i = _instantiatedLevels.Count - 1; i >= 0; --i)
            {
                if (_instantiatedLevels[i].transform.position.x + (currentLevelSize.x * gridComponent.cellSize.x) <
                    CamManager.mainCam.transform.position.x)
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
        var currentLevel = ++_inventoryManager.LastSpawnedLevel;
        var newLevel = Instantiate(levels[currentLevel - 1], transform);
        var currentLevelSize = _inventoryManager.LevelsSize[currentLevel - 1];
        levelTilemaps.Add(newLevel.GetComponent<Tilemap>());
        newLevel.transform.SetPositionAndRotation(
            position: new Vector2(lastSpawn, 0),
            rotation: Quaternion.identity
        );
        _instantiatedLevels.Add(newLevel);


        // GATE SPAWNING AND SHOP INITIALIZE
        if (!_inventoryManager.RestLevels.Contains(currentLevel))
        {
            var newGate = Instantiate(
                original: gate,
                position: new Vector2(
                    lastSpawn + currentLevelSize.x * gridComponent.cellSize.x - CamManager.camWidth / 2f, 0),
                rotation: Quaternion.identity
            );
            _inventoryManager.Gates.Add(newGate);
        }
        else
            InitializeShops(GameObject.FindGameObjectsWithTag("Mercant"));

        lastSpawn += (currentLevelSize.x * gridComponent.cellSize.x);
        if (currentLevel > 1)
            _inventoryManager.SpawnedLevelsTotalSize +=
                _inventoryManager.LevelsSize[currentLevel - 2].x * gridComponent.cellSize.x;
        print($"*** Added level ***");
    }

    private void InitializeShops(GameObject[] shops)
    {
        var shuffledShops = shops.ToList();
        ListsOperation.Shuffle(shuffledShops);

        var count = 0;
        foreach (var merchantScript in shuffledShops.Select(shop => shop.GetComponent<Mercant>()))
        {
            merchantScript.Model = _inventoryManager.MercantModels[count++];
            merchantScript.Initialize();
        }
    }
}