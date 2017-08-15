using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Attributes
{
    [System.Serializable]
    public class Stats
    {
        public int maxHP;
        public int speed;
        public int defense;
        public int attack;
        public int accuracy;
        public int spirit;

        public Stats(int hp, int attack, int speed, int defense, int accuracy, int spirit)
        {
            this.maxHP = hp;
            this.attack = attack;
            this.speed = speed;
            this.defense = defense;
            this.accuracy = accuracy;
            this.spirit = spirit;
        }
    }
}
