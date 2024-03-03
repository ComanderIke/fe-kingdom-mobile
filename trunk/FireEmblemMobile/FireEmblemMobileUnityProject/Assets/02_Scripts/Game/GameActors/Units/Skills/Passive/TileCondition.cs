using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.Grid;
using Game.Grid.Tiles;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [CreateAssetMenu(menuName = "GameData/Skills/TargetConditions/OnTile", fileName = "TileCondition")]
    public class TileCondition:Condition
    {
        public List<TerrainType> TerrainTypes;

        public bool Valid(Tile tile=null)
        {
            return TerrainTypes.Contains(tile.TileData.TerrainType);
        }
    }
}