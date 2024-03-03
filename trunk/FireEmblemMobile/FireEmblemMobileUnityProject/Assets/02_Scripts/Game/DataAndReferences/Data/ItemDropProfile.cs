using System.Collections.Generic;
using Game.EncounterAreas.Encounters.Battle;
using UnityEngine;

namespace Game.DataAndReferences.Data
{
    [CreateAssetMenu(menuName = "GameData/Profiles/ItemDropRates")]
    public class ItemDropProfile : ScriptableObject
    {
        [SerializeField] private List<RewardItem> relicDrops;
        [SerializeField] private List<RewardItem> gemDrops;
        [SerializeField] private List<RewardItem> smithingMaterialDrops;
        [SerializeField] private List<RewardItem> otherDrops;
 
        public IEnumerable<RewardItem> GetRelicDrops()
        {
            return relicDrops;

        }
        public IEnumerable<RewardItem> GetGemDrops()
        {
            return gemDrops;

        }
        public IEnumerable<RewardItem> GetSmithingMaterialDrops()
        {
            return smithingMaterialDrops;

        }
        public IEnumerable<RewardItem> GetOtherDrops()
        {
            return otherDrops;

        }
    }
}