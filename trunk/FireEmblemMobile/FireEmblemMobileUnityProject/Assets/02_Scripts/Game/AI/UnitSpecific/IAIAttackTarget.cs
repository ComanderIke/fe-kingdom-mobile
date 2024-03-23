using System.Collections.Generic;
using Game.GameActors.InteractableGridObjects;
using Game.States.Mechanics;
using UnityEngine;

namespace Game.AI.UnitSpecific
{
    public class AIAttackTarget
    {
        public Vector2Int OptimalAttackPos { get; set; }
        public Vector2Int OptimalCastPos { get; set; }
        public IAttackableTarget Target { get; set; }
        public List<Vector2Int> AttackableTiles { get; set; }
        public ICombatResult CombatResult { get; set; }

        public void AddAttackableTile(Vector2Int pos)
        {
            if(!AttackableTiles.Contains(pos))
                AttackableTiles.Add(pos);
        }

        public AIAttackTarget(IAttackableTarget target)
        {
            Target = target;
            AttackableTiles = new List<Vector2Int>();
        }
    }
}