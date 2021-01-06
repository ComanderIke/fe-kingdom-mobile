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
            //switch MoveType ?
            //moveType.GetMovementCost(this);
            //Designer wants to create new terraintypes
            // Designer also wants to create new moveTypes....
            // Also switch case for movetype in other classes... effective dmg etc...
            //Better for MoveType to know its movement penalties on terrain.
            //Additional Terrain Type Enum?
            //So 2 TileTypes both have Desert type enum but different avoid stats etc..
            // Cavalry
            // Movementcost on Desert:3

            return 0;
           // return moveType.GetMovementCost(TerrainType);
        }

        public virtual bool CanMoveThrough(MoveType moveType)
        {
            return true;
            //return moveType.CanMoveThrough(TerrainType);
        }
    }
}