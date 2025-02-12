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
    public static List<int> RandomLevelOrder;

    private float lastSpawn;
    private Grid gridComponent;
    private readonly List<GameObject> _instantiatedLevels = new();
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;

    private void Awake()
    {
        gridComponent = GetComponent<Grid>();
        foreach (var g in GameObject.FindGameObjectsWithTag("EditorOnly"))
            Destroy(g);
        RandomLevelOrder = new List<int> { 0 };
        foreach (var restLevel in _inventoryManager.RestLevels)
        {
            RandomLevelOrder.AddRange(Enumerable
                .Range(RandomLevelOrder.Count, restLevel - 1 - RandomLevelOrder.Count)
                .OrderBy(_ => Random.value));
            RandomLevelOrder.Add(restLevel - 1);
        }

        RandomLevelOrder.AddRange(Enumerable.Range(RandomLevelOrder.Count, levels.Count - 2 - RandomLevelOrder.Count)
            .OrderBy(_ => Random.value));
        RandomLevelOrder.Add(levels.Count - 1);

        print(string.Join(", ", RandomLevelOrder));
        NewLevel();
    }

    private void FixedUpdate()
    {
        if (CamManager.mainCam.transform.position.x + CamManager.camWidth > lastSpawn &&
            _inventoryManager.CurrentLevel < _inventoryManager.LevelsSize.Count)
        {
            NewLevel();

            // REMOVING OLD LEVELS
            for (var i = _instantiatedLevels.Count - 1; i >= 0; --i)
            {
                var currentLevelSize =
                    _inventoryManager.LevelsSize[_inventoryManager.LastSpawnedLevel -
                                                 (_instantiatedLevels.Count - i)];
                if (_instantiatedLevels[i].transform.position.x + (currentLevelSize.x * gridComponent.cellSize.x) <
                    CamManager.mainCam.transform.position.x)
                {
                    // print($"*** Removed level {i} ***");
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
        var newLevel = Instantiate(levels[RandomLevelOrder[currentLevel - 1]], transform);
        var currentLevelSize = _inventoryManager.LevelsSize[RandomLevelOrder[currentLevel - 1]];
        levelTilemaps.Add(newLevel.GetComponent<Tilemap>());
        newLevel.transform.SetPositionAndRotation(
            position: new Vector2(lastSpawn - (CamManager.camWidth - 18) / 2f, 0),
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
                _inventoryManager.LevelsSize[RandomLevelOrder[currentLevel - 2]].x * gridComponent.cellSize.x;
        // print($"*** Added level ***");

        // CHICCHETTIMANAGER INITIALIZE
        GameObject.FindWithTag("ChicchettiManager").GetComponent<ChicchettiManager>().SetSpawnTime();
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