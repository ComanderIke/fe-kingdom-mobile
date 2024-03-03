using System.Collections.Generic;
using Game.EncounterAreas.Encounters;
using Game.EncounterAreas.Encounters.Merchant;
using UnityEngine;

namespace Game.EncounterAreas.EncounterData
{
    [CreateAssetMenu(menuName = "GameData/MerchantEncounterData", fileName = "MerchantEncounterData")]
    public class MerchantEncounterNodeData: EncounterNodeData
    {
        public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
        {
            return new MerchantEncounterNode(parents, depth, childIndex,  label,description, sprite);
        }
    }
}