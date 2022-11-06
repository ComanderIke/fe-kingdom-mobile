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
        [SerializeField] List<Unit> selectableCharacters;
        [SerializeField] private List<MetaUpgrade> metaUpgrades;
        public  List<Unit> GetUnits()
        {
            foreach(var unit in selectableCharacters)
                unit.Initialize();
            return selectableCharacters;
        }
        public  List<MetaUpgrade> GetUpgrades()
        {
            return metaUpgrades;
        }
    }
}