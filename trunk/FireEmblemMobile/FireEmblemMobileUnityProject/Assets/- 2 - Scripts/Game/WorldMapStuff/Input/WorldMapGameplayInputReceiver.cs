using Game.Manager;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;

namespace Game.WorldMapStuff.Input
{
    public class WorldMapGameplayInputReceiver: IWorldMapInputReceiver
    {
        private WM_GameplayInput gameplayInput;
        private IWM_SelectionDataProvider selectionDataProvider;
        private WM_LastInputPositionManager lastInputPositionManager;
        private FactionManager factionManager;


        public WorldMapGameplayInputReceiver(FactionManager factionManager,WM_PartySelectionSystem selectionSystem, WM_GameplayInput gameplayInput)
        {
            selectionDataProvider = new WM_SelectionManager(selectionSystem);
            this.gameplayInput = gameplayInput;
            
            this.factionManager = factionManager;
            WM_PartySelectionSystem.OnDeselectParty += selectionDataProvider.ClearData;
            // WM_PartySelectionSystem.OnSelectedParty += OnSelectedCharacter;
            // WM_PartySelectionSystem.OnSelectedInActiveParty += OnSelectedCharacter;
        }

        void ResetInput()
        {
            selectionDataProvider.ClearData();
        }
        public void DoubleClickedActor(WM_Actor unit)
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
            if(!location.HasSpace())
            {
                return;
            }
           
            if (selectionDataProvider.SelectedActor != null)
            {
                if (location.IsReachable(selectionDataProvider.SelectedActor)&&!selectionDataProvider.SelectedActor.TurnStateManager.IsWaiting)
                {
                    // if (selectionDataProvider.GetSelectedLocation()==location)
                    // {
                      //  Debug.Log("Confirm Move");
                        gameplayInput.MoveActor(selectionDataProvider.SelectedActor, location);
                        gameplayInput.Wait(selectionDataProvider.SelectedActor);
                        // gameplayInput.ExecuteInputActions(null);
                        selectionDataProvider.ClearData();
                    // }
                    // else
                    // {
                    //     selectionDataProvider.SelectedActor.GameTransformManager.SetPosition(location.gameObject.transform.position);
                    //     selectionDataProvider.SetSelectedLocation(location);
                    //     selectionDataProvider.ClearAttackTarget();
                    //     Debug.Log("Select location");
                    // }
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


            }
            else
            {
                ResetInput();
                gameplayInput.DeselectActor();
            }
        }

        public void ActorClicked(WM_Actor party)
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

        private void EnemyActorClicked(WM_Actor actor)
        {
            var selectedActor = selectionDataProvider.SelectedActor;
            if (selectedActor == null)
            {
                gameplayInput.SelectActor(actor);
            }
            else 
            {
                if (actor.location.IsAttackable(selectedActor))
                {
                    if (selectionDataProvider.GetSelectedAttackTarget() != actor)
                    {
                        selectedActor.ResetPosition();
                        selectionDataProvider.ClearData();
                        selectionDataProvider.SetSelectedAttackTarget(actor);
                        gameplayInput.AttackPreviewEnemyActor(actor);
                        
                    }
                    else
                    {
                        gameplayInput.AttackEnemyActor(actor);
                    }
                }
                else
                {
                    gameplayInput.SelectActor(actor);
                }
            }
            
        }

        private void OwnActorClicked(WM_Actor party)
        {
            Debug.Log("Own Party clicked!");
            
            // if (selectionDataProvider.SelectedActor!=null&&party.location == selectionDataProvider.SelectedActor.location)
            // {
            //     LocationClicked(party.location);
            // }
            // else
            // {
            //     //TODO Also select for now
                gameplayInput.SelectActor(party);
            // }
        }
    }
}