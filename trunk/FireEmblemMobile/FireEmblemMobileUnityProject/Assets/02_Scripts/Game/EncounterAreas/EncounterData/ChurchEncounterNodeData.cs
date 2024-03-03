using System.Collections.Generic;
using Game.EncounterAreas.Encounters;
using Game.EncounterAreas.Encounters.Church;
using UnityEngine;

namespace Game.EncounterAreas.EncounterData
{
    [CreateAssetMenu(menuName = "GameData/ChurchEncounterData", fileName = "ChurchEncounterData")]
    public class ChurchEncounterNodeData: EncounterNodeData
    {
        public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
        {
            return new ChurchEncounterNode(parents, depth, childIndex, label,description, sprite);
        }
    }
}