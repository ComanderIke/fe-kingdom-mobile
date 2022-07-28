using System.Linq;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameResources
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