using System;
using System.Collections.Generic;
using Game.GameActors.Items.Consumables;
using Game.GameActors.Units;
using Game.GameResources;
using GameEngine;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameConfig/Config", fileName = "GameConfig")]
    public class GameConfig : SingletonScriptableObject<GameConfig>
    {
        public GameConfigProfile ConfigProfile;
        public  List<Unit> GetUnits()
        {
            return ConfigProfile.GetUnits();
        }
       
    }
}