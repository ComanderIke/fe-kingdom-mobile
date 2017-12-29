using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class Monster :LivingObject
    {
        public MonsterType Type { get; set; }
        public Monster(string name, MonsterType type, Sprite sprite) : base(name, sprite)
        {
            Sprite = sprite;
            Type = type;
            List<int> attackRanges = new List<int>();
            attackRanges.Add(1);
            if (type == MonsterType.Mammoth)
            {
                GridPosition = new BigTilePosition(this);
                Stats = new Stats(50, 5, 15, 5, 8, 5, 5, 4, attackRanges);
            }
            else
            {
                Stats = new Stats(20, 5, 9, 5, 3, 6, 3, 4, attackRanges);
                GridPosition = new GridPosition(this);
            }
            
            
        }
    }
}
