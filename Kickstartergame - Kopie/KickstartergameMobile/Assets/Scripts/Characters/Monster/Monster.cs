using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.AI.AttackReactions;
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
        public List<AttackReaction> Reactions { get; set; }
        public Monster(string name, MonsterType type, Sprite sprite) : base(name, sprite)
        {
            Sprite = sprite;
            Type = type;
            List<int> attackRanges = new List<int>();
            Reactions = new List<AttackReaction>();
            attackRanges.Add(1);
            if (type == MonsterType.Mammoth)
            {
                GridPosition = new BigTilePosition(this);
                Stats = new Stats(50, 5, 15, 5, 8, 5, 5, 4, attackRanges);
            }
            else
            {
                Stats = new Stats(20, 5, 9, 5, 3, 6, 3, 4, attackRanges);
                Reactions.Add(new NoReaction(this));
                Reactions.Add(new RevengeStrike(this));
                GridPosition = new GridPosition(this);
            }
            
            
        }

        public AttackReaction GetRandomAttackReaction()
        {
            int rng = UnityEngine.Random.Range(0, Reactions.Count);
            return Reactions[rng];
        }
    }
}
