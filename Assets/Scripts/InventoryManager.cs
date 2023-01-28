using System;
using System.Collections.Generic;
using DefaultNamespace.Model;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class InventoryManager
    {
        private static InventoryManager _instance;
        public List<GameObject> Gates = new();
        public int Coins=0, Stars = 0;
        public int CurrentLevel;
        public GameObject MessageBox = null;
        public Shop Shop;
        public bool IsThereMessageBox = false;
        public float LastSpacebarMessageBox = 0;

        public List<Vector2> LevelsSize = new()
        {
            new Vector2(18, 10), //1
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10), //5
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(56, 10), //10
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(33, 10),
            new Vector2(18, 10), //15
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10), //20
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10), //25
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10),
            new Vector2(18, 10), //30
        };

        public List<int> RestLevels = new() { 1, 5, 10, 14 };
        public float SpawnedLevelsTotalSize, VisitedLevels;

        public List<MercantModel> MercantModels = new()
        {
            new Cornetto(), new Marmo(), new Gino()
        };

        public static InventoryManager getInstance
        {
            get { return _instance ??= new InventoryManager(); }
        }

        public static void ResetInstance()
        {
            _instance = null;
        }

        private InventoryManager()
        {
        }

        public void CollectStar()
        {
            ++Stars;
            Gates.RemoveAt(0);
        }

        public void HandleMessageBox(float xInput, bool spaceBar)
        {
            if (spaceBar)
            {
                Shop.Confirm();
                LastSpacebarMessageBox = Time.timeSinceLevelLoad;
                return;
            }

            if (xInput > 0.05)
                Shop.SetRightArrow();
            else if (xInput < -0.05)
                Shop.SetLeftArrow();
        }
    }
}