using Game.Manager;
using Game.WorldMapStuff.Input;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public class WorldMapGameplayInputReceiver: IWorldMapInputReceiver
    {
        private WM_GameplayInput gameplayInput;
        private IWM_SelectionDataProvider selectionDataProvider;
        private WM_LastInputPositionManager lastInputPositionManager;
        public void LocationClicked(WorldMapPosition location)
        {
            if (selectionDataProvider.SelectedActor != null)
            {
                Debug.Log("Location clicked with selected actor");
            }
            else
            {
                Debug.Log("Location clicked without selected actor");
            }
        }

        public void PartyClicked(Party party)
        {
            if (WorldMapGameManager.Instance.FactionManager.IsActiveFaction(party.Faction))
            {
                OwnPartyClicked(party);
            }
            else
            {
                EnemyPartyClicked(party);
                
            }
        }

        private void EnemyPartyClicked(Party party)
        {
            Debug.Log("Enemy Party clicked!");
            if (selectionDataProvider.SelectedActor == null)
            {
                gameplayInput.SelectParty(party);
            }
            else if(selectionDataProvider.GetSelectedAttackTarget()==party)
            {
                gameplayInput.AttackEnemyParty(party);
            }
            else
            {
                gameplayInput.AttackPreviewEnemyParty(party);
            }
            
        }

        private void OwnPartyClicked(Party party)
        {
            Debug.Log("Own Party clicked!");
            
            if (selectionDataProvider.SelectedActor == null)
            {
                gameplayInput.SelectParty(party);
            }
            else
            {
                //TODO Also select for now
                gameplayInput.SelectParty(party);
            }
        }
    }
}