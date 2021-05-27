using System;
using System.Linq;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Interfaces;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
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
                WorldMapSceneController.Instance.LoadBattleLevel(pty, (Party)party);
                currentAttackTarget = (Party) party;
                WorldMapSceneController.Instance.OnBattleFinished += AttackFinished;
            }
        }

        private void AttackFinished(bool victory)
        {
            var selected = selectionSystem.SelectedActor;
            if (victory)
            {
                Debug.Log("Enemy Party Defeated");
                currentAttackTarget.Defeated();
                var location = currentAttackTarget.location;
                MoveParty(selected, location);
                Wait(selected);
            }
            else
            {
                if (selected is Party pty)
                {
                    pty.Defeated();
                    Debug.Log("Own Party Defeated");
                }
            }

        }

        public void MoveParty(WM_Actor party, LocationController location)
        {  
            Debug.Log("Move Party");
            party.TurnStateManager.HasMoved = true;
            party.location.Actor = null;
            party.location.Reset();
            party.location = location;
            location.Actor = party;


        }

        public void Wait(WM_Actor party)
        {
            Debug.Log("Wait Party");
            selectionSystem.DeselectActor();

            party.TurnStateManager.IsWaiting = true;
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
                
                party.Join(otherParty);
            }
        }

        private void SplitParty()
        {
            var actor = selectionSystem.SelectedActor;
            if (actor is Party party)
            {
                var splitParty=party.Split();
                partyInstantiator.InstantiateParty(splitParty, party.location.worldMapPosition);
                splitParty.GameTransformManager.SetInputReceiver(party.GameTransformManager.GetInputReceiver());
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