using System.Collections.Generic;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units.Interfaces;
using UnityEngine;

namespace Game.States.Mechanics.Battle
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