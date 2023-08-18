using System;
using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units
{
    [CreateAssetMenu(menuName = "GameData/Unit/MoveType", fileName = "MoveType")]
    public class MoveType: EffectiveAgainstType
    {
        public int moveTypeId;
        public Sprite icon;
        public int baseMovement = 3;
        public SerializableDictionary<TerrainType, int> moveCosts;
        public int GetMovementCost(TerrainType type)
        {
            return moveCosts[type];
        }
    }

    public abstract class EffectiveAgainstType :ScriptableObject
    {
        
    }

 
}