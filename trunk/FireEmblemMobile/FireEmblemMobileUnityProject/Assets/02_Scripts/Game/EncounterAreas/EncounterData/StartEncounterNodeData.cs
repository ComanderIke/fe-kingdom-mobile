using System.Collections.Generic;
using Game.EncounterAreas.Encounters;
using Game.EncounterAreas.Encounters.Start;
using UnityEngine;

namespace Game.EncounterAreas.EncounterData
{
    [CreateAssetMenu(menuName = "GameData/StartEncounterData", fileName = "StartEncounterData")]
    public class StartEncounterNodeData: EncounterNodeData
    {
        public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
        {
            return new StartEncounterNode();
        }
    }
}