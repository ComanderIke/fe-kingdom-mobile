using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.AI.AttackReactions;
using Assets.Scripts.Characters.Monsters;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    [CreateAssetMenu(menuName ="GameData/Monster", fileName ="Monster")]
    public class Monster : Unit
    {
        public MonsterType Type;
        public List<AttackReaction> Reactions { get; set; }
        [HideInInspector]
        public bool attentionWaked;
        //public Monster(string name, MonsterType type, Sprite sprite, Stats stats)
        //{


        //}
        private new void OnEnable()
        {
            base.OnEnable();
            Reactions = new List<AttackReaction>();
            if (Type == MonsterType.MeleeAttacker2x2)
            {
                GridPosition = new BigTilePosition(this);
            }
            else if (Type == MonsterType.MeleeAttacker)
            {
                //Reactions.Add(new NoReaction(this));
                Reactions.Add(new RevengeStrike(this));
                GridPosition = new GridPosition(this);
            }
            else
            {
                //Reactions.Add(new NoReaction(this));
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
