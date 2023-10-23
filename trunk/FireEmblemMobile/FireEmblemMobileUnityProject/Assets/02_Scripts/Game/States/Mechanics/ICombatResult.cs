using Game.AI;
using UnityEngine;

namespace Game.Mechanics
{
    public interface ICombatResult
    {
        Vector2Int GetAttackPosition();
        AttackResult AttackResult { get; set; }
        int GetDamageRatio();
        int GetTileDefenseBonuses();
        int GetTileAvoidBonuses();
        int GetTileSpeedBonuses();
 
    }
    public interface ISkillResult:ICombatResult
    {
        Vector2Int GetCastPosition();
    }
   
}