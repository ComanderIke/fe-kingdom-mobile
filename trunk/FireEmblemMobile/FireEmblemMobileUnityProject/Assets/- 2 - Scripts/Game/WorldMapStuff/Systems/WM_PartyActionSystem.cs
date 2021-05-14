using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using GameEngine;
using UnityEngine;

namespace Game.WorldMapStuff.Input
{
    public class WM_PartyActionSystem:IEngineSystem
    {
 
        public void AttackPreviewParty(WM_Actor party)
        {
            Debug.Log("TODO UI AttackPreview Party");
        }

        public void AttackParty(WM_Actor party)
        {
            Debug.Log("TODO Attack Party");
        }

        public void MoveParty(WM_Actor party, WorldMapPosition location)
        {
            party.location.Actor = null;
            party.location.Reset();
            party.location = location;
            location.Actor = party;
            party.GameTransformManager.SetPosition(location.transform.position);
            Debug.Log("Move Party");
        }

        public void Wait(WM_Actor party)
        {
            Debug.Log("Wait Party");
            party.TurnStateManager.IsSelected = false;
            party.TurnStateManager.IsWaiting = true;
        }

        public void Init()
        {
            
        }
    }
}