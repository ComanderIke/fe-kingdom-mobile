using System.Linq;
using Game.GameActors.Units.UnitType;
using UnityEngine;

namespace Game.DataAndReferences.Data
{
    [System.Serializable]
    public class UnitData
    {
        [SerializeField] private MoveType[] moveTypes;
        

        public MoveType GetMoveType(int moveTypeId)
        {
            return moveTypes.FirstOrDefault(m => m.moveTypeId == moveTypeId);
        }
    }
}