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

        private static InventoryManager InventoryManager => InventoryManager.getInstance;

        public virtual  void Buy()
            {
            InventoryManager.Coins -= Cost;
            GameObject.FindWithTag("CoinCounter").GetComponent<TextMeshProUGUI>().text =
                InventoryManager.Coins.ToString();
        }
    }
}