using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.AttackReactions
{
    public abstract class AttackReaction
    {
        public Unit User  { get; set; }
        public string Name { get; set; }
        public List<Vector2> TargetPositions { get; set; }

        protected AttackReaction(Unit unit)
        {
            User = unit;
            TargetPositions = new List<Vector2>();
        }

        public abstract void Execute();
    }
}
