using System.Collections.Generic;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units
{
    [CreateAssetMenu(menuName = "GameData/Unit/MoveType", fileName = "MoveType")]
    public class MoveType : ScriptableObject
    {
        public Dictionary<TerrainType, int> movementCosts;

        public int baseMovement = 3;

        private List<TerrainType> TerrainTypes;
    }
}