using System.Collections.Generic;
using Game.EncounterAreas.Encounters;

namespace Game.EncounterAreas.AreaConstruction
{
    public class Column
    {
        public List<EncounterNode> children;
        public int index;
        public bool battle = false;

        public Column()
        {
            children = new List<EncounterNode>();
        }
    }
}