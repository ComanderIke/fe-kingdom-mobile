using System;
using System.Linq;
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

        public void AttackParty(WM_Actor party)
        {
            var selected = selectionSystem.SelectedActor;
            Debug.Log(selectionSystem.SelectedActor+" "+selectionSystem.SelectedActor.name);
            if (selected is Party pty)
            {
                WorldMapSceneController.Instance.LoadLevel(pty, (Party)party);
            }
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

        private void JoinParty()
        {
            var actor = selectionSystem.SelectedActor;
            if (actor is Party party)
            {
                Party otherParty = null;
                foreach (var locActor in party.location.Actors)
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
            }
        }
        private void PartySelected(WM_Actor actor)
        {
            Debug.Log("Party Selected");
            if (actor is Party party)
            {
                if (party.members.Count >= 1 && party.location.HasSpace())
                {
                    Debug.Log("Show Split");
                    partyActionRenderer.ShowSplitButton();
                }
                else
                {
                    Debug.Log("Hide Split ??");
                    partyActionRenderer.HideSplitButton();
                }

                if (party.location.Actors.Select(a=> a.Faction.Id==party.Faction.Id).Count()==2)
                {
                    
                    partyActionRenderer.ShowJoinButton();
                }
                else
                {
                    partyActionRenderer.HideJoinButton();
                }
            }
            else
            {
                partyActionRenderer.HideJoinButton();
                Debug.Log("Hide Split 222??");
                partyActionRenderer.HideSplitButton();
            }
            
            
        }
        private void PartyDeselected()
        {
            Debug.Log("PartyDeselected");
            partyActionRenderer.HideJoinButton();
            partyActionRenderer.HideSplitButton();
        }
        public void Init()
        {
            
        }

       

        public void Deactivate()
        {
            OnJoinClicked -= JoinParty;
            OnSplitClicked -= SplitParty;
            WM_PartySelectionSystem.OnSelectedParty -= PartySelected;
            WM_PartySelectionSystem.OnDeselectParty -= PartyDeselected;
        }

        public void Activate()
        {
            OnJoinClicked += JoinParty;
            OnSplitClicked += SplitParty;
            WM_PartySelectionSystem.OnSelectedParty += PartySelected;
            WM_PartySelectionSystem.OnDeselectParty += PartyDeselected;
        }
    }
}