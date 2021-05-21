using Game.AI;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;

namespace Game.WorldMapStuff.Input
{
    public class WM_GameplayInput
    {
        private WM_PartyActionSystem partyActionSystem;
        private WM_PartySelectionSystem partySelectionSystem;

        public WM_GameplayInput(WM_PartySelectionSystem partySelectionSystem, WM_PartyActionSystem partyActionSystem)
        {
            this.partySelectionSystem = partySelectionSystem;
            this.partyActionSystem = partyActionSystem;
        }
        public void SelectActor(WM_Actor party)
        {
            partySelectionSystem.SelectParty(party);

        }
        public void DeselectActor()
        {
            partySelectionSystem.DeselectActor();

        }

        public void AttackPreviewEnemyActor(WM_Actor party)
        {
            partyActionSystem.AttackPreviewParty(party);
        }

        public void AttackEnemyActor(WM_Actor party)
        {
            partyActionSystem.AttackParty(party);
        }
        public void MoveActor(WM_Actor party, LocationController location)
        {
            partyActionSystem.MoveParty(party, location);
        }

        public void Wait(WM_Actor party)
        {
            partyActionSystem.Wait(party);
        }
    }
}