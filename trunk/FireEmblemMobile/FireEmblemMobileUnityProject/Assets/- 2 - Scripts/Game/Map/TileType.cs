using Game.GameActors.Units;
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

            return moveType.movementCosts[TerrainType];
        }

        public virtual bool CanMoveThrough(MoveType moveType)
        {
            return true;
            //return moveType.CanMoveThrough(TerrainType);
        }
    }
}