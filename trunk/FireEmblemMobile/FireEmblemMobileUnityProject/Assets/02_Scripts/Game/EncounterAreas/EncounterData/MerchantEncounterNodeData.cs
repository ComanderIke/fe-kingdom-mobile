using UnityEngine;

[CreateAssetMenu(menuName = "GameData/MerchantEncounterData", fileName = "MerchantEncounterData")]
public class MerchantEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent,int depth, int childIndex)
    {
        return new MerchantEncounterNode(parent, depth, childIndex, description, sprite);
    }
}