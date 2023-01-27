using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    void Start () {
        var btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        MapManager.levelTilemaps = new List<Tilemap>();
        InventoryManager.ResetInstance();
        Application.LoadLevel(Application.loadedLevel);
    }
}
