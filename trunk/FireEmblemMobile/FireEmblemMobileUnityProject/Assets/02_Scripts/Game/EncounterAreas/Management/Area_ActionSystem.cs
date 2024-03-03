using Game.EncounterAreas.Encounters;
using Game.EncounterAreas.Serialization;
using Game.SerializedData;

namespace Game.EncounterAreas.Management
{
    public class Area_ActionSystem
    {
        public void Move(EncounterNode location)
        {  
           // Debug.Log("Move Party");
            var action = new MoveAction(location);
                action.PerformAction();
                SaveGameManager.Save();
        }
    }
}