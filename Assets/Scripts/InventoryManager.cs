using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class InventoryManager
    {
        private static InventoryManager _instance;
        public List<GameObject> Gates=new ();
        public int Coins,Stars = 0;
        public int CurrentLevel;

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
    }
}