using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class HardDifficultyButton : MonoBehaviour
{
    void Start () {
        var btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        InventoryManager.GetInstance.GameDifficulty = InventoryManager.Difficulty.Hard;
        InventoryManager.GetInstance.InitializeGame();
    }
}
