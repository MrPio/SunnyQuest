using UnityEngine;

namespace DefaultNamespace.Model
{
    public class Cornetto : MercantModel
    {
        public Cornetto()
        {
            Sprite = "Images/Cornetto";
            SpriteCut = "Images/CornettoCut";
            Message = "Hey there! Why not take a rest? Wanna buy a +1 HP?";
            Cost = 6;
        }

        public override void Buy()
        {
            base.Buy();
            var Pacman = GameObject.FindWithTag("Pacman").GetComponent<Pacman>();
            if (Pacman.Health < Pacman.MaxHealth)
            {
                ++Pacman.Health;
                Pacman.UpdateHeartsIcons();
            }
        }
    }
}