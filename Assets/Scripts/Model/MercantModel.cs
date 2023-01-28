using System;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

namespace DefaultNamespace.Model
{
    public abstract class MercantModel
    {
        public String Sprite;
        public String SpriteCut;
        public String Message;
        public int Cost;
        protected InventoryManager _inventoryManager = InventoryManager.getInstance;

        public void Buy()
        {
            _inventoryManager.Coins -= Cost;
            GameObject.FindWithTag("CoinCounter").GetComponent<TextMeshProUGUI>().text =
                _inventoryManager.Coins.ToString();
        }
    }
}