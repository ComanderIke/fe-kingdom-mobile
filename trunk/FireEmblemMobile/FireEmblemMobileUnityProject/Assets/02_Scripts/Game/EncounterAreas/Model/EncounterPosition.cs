using System.Collections.Generic;

namespace Game.WorldMapStuff.Model
{
    public class EncounterPosition
    {
        public List<string> MovedEncounterIds { get; set; }
        public string EncounterNodeId { get; set; }

        private EncounterNode encounterNode;
        public EncounterNode EncounterNode
        {
            get
            {
                return encounterNode;
            }
            set
            {
                encounterNode = value;
                if(encounterNode!=null)
                    EncounterNodeId = encounterNode.GetId();
                else
                {
                    EncounterNodeId = "";
                }
            }
        }

        private List<EncounterNode> movedEncounters;

        public IReadOnlyList<EncounterNode> MovedEncounters
        {
            get
            {
                return movedEncounters.AsReadOnly();
            }
        }

        public void AddMovedEncounter(EncounterNode node)
        {
            movedEncounters.Add(node);
            MovedEncounterIds.Add(node.GetId());
        }
        public EncounterPosition()
        {
            movedEncounters = new List<EncounterNode>();
            MovedEncounterIds = new List<string>();
        }

        public void RemoveMovedEncounterAt(int i)
        {
            movedEncounters.RemoveAt(i);
            MovedEncounterIds.RemoveAt(i);
        }
    }
}