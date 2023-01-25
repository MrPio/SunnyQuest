using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public GameObject grid;
    public List<GameObject> levels;
    public Vector2 levelCells;

    public static List<Tilemap> levelTilemaps = new();

    private float lastSpawn;
    private Grid gridComponent;
    private List<GameObject> instantiatedLevels = new();

    private void Awake()
    {
        gridComponent = grid.GetComponent<Grid>();
        foreach (var g in GameObject.FindGameObjectsWithTag("EditorOnly"))
            Destroy(g);
        instantiatedLevels.Add(Instantiate(levels[0], grid.transform));
        levelTilemaps.Add(instantiatedLevels[0].GetComponent<Tilemap>());
        lastSpawn = levelCells.x * gridComponent.cellSize.x;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (transform.position.x + CamManager.camWidth > lastSpawn)
        {
            var newLevel = Instantiate(levels[Random.Range(1,levels.Count)], grid.transform);
            levelTilemaps.Add(newLevel.GetComponent<Tilemap>());
            newLevel.transform.SetPositionAndRotation(
                position: new Vector2(lastSpawn, 0),
                rotation: Quaternion.identity
            );
            instantiatedLevels.Add(newLevel);
            lastSpawn += (levelCells.x * gridComponent.cellSize.x);
            print($"*** Added level ***");

            for (var i = instantiatedLevels.Count - 1; i >= 0; --i)
            {
                if (instantiatedLevels[i].transform.position.x + (levelCells.x * gridComponent.cellSize.x) <
                    transform.position.x)
                {
                    print($"*** Removed level {i} ***");
                    Destroy(instantiatedLevels[i]);
                    instantiatedLevels.RemoveAt(i);
                    levelTilemaps.RemoveAt(i);
                }
            }
        }
    }
}