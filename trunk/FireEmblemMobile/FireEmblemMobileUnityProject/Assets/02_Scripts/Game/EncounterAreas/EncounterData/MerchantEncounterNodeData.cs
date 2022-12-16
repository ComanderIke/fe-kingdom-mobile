using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/MerchantEncounterData", fileName = "MerchantEncounterData")]
public class MerchantEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
    {
        return new MerchantEncounterNode(parents, depth, childIndex,  label,description, sprite);
    }
}