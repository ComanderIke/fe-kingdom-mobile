using System;
using Game.Manager;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public class WM_PartySelectionSystem
    {
        public static event Action OnDeselectParty;

        public static event Action<IWM_Actor> OnSelectedParty;

        public static event Action<IWM_Actor> OnSelectedInActiveParty;

        public static event Action<IWM_Actor> OnEnemyPartySelected;
        public IWM_Actor SelectedActor { get; set; }
        private FactionManager factionManager;

        public WM_PartySelectionSystem(FactionManager factionManager)
        {
            this.factionManager = factionManager;
        }
        private void SameCharacterSelected()
        {
            SelectCharacter(SelectedActor);
        }

        public void DeselectActor()
        {
            if (SelectedActor != null)
            {
                SelectedActor.ResetPosition();
                SelectedActor.TurnStateManager.IsSelected = false;
            }

            OnDeselectParty?.Invoke();
            SelectedActor = null;
        }

        private void SelectCharacter(IWM_Actor c)
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

            private void EnemySelected(IWM_Actor c)
            {
                Debug.Log("enemy selected " + c);
                if (SelectedActor != null)
                {
                    SelectedActor.TurnStateManager.IsSelected = false;
                }
                OnEnemyPartySelected?.Invoke(c);
            }
            private void SelectInActiveCharacter(IWM_Actor c)
            {
                Debug.Log("SelectInactiveCharacter");
                OnSelectedInActiveParty?.Invoke(c);
            }
            public void SelectParty(IWM_Actor c)
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

    }
}