using System.Collections.Generic;
using Game.EncounterAreas.Encounters;
using Game.EncounterAreas.Encounters.Event;
using UnityEngine;

namespace Game.EncounterAreas.EncounterData
{
    [CreateAssetMenu(menuName = "GameData/EventEncounterData", fileName = "EventEncounterData")]
    public class EventEncounterNodeData: EncounterNodeData
    {
        public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
        {
            return new EventEncounterNode(parents, depth, childIndex,  label,description, sprite);
        }

        public EncounterNode CreateNode(string customId, List<EncounterNode> parents, int depth, int childIndex)
        {
            return new EventEncounterNode(customId,parents, depth, childIndex,  label,description, sprite);
        }
    }
}