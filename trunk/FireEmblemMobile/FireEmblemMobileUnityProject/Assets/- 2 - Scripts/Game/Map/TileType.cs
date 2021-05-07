﻿using Game.GameActors.Units;
using UnityEngine;

namespace Game.Grid
{
    [CreateAssetMenu(menuName = "Map/TileType", fileName = "TileType")]
    public class TileType : ScriptableObject
    {
        public TerrainType TerrainType;//Desert, Woods enum

        public int avoidBonus;
        public int defenseBonus;

        public virtual int GetMovementCost(MoveType moveType)
        {
            if (moveType.movementCosts.ContainsKey(TerrainType))
                return moveType.movementCosts[TerrainType];
            else return 1;
        }

        public virtual bool CanMoveThrough(MoveType moveType)
        {
            return true;
            //return moveType.CanMoveThrough(TerrainType);
        }
    }
}