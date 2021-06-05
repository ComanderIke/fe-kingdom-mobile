using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.Attributes;
using UnityEngine;

namespace Game.GameResources
{
    [System.Serializable]
    public class UnitData
    {
        [SerializeField] private Stats defaultStats = default;
        [SerializeField] private Stats swordFighterStats = default;
        [SerializeField] private Stats archerStats = default;
        [SerializeField] private Stats axeFighterStats = default;
        [SerializeField] private Stats spearFighterStats = default;
        [SerializeField] private MoveType[] moveTypes;

        public Stats DefaultStats => Object.Instantiate(defaultStats);

        public Stats ArcherStats => Object.Instantiate(archerStats);

        public Stats SwordFighterStats => Object.Instantiate(swordFighterStats);

        public Stats AxeFighterStats => Object.Instantiate(axeFighterStats);

        public Stats SpearFighterStats => Object.Instantiate(spearFighterStats);

        public MoveType GetMoveType(int moveTypeId)
        {
            return moveTypes.FirstOrDefault(m => m.moveTypeId == moveTypeId);
        }
    }
}