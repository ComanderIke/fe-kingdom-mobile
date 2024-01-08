using System;
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
        public float BondexpRate = 1.0f;
        public float ItemRate = 1.0f;
        public float EliteEnemyRate = 1.0f;
        public int LevelInfluence;
        [TextArea]
        public string Description;
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
        public Sprite Icon;

        public List<DifficultyVariable> GetDifficultyVariables()
        {
            var list = new List<DifficultyVariable>();
            var textStyle = ExpMult < 1 ? DifficultyVariableStyle.Decrease : Math.Abs(ExpMult - 1) < 0.01f?DifficultyVariableStyle.Normal:DifficultyVariableStyle.Increase;
            string prefix = ExpMult <= 1 ? "" : "+";
            list.Add(new DifficultyVariable("Exp:", prefix+Math.Round(ExpMult*100f-100)+"%", textStyle));
            textStyle = GoldMult < 1 ? DifficultyVariableStyle.Decrease : Math.Abs(GoldMult - 1) < 0.01f?DifficultyVariableStyle.Normal:DifficultyVariableStyle.Increase;
            prefix = GoldMult <= 1 ? "" : "+";
            list.Add(new DifficultyVariable("Gold:", prefix+Math.Round(GoldMult*100f-100)+"%", textStyle));
            textStyle = GraceMult < 1 ? DifficultyVariableStyle.Decrease : Math.Abs(GraceMult - 1) < 0.01f?DifficultyVariableStyle.Normal:DifficultyVariableStyle.Increase;
            prefix = GraceMult <= 1 ? "" : "+";
            list.Add(new DifficultyVariable("Grace:", prefix+Math.Round(GraceMult*100f-100)+"%", textStyle));
            textStyle = BondexpRate < 1 ? DifficultyVariableStyle.Decrease :!(Math.Abs(BondexpRate - 1) < 0.01f)? DifficultyVariableStyle.Increase:DifficultyVariableStyle.Normal;
            prefix = BondexpRate <= 1 ? "" : "+";
            list.Add(new DifficultyVariable("Bond Exp:", prefix+Math.Round(BondexpRate*100f-100)+"%", textStyle));
            return list;
        }

    }
}