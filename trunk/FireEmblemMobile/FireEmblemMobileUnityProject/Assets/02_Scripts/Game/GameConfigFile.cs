using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameConfig", fileName = "GameConfig")]
    public class GameConfigFile : ScriptableObject
    {
        public bool tutorial = false;
        [SerializeField] List<UnitBP> selectableCharacters;
       
        public  List<Unit> GetUnits()
        {
            var list = new List<Unit>();
            foreach (var unitbp in selectableCharacters)
            {
                var unit = unitbp.Create();
                Debug.Log("Name: "+unit.Name+" "+"Skills: "+unit.SkillManager.Skills);
                list.Add(unit);
            }
            return list;
        }
       
    }
}