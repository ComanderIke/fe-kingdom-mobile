using System.Collections.Generic;
using Game.GameActors.Players;
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
      

        public List<Mechanics.AttackData> AttacksData { get; set; }
        public IAttackableTarget TargetObject { get; set; }
    }
}