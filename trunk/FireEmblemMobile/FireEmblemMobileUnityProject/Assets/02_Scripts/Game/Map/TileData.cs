using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Map
{
    public class GridTerrainData:ScriptableObject
    {
        [Serializable]
        public struct MoveTypeCosts
        {
            [SerializeField]
            public MoveType moveType;
            public int cost;

        }

        public List<Sprite> sprites;
        public new string name;
        public bool walkable = true;
        public int defenseBonus;
        public int avoBonus;
        public int speedMalus;
        public MoveTypeCosts[] movementCostsArray;
        private Dictionary<MoveType, int> movementCosts;
        
        private void OnEnable()
        {
            movementCosts = new Dictionary<MoveType, int>();
            foreach (var mtc in movementCostsArray)
            {
                movementCosts.Add(mtc.moveType, mtc.cost);
            }
        }
        public int GetMovementCost(MoveType moveType)
        {
            if (movementCosts.ContainsKey(moveType))
                return movementCosts[moveType];
            return 1;
        }

        public bool CanMoveThrough(MoveType moveType)
        {
            return walkable;//TODO
            //if (TerrainType.Obstacle == TerrainType)
            //    return false;
            // return true;
       
        }
    }
    [CreateAssetMenu(menuName = "Map/TileData", fileName = "TileData")]
    public class TileData : GridTerrainData
    {
       
        public TileBase[] tiles;
      
 
       
    }
}