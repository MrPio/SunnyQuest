﻿using UnityEngine;

namespace DefaultNamespace.Model
{
    public class Gino : MercantModel
    {
        public Gino()
        {
            Sprite = "Images/Gino";
            SpriteCut = "Images/GinoCut";
            Message = "Wuzzup! Aren't you tired of yer little jumps? Wanna buy a +1 JMP?";
            Cost = 9;
        }

        public void Buy()
        {
            base.Buy();
            var Pacman = GameObject.FindWithTag("Pacman").GetComponent<Pacman>();
            ++Pacman.jumpHoldLimit;
        }
    }
}