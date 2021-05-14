using System;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using GameEngine;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public class WM_PartySelectionSystem:IEngineSystem
    {
        public static event Action OnDeselectParty;

        public static event Action<WM_Actor> OnSelectedParty;

        public static event Action<WM_Actor> OnSelectedInActiveParty;

        public static event Action<WM_Actor> OnEnemyPartySelected;
        public WM_Actor SelectedActor { get; set; }
        private FactionManager factionManager;

        public WM_PartySelectionSystem(FactionManager factionManager)
        {
            this.factionManager = factionManager;
            Debug.Log("Setup Events for PartySelection");
            TurnSystem.OnEndTurn += DeselectActor;
        }
        private void SameCharacterSelected()
        {
            DeselectActor();
            //SelectCharacter(SelectedActor);
        }

        public void DeselectActor()
        {
            if (SelectedActor != null)
            {
                SelectedActor.ResetPosition();
                SelectedActor.TurnStateManager.IsSelected = false;
            }
            Debug.Log("DeselectActor");
            OnDeselectParty?.Invoke();
            SelectedActor = null;
        }

        private void SelectCharacter(WM_Actor c)
            {
                Debug.Log("SELECT CHARACTER "+c);
                if (SelectedActor != null)
                {
                    DeselectActor();
                }
                SelectedActor = c;
                c.TurnStateManager.IsSelected = true;
                OnSelectedParty?.Invoke(SelectedActor);
            }

            private void EnemySelected(WM_Actor c)
            {
                Debug.Log("enemy selected " + c);
                if (SelectedActor != null)
                {
                    SelectedActor.TurnStateManager.IsSelected = false;
                }
                OnEnemyPartySelected?.Invoke(c);
            }
            private void SelectInActiveCharacter(WM_Actor c)
            {
                Debug.Log("SelectInactiveCharacter");
                OnSelectedInActiveParty?.Invoke(c);
            }
            public void SelectParty(WM_Actor c)
            {
                if (factionManager.IsActiveFaction(c.Faction))
                {
                    if (!c.TurnStateManager.IsWaiting)
                    {
                        if (SelectedActor != null && SelectedActor == c)
                        {
                            SameCharacterSelected();
                        }
                        else
                        {
                            SelectCharacter(c);
                        }
                    }
                    else //if (c.UnitTurnState.HasMoved && !c.UnitTurnState.HasAttacked && c.)
                    {
                        SelectInActiveCharacter(c);
                    }
                }
                else
                {
                    EnemySelected(c);
                }
            }

            public void Init()
            {
                
            }
    }
}