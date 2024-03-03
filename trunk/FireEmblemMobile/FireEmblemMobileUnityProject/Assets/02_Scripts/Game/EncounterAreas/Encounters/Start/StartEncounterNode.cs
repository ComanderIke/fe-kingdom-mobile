using Game.EncounterAreas.Management;
using Game.EncounterAreas.Model;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Start
{
    public class StartEncounterNode : EncounterNode
    {
        public StartEncounterNode() : base(null,0,0, "Home base","", null)
        {

        }

        public override void Activate(Party party)
        {
            GameObject.FindObjectOfType<AreaGameManager>().Continue();
       
        }
    }
}