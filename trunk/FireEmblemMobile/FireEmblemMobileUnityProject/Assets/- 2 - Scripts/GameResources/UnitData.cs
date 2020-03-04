using Assets.GameActors.Units.Attributes;
using UnityEngine;

namespace Assets.GameResources
{
    [System.Serializable]
    public class UnitData
    {
        [SerializeField] private Stats defaultStats = default;
        [SerializeField] private Stats swordFighterStats = default;
        [SerializeField] private Stats archerStats = default;
        [SerializeField] private Stats axeFighterStats = default;
        [SerializeField] private Stats spearFighterStats = default;

        public Stats DefaultStats => Object.Instantiate(defaultStats);

        public Stats ArcherStats => Object.Instantiate(archerStats);

        public Stats SwordFighterStats => Object.Instantiate(swordFighterStats);

        public Stats AxeFighterStats => Object.Instantiate(axeFighterStats);

        public Stats SpearFighterStats => Object.Instantiate(spearFighterStats);
    }
}