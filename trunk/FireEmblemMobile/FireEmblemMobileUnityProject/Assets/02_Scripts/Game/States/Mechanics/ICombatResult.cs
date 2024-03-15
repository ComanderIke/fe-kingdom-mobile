using Game.AI.DecisionMaking;
using UnityEngine;

namespace Game.States.Mechanics
{
    public interface ICombatResult
    {
        Vector2Int GetAttackPosition();
        AttackResult AttackResult { get; set; }
        int GetDamageRatio();
        int GetTileDefenseBonuses();
        int GetTileAvoidBonuses();
        int GetTileSpeedBonuses();

        int GetTargetCount();
    }
    public interface ISkillResult:ICombatResult
    {
        Vector2Int GetCastPosition();
    }
   
}