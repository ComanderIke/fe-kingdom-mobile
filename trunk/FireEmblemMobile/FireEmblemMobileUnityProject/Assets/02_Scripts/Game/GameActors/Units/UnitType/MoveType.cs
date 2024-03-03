using Game.GUI.Utility;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.UnitType
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