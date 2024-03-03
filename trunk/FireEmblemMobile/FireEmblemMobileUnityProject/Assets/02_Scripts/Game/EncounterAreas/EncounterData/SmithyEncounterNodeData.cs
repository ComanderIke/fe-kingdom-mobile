using System.Collections.Generic;
using Game.EncounterAreas.Encounters;
using Game.EncounterAreas.Encounters.Smithy;
using UnityEngine;

namespace Game.EncounterAreas.EncounterData
{
    [CreateAssetMenu(menuName = "GameData/SmithyEncounterData", fileName = "SmithyEncounterData")]
    public class SmithyEncounterNodeData: EncounterNodeData
    {
        public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
        {
            return new SmithyEncounterNode(parents, depth, childIndex,  label,description, sprite);
        }
    }
}