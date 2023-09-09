using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameResources;
using GameEngine;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameConfig", fileName = "GameConfig")]
    public class GameConfig : SingletonScriptableObject<GameConfig>
    {
        public bool tutorialEnabled = false;
        public bool debugModeEnabled = false;
        [SerializeField] List<UnitBP> selectableCharacters;
       
        public  List<Unit> GetUnits()
        {
            var list = new List<Unit>();
            foreach (var unitbp in selectableCharacters)
            {
                var unit = unitbp.Create();
                //Debug.Log("Name: "+unit.Name+" "+"Skills: "+unit.SkillManager.Skills);
                list.Add(unit);
            }
            return list;
        }
       
    }
}