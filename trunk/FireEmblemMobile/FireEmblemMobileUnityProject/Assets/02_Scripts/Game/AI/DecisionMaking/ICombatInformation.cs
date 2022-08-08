using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using UnityEngine;

namespace Game.AI
{
    public interface ICombatInformation
    {
        ICombatResult GetCombatResultAtAttackLocation(IBattleActor attacker, IAttackableTarget targetTarget, Vector2Int tile);
    }
}