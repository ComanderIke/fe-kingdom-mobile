using Game.GameInput;
using UnityEngine;

namespace Game.Mechanics.Battle
{
    [CreateAssetMenu(menuName =  "Debug/BattlePreview", fileName="battlePreview")]
    public class BattlePreview : ScriptableObject
    {
        public IBattleActor Attacker;
        public IBattleActor Defender;
        public BattlePreviewStats AttackerStats;
        public BattlePreviewStats DefenderStats;

    }
}