using System;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Model.Battle;
using Game.WorldMapStuff.Systems;
using GameEngine;
using ICSharpCode.NRefactory.Ast;
using Menu;
using UnityEngine;

namespace Game.WorldMapStuff.Input
{
    public class WM_PartyActionSystem:IEngineSystem
    {
        private WM_PreviewSystem previewSystem;
        private WM_PartySelectionSystem selectionSystem;

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
                BattleTransferData.Instance.PlayerName = pty.Faction.name;
                BattleTransferData.Instance.UnitsGoingIntoBattle = pty.members;
                if(party is Party enemyParty)
                    BattleTransferData.Instance.EnemyUnits = enemyParty.members;    

                SceneController.SwitchScene("Level2");
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

        public void Init()
        {
            
        }
    }
}