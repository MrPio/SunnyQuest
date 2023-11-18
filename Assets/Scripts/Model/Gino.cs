using UnityEngine;

namespace DefaultNamespace.Model
{
    public class Gino : MercantModel
    {
        public Gino()
        {
            Sprite = "Images/Gino";
            SpriteCut = "Images/GinoCut";
            Message = "Wuzzup! Aren't you tired of yer little jumps? Wanna buy a +1 JMP?";
            BaseCost = 18;
        }

        public override void Buy()
        {
            base.Buy();
            var Pacman = GameObject.FindWithTag("Pacman").GetComponent<Pacman>();
            Pacman.jumpForce+=0.2f;
            InventoryManager.GetInstance.CollectPoints(typeof(Gino));
        }
    }
}