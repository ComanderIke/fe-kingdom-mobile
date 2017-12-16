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
        public Monster(string name, MonsterType type) : base(name)
        {
            Type = type;
            if(type==MonsterType.Mammoth)
                GridPosition = new BigTilePosition(this);
            else
            {
                GridPosition = new GridPosition(this);
            }
        }
    }
}
