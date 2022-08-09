using Game.AI;
using UnityEngine;

namespace Game.Mechanics
{
    public interface ICombatResult
    {
        Vector2Int GetAttackPosition();
        BattleResult BattleResult { get; set; }
        int GetDamageRatio();
        int GetTileDefenseBonuses();
        int GetTileAvoidBonuses();
 
    }
}