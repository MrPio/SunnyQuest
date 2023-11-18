using UnityEngine;

namespace DefaultNamespace.Model
{
    public class Marmo : MercantModel
    {
        
        public Marmo()
        {
            Sprite = "Images/Marmo";
            SpriteCut = "Images/MarmoCut";
            Message = "Meoow! Speed is everyFURRing! Wanna buy a +1 SPD?";
            BaseCost = 15;
        }

        public override void Buy()
        {
            base.Buy();
            var Pacman = GameObject.FindWithTag("Pacman").GetComponent<Pacman>();
            Pacman.ladderSpeed += 1f;
            Pacman.moveSpeed += 1.1f;
            InventoryManager.GetInstance.CollectPoints(typeof(Marmo));
        }
    }
}