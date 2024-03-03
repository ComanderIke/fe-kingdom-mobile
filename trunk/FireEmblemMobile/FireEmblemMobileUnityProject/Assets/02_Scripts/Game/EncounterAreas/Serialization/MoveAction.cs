using Game.EncounterAreas.Encounters;
using Game.GameActors.Player;
using Game.SerializedData;

namespace Game.EncounterAreas.Serialization
{
    public class MoveAction:SerializedAction
    {
        private EncounterNode location;
        public MoveAction(EncounterNode location)
        {
            this.location = location;
        }

        public override void PerformAction()
        {
            MyDebug.LogLogic("Moving to Node: " + location);
            Player.Instance.Party.EncounterComponent.AddMovedEncounter(location);
            Player.Instance.Party.EncounterComponent.EncounterNode = location;
           // Player.Instance.Party.EncounterComponent.MovedEncounterIds.Add(location.GetId());
            //Player.Instance.Party.EncounterComponent.EncounterNodeId = location.GetId();
           // Player.Instance.Party.GameObject.transform.position = location.gameObject.transform.position;

        }

        public override void Save(SaveData current)
        {
            //Debug.Log("Saving MoveAction");
            // if (party.Faction.IsPlayerControlled)
            // {
            //     current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).locationId = location.UniqueId;
            // }
            // else
            // {
            //     current.campaignData.enemyFactionData.Parties.FirstOrDefault(p => p.name == party.name).locationId = location.UniqueId;
            // }
        }
    }
}