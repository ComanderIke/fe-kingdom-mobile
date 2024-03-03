using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units.Interfaces;
using Game.States.Mechanics;
using UnityEngine;

namespace Game.AI.DecisionMaking
{
    public interface ICombatInformation
    {
        ICombatResult GetCombatResultAtAttackLocation(IBattleActor attacker, IAttackableTarget targetTarget, Vector2Int tile);
    }
}