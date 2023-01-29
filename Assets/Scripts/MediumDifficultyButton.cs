using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class MediumDifficultyButton : MonoBehaviour
{
    void Start () {
        var btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        InventoryManager.GetInstance.GameDifficulty = InventoryManager.Difficulty.Medium;
        InventoryManager.GetInstance.InitializeGame();
        
    }
}
