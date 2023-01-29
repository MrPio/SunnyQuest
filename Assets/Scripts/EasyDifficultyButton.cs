using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class EasyDifficultyButton : MonoBehaviour
{
    void Start () {
        var btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        InventoryManager.GetInstance.GameDifficulty = InventoryManager.Difficulty.Easy;
        InventoryManager.GetInstance.InitializeGame();
    }
}
