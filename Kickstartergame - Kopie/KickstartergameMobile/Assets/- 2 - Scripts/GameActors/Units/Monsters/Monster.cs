using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.AI.AttackReactions;
using Assets.Scripts.Characters.Attributes;
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
    public class Monster :LivingObject
    {
        public MonsterType Type { get; set; }
        public List<AttackReaction> Reactions { get; set; }
        public List<TargetPoint> TargetPoints { get; set; }
        public Monster(string name, MonsterType type, Sprite sprite) : base(name, sprite)
        {
            Sprite = sprite;
            Type = type;
            List<int> attackRanges = new List<int>();

            TargetPoints = new List<TargetPoint>();
            Reactions = new List<AttackReaction>();
            attackRanges.Add(1);
            if (type == MonsterType.Mammoth)
            {
                GridPosition = new BigTilePosition(this);
                Stats = new Stats(50, 5, 15, 5, 8, 5, 5, 4, attackRanges);
            }
            else if (type == MonsterType.Sabertooth)
            {
                Stats = new Stats(20, 5, 9, 5, 3, 6, 3, 4, attackRanges);
                //Reactions.Add(new NoReaction(this));
                Reactions.Add(new RevengeStrike(this));
                TargetPoints.Add(new TargetPoint(-285, 55, "Head", 1.0f, 0, 0.7f, true));
                TargetPoints.Add(new TargetPoint(-44, 59, "Body", 1.0f, 0,1.4f));
                TargetPoints.Add(new TargetPoint(-222, -85, "Front Legs", 1.0f, 0,0.6f));
                TargetPoints.Add(new TargetPoint(101, -62, "Rear Legs", 1.0f, 0,0.6f));
                GridPosition = new GridPosition(this);
            }
            else
            {
                Stats = new Stats(20, 5, 9, 5, 3, 6, 3, 4, attackRanges);
                //Reactions.Add(new NoReaction(this));
                Reactions.Add(new RevengeStrike(this));
                GridPosition = new GridPosition(this);
            }
            
            
        }
        public bool attentionWaked;
        public bool CanSeePosition(int x, int y)
        {
            return MainScript.GetInstance().GetSystem<GridSystem>().PositionVisible(this, x,y);

        }
        public AttackReaction GetRandomAttackReaction()
        {
            int rng = UnityEngine.Random.Range(0, Reactions.Count);
            return Reactions[rng];
        }
        public void MonsterAttentionWaked()
        {
            Debug.Log("AttentionWaked!");
            attentionWaked = true;
            RessourceScript rs = GameObject.FindObjectOfType<RessourceScript>();
            GameObject.Instantiate(rs.particles.enemyAttention,this.GameTransform.GameObject.transform);
            MainScript.GetInstance().GetSystem<GridSystem>().ShowSightRange(this);
        }
    }
}
