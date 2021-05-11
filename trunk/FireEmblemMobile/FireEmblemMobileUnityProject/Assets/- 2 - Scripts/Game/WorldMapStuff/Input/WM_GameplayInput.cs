using Game.AI;
using Game.WorldMapStuff.Systems;
using UnityEngine;

namespace Game.WorldMapStuff.Input
{
    public class WM_GameplayInput
    {
        private WM_PartyActionSystem partyActionSystem;
        private WM_PartySelectionSystem partySelectionSystem;

        public void SelectActor(IWM_Actor party)
        {
            partySelectionSystem.SelectParty(party);

        }
        public void DeselectActor()
        {
            partySelectionSystem.DeselectActor();

        }

        public void AttackPreviewEnemyActor(IWM_Actor party)
        {
            partyActionSystem.AttackPreviewParty(party);
        }

        public void AttackEnemyActor(IWM_Actor party)
        {
            partyActionSystem.AttackParty(party);
        }
        public void MoveActor(IWM_Actor party, WorldMapPosition location)
        {
            partyActionSystem.MoveParty(party, location);
        }

        public void Wait(IWM_Actor party)
        {
            partyActionSystem.Wait(party);
        }
    }
}