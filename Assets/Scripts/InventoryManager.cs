using System;
using System.Collections.Generic;
using DefaultNamespace.Model;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class InventoryManager : MonoBehaviour
    {
        private static InventoryManager _instance;
        public readonly List<GameObject> Gates = new();
        public int Coins, Stars;
        public int LastSpawnedLevel, CurrentLevel = 1;
        public float SpawnedLevelsTotalSize, VisitedLevels;
        public GameObject MessageBox;
        public Shop Shop;
        public bool IsThereMessageBox;
        public float LastSpacebarMessageBox;

        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        public Difficulty GameDifficulty = Difficulty.Easy;

        private Dictionary<Difficulty, float> _pointsFactor =
            new()
            {
                { Difficulty.Hard, 1.7f },
                { Difficulty.Medium, 1.4f },
                { Difficulty.Easy, 1f }
            };
        private Dictionary<Difficulty, int> _healthFactor =
            new()
            {
                { Difficulty.Hard, 3 },
                { Difficulty.Medium, 4 },
                { Difficulty.Easy, 5 }
            };

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
            new Vector2(47, 17),
            new Vector2(55, 18),
            new Vector2(18, 10),
            new Vector2(23, 62),
            new Vector2(77, 11), //20
            new Vector2(30, 10),
            new Vector2(44, 10),
            new Vector2(52, 16)
        };

        public readonly List<int> RestLevels = new() { 5, 10, 14, 18 };
        public readonly List<int> CrazyLevels = new() { 9, 20 };
        public int Points;

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

        public void CollectPoints(Type type)
        {
            var toAdd = 0;
            if (type == typeof(Coin))
            {
                toAdd += 5;
            }
            else if (type == typeof(Star))
            {
                toAdd += 75;
            }
            else if (type == typeof(Marmo))
            {
                toAdd += 100;
            }
            else if (type == typeof(Cornetto))
            {
                toAdd += 35;
            }
            else if (type == typeof(Gino))
            {
                toAdd += 155;
            }
            else if (type == typeof(NewLevelHitbox))
            {
                toAdd += 10 * CurrentLevel;
            }

            toAdd = (int)(toAdd * _pointsFactor[GameDifficulty]);
            Points += toAdd;

            var points = Resources.Load("Points") as GameObject;
            var go = Instantiate(points, GameObject.FindWithTag("Canvas").transform);
            go.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"+ {toAdd} pts";
            go.transform.position = GameObject.FindWithTag("Pacman").transform.position + Vector3.up * 0.5f;
            print($"*****{go.transform.position}****");
        }

        public void InitializeGame()
        {
            GameObject.FindWithTag("Health").SetActive(true);
            GameObject.FindWithTag("Timer").SetActive(true);
            GameObject.FindWithTag("CoinsCount").SetActive(true);
            GameObject.FindWithTag("TutorialStar").SetActive(true);
            var pacman=GameObject.FindWithTag("Pacman").GetComponent<Pacman>();
            pacman.Health = _healthFactor[GameDifficulty];
            pacman.UpdateHeartsIcons();
        }
    }
}