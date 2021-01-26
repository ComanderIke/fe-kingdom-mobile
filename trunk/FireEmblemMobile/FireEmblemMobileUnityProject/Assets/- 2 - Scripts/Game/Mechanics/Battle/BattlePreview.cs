using UnityEngine;

namespace Game.Mechanics.Battle
{
    [CreateAssetMenu(menuName =  "Debug/BattlePreview", fileName="battlePreview")]
    public class BattlePreview : ScriptableObject
    {
        public BattlePreviewStats Attacker;
        public BattlePreviewStats Defender;

    }
}