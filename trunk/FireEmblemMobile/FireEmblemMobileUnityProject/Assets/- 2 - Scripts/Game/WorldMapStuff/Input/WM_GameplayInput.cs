using Game.AI;
using Game.WorldMapStuff.Systems;
using UnityEngine;

namespace Game.WorldMapStuff.Input
{
    public class WM_GameplayInput
    {
        private WM_PartyActionSystem partyActionSystem;
        private WM_PartySelectionSystem partySelectionSystem;

        public void SelectParty(Party party)
        {
            partySelectionSystem.SelectParty(party);

        }

        public void AttackPreviewEnemyParty(Party party)
        {
            partyActionSystem.AttackPreviewParty(party);
        }

        public void AttackEnemyParty(Party party)
        {
            partyActionSystem.AttackParty(party);
        }
        public void MoveParty(Party party, WorldMapPosition location)
        {
            partyActionSystem.MoveParty(party, location);
        }
    }
}