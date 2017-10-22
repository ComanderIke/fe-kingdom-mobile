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
        public BigTile Position { get; set; }

        public Monster(string name, MonsterType type) : base()
        {
            this.name = name;
            movRange = 3;
            stats = new Stats(100, 5, 5, 5, 5, 5);
            AttackRanges = new List<int>();
            AttackRanges.Add(1);
            HP = stats.maxHP;
            level = 1;
        }
        public override void SetPosition(int x, int y)
        {
            base.SetPosition(x, y);
            Position = new BigTile(new Vector2(x, y), new Vector2(x+1, y), new Vector2(x, y+1), new Vector2(x+1, y+1));
        }
    }
}
