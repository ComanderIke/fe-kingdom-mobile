using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [CreateAssetMenu(fileName = "Village", menuName = "GameData/Village")]
    public class Village:ScriptableObject
    {
        
        public List<BuildingType> buildings;
        public Sprite background;

        public Village():base()
        {
            buildings = new List<BuildingType>();
        }
    
        
        
        
    }

    [Serializable]
    public enum BuildingType
    {
        Smithy,
        Tavern,
        Church,
        Shop,
        Barracks
    }
}