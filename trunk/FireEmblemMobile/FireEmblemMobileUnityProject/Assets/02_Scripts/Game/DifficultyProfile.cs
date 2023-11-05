using System.Collections.Generic;
using _02_Scripts.Game.Dialog.DialogSystem;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameConfig/DifficultyProfile", fileName = "DifficultyProfile")]
    public class DifficultyProfile : ScriptableObject
    {
        public float ExpMult = 1.0f;
        public float GraceMult = 1.0f;
        public float GoldMult = 1.0f;
        public float ItemRate = 1.0f;
        public float EliteEnemyRate = 1.0f;
        public int LevelInfluence;
        public Attributes BaseStatInfluences;
        public Attributes GrowthInfluences;
        public List<LGEventDialogSO> extraEvents;
        public List<LGEventDialogSO> extraMaps; // Reusing maps but different enemy and player placements.
        [Header("Maybe just use different UnitBPs for all enemies with adjusted stats and skills")]
        public SerializableDictionary<UnitBP,SkillBp> uniqueSkills;

        [Header("Minotaur Boss")]
        public Attributes BaseStats;
        public int revivalStones;
        private List<SkillBp> skills;
        [Header("Lion Boss")]
        public Attributes BaseStats2;
        public int revivalStones2;
        private List<SkillBp> skills2;

    }
}