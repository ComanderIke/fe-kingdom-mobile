using System.Collections.Generic;
using Game.GameActors.Players;
using UnityEngine;

namespace Game.AI
{
    public class AIAttackTarget
    {
        public Vector2Int OptimalAttackPos { get; set; }
        public IAttackableTarget Target { get; set; }
        public List<Vector2Int> AttackableTiles { get; set; }

        public AIAttackTarget(IAttackableTarget target)
        {
            Target = target;
            AttackableTiles = new List<Vector2Int>();
        }
    }
}