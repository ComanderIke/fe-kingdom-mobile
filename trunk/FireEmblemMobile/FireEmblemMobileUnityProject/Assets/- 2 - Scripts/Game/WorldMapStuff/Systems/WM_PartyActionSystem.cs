using System;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Interfaces;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Serialization;
using GameEngine;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public class WM_PartyActionSystem:IEngineSystem
    {
        public static Action OnJoinClicked;
        public static Action OnSplitClicked;
        private WM_PreviewSystem previewSystem;
        private WM_PartySelectionSystem selectionSystem;
        public IPartyActionRenderer partyActionRenderer;
        public PartyInstantiator partyInstantiator;

        public WM_PartyActionSystem(WM_PreviewSystem system, WM_PartySelectionSystem selectionSystem)
        {
            previewSystem = system;
            this.selectionSystem = selectionSystem;
           
        }
        public void AttackPreviewParty(WM_Actor party)
        {
            previewSystem.ShowAttackPreview(party);
            Debug.Log("TODO UI AttackPreview Party");
        }

        private Party currentAttackTarget;
        public void AttackParty(WM_Actor party)
        {
            var selected = selectionSystem.SelectedActor;
            Debug.Log(selectionSystem.SelectedActor+" "+selectionSystem.SelectedActor.name);
            if (selected is Party pty)
            {
               
                GameSceneController.Instance.LoadBattleLevel(pty, (Party)party);
                currentAttackTarget = (Party) party;
               // Debug.Log("TODO ATTACK FINISHED");
                //GameSceneController.Instance.OnBattleFinished += AttackFinished;
            }
        }

        private void AttackFinished()
        {
            var selected = selectionSystem.SelectedActor;
            var action = new AttackFinishedAction((Party)selected, currentAttackTarget,selectionSystem, SceneTransferData.Instance.BattleOutCome);
            action.PerformAction();
            action.Save(SaveData.currentSaveData);
           

        }

        public void MoveParty(WM_Actor party, LocationController location)
        {  
           // Debug.Log("Move Party");
            var action = new MoveAction((Party)party, location);
                action.PerformAction();
                action.Save(SaveData.currentSaveData);
            


        }

        public void Wait(WM_Actor party)
        {
           // Debug.Log("Wait Party");

            var action = new WaitAction((Party)party, selectionSystem);
            action.PerformAction();
            action.Save(SaveData.currentSaveData);
        }

        private void JoinParty()
        {
            var actor = selectionSystem.SelectedActor;
            if (actor is Party party)
            {
                Party otherParty = null;
                foreach (var locActor in party.location.worldMapPosition.GetActors())
                {
                    if (locActor != party)
                        otherParty = (Party)locActor;
                }

                if (otherParty != null)
                {
                    var action = new JoinAction((Party) party, otherParty);
                    action.PerformAction();
                    action.Save(SaveData.currentSaveData);
                }

            }
        }

        private void SplitParty()
        {
            var actor = selectionSystem.SelectedActor;
            if (actor is Party party)
            {
                var action = new SplitAction(party, partyInstantiator);
                action.PerformAction();
                action.Save(SaveData.currentSaveData);
                
            }
        }
     

        public void Init()
        {
            
        }

       

        public void Deactivate()
        {
            OnJoinClicked -= JoinParty;
            OnSplitClicked -= SplitParty;

        }

        public void Activate()
        {
            OnJoinClicked += JoinParty;
            OnSplitClicked += SplitParty;

        }
    }
}