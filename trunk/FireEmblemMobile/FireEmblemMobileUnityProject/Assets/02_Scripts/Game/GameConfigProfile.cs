using System;
using System.Collections.Generic;
using Game.Dialog.DialogSystem;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "GameConfig/Profile", fileName = "GameConfigProfile")]
    public class GameConfigProfile:ScriptableObject
    {
        public bool tutorialEnabled = false;
        public bool debugModeEnabled = false;
        public bool fixedGrowths = false;
        public bool overWriteItems = false;
        public bool overWriteSkills = false;
        public bool overWriteEvents = false;
      
        public bool overWriteAchievements = false;
        public bool overWriteMetaUpgrades = false;
        public ItemBP[] OverwritenItems;
        public SkillBp[] OverwritenSkills;
        public LGEventDialogSO[] OverwritenEvents;
        public LGEventDialogSO[] TmpEventsHolder;
        public DifficultyProfile chosenDifficulty;
        
        [SerializeField] List<UnitBP> playableCharacters;
        public  List<Unit> GetUnits()
        {
            var list = new List<Unit>();
            foreach (var unitbp in playableCharacters)
            {
                var unit = unitbp.Create(Guid.NewGuid());
                //Debug.Log("Name: "+unit.Name+" "+"Skills: "+unit.SkillManager.Skills);
                list.Add(unit);
            }
            return list;
        }
    }
}