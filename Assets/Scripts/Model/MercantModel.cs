using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Model
{
    public abstract class MercantModel
    {
        public String Sprite;
        public String SpriteCut;
        public String Message;
        public int BaseCost;
        public int Bought = 0;

        public int CalculateCost =>
            (int)(BaseCost * (1f + 0.35f * Bought) * _costFactor[InventoryManager.GameDifficulty]);

        private static InventoryManager InventoryManager => InventoryManager.GetInstance;

        private Dictionary<InventoryManager.Difficulty, float> _costFactor =
            new()
            {
                { InventoryManager.Difficulty.Hard, 1.4f },
                { InventoryManager.Difficulty.Medium, 1f },
                { InventoryManager.Difficulty.Easy, 0.75f }
            };

        public virtual void Buy()
        {
            InventoryManager.Coins -= CalculateCost;
            GameObject.FindWithTag("CoinCounter").GetComponent<TextMeshProUGUI>().text =
                InventoryManager.Coins.ToString();
            ++Bought;
        }
    }
}