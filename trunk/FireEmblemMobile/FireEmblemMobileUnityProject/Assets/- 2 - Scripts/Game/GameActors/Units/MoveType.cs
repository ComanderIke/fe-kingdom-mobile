using System;
using System.Collections.Generic;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units
{
    [CreateAssetMenu(menuName = "GameData/Unit/MoveType", fileName = "MoveType")]
    public class MoveType : ScriptableObject
    {
        public int moveTypeId;
        public Dictionary<TerrainType, int> movementCosts;
        [SerializeField] public List<int> movementCostSerialized;
        public int baseMovement = 3;

        void OnEnable()
        {
            movementCosts = new Dictionary<TerrainType, int>();
            int cnt = 0;
            if(movementCostSerialized!=null)
                foreach (var terrainType in (TerrainType[]) Enum.GetValues(typeof(TerrainType)))
                {
                    if(movementCostSerialized.Count > cnt)
                        movementCosts[terrainType] = movementCostSerialized[cnt];
                    cnt++;
                }
        }
        
    }
}