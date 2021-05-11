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
        private FactionManager factionManager;


        public WorldMapGameplayInputReceiver(WorldMapGameManager gameManager)
        {
            selectionDataProvider = new WM_SelectionManager(gameManager.GetSystem<WM_PartySelectionSystem>());
            gameplayInput = new WM_GameplayInput();
            factionManager = gameManager.FactionManager;
            // WM_PartySelectionSystem.OnSelectedParty += OnSelectedCharacter;
            // WM_PartySelectionSystem.OnSelectedInActiveParty += OnSelectedCharacter;
        }

        void ResetInput()
        {
            selectionDataProvider.ClearData();
        }
        public void DoubleClickedActor(IWM_Actor unit)
        {
            if (factionManager.IsActiveFaction(unit.Faction))
            {
                gameplayInput.Wait(unit);
                //gameplayInput.ExecuteInputActions(null);
            }
            else
            {
                ActorClicked(unit);
            }
        }
        public void LocationClicked(WorldMapPosition location)
        {
            if(!location.IsFree())
            {
                return;
            }
           
            if (selectionDataProvider.SelectedActor != null)
            {
                if (location.IsReachable(selectionDataProvider.SelectedActor))
                {
                    if (selectionDataProvider.GetSelectedLocation()==location)
                    {
                        Debug.Log("Confirm Move");
                        gameplayInput.MoveActor(selectionDataProvider.SelectedActor, location);
                        gameplayInput.Wait(selectionDataProvider.SelectedActor);
                        // gameplayInput.ExecuteInputActions(null);
                        selectionDataProvider.ClearData();
                    }
                    else
                    {
                        selectionDataProvider.SelectedActor.GameTransformManager.SetPosition(location.gameObject.transform.position);
                        selectionDataProvider.SetSelectedLocation(location);
                        selectionDataProvider.ClearAttackTarget();
                        Debug.Log("Select location");
                    }
                }
                else
                {
                    if (location == selectionDataProvider.SelectedActor.location)
                    {
                        selectionDataProvider.SelectedActor.ResetPosition();
                        gameplayInput.DeselectActor();
                    }
                    else
                    {
                        gameplayInput.DeselectActor();
                    }
                    ResetInput();
                }

                Debug.Log("Location clicked with selected actor");
            }
            else
            {
                Debug.Log("Location clicked without selected actor");
                ResetInput();
                gameplayInput.DeselectActor();
            }
        }

        public void ActorClicked(IWM_Actor party)
        {
            if (factionManager.IsActiveFaction(party.Faction))
            {
                OwnActorClicked(party);
            }
            else
            {
                EnemyActorClicked(party);
                
            }
        }

        private void EnemyActorClicked(IWM_Actor party)
        {
            Debug.Log("Enemy Party clicked!");
            if (selectionDataProvider.SelectedActor == null)
            {
                gameplayInput.SelectActor(party);
            }
            else if(selectionDataProvider.GetSelectedAttackTarget()==party)
            {
                gameplayInput.AttackEnemyActor(party);
            }
            else
            {
                gameplayInput.AttackPreviewEnemyActor(party);
            }
            
        }

        private void OwnActorClicked(IWM_Actor party)
        {
            Debug.Log("Own Party clicked!");
            
            if (selectionDataProvider.SelectedActor == null)
            {
                gameplayInput.SelectActor(party);
            }
            else
            {
                //TODO Also select for now
                gameplayInput.SelectActor(party);
            }
        }
    }
}