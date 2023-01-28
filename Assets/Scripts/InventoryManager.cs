using System.Collections.Generic;
using DefaultNamespace.Model;
using UnityEngine;

namespace DefaultNamespace
{
    public class InventoryManager
    {
        private static InventoryManager _instance;
        public readonly List<GameObject> Gates = new();
        public int Coins=0, Stars = 0;
        public int LastSpawnedLevel,CurrentLevel=1;
        public float SpawnedLevelsTotalSize, VisitedLevels;
        public GameObject MessageBox = null;
        public Shop Shop;
        public bool IsThereMessageBox = false;
        public float LastSpacebarMessageBox = 0;

        public readonly List<Vector2> LevelsSize = new()
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
            new Vector2(23, 23), //15
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

        public readonly List<int> RestLevels = new() { 5, 10, 14 };

        public readonly List<float> ChicchettiRate = new()
        {
            0, 0, 0, 0, 10,
            10, 9, 9, 8, 8,
            8, 7, 7, 6,
            6, 6, 6, 5.5f, 5.5f,
            5, 5, 5, 4.5f, 4.5f,
            4.5f, 4, 4, 3.5f, 2,
        };

        public readonly List<MercantModel> MercantModels = new()
        {
            new Cornetto(), new Marmo(), new Gino()
        };

        public static InventoryManager GetInstance
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