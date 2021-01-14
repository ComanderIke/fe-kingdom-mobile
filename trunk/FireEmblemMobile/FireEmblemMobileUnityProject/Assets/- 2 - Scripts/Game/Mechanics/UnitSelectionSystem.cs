using System;
using Game.GameInput;
using Game.Manager;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class UnitSelectionSystem : MonoBehaviour, IEngineSystem
    {
        public static event Action OnDeselectCharacter;

        public static event Action<ISelectableActor> OnSelectedCharacter;

        public static event Action<ISelectableActor> OnSelectedInActiveCharacter;

        public static event Action<ISelectableActor> OnEnemySelected;

        private GridGameManager gridGameManager;
        public ISelectableActor SelectedCharacter { get; set; }

        public void Init()
        {
            gridGameManager = GridGameManager.Instance;

            GameplayInput.OnSelectUnit += SelectUnit;
            GameplayInput.OnDeselectUnit += DeselectActiveCharacter;
            TurnSystem.OnEndTurn += DeselectActiveCharacter;
            BattleState.OnExit += DeselectActiveCharacter;
        }

        private void SameCharacterSelected()
        {
            SelectCharacter(SelectedCharacter);
        }

        public void DeselectActiveCharacter()
        {
            if (SelectedCharacter != null)
            {
                SelectedCharacter.ResetPosition();
                SelectedCharacter.UnitTurnState.Selected = false;
            }
            OnDeselectCharacter();
            SelectedCharacter = null;
        }

        private void SelectCharacter(ISelectableActor c)
        {
            if (SelectedCharacter != null)
            {
                DeselectActiveCharacter();
            }
            SelectedCharacter = c;
            c.UnitTurnState.Selected = true;
            OnSelectedCharacter(SelectedCharacter);
        }

        private void EnemySelected(ISelectableActor c)
        {
            Debug.Log("enemy selected " + c);
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
            }
            OnEnemySelected?.Invoke(c);
        }

        private void SelectInActiveCharacter(ISelectableActor c)
        {
            Debug.Log("SelectInactiveCharacter");
            OnSelectedInActiveCharacter?.Invoke(c);
        }
        private void SelectUnit(ISelectableActor c)
        {
            if (gridGameManager.FactionManager.IsActiveFaction(c.FactionId))
            {
                if (!c.UnitTurnState.IsWaiting)
                {
                    if (SelectedCharacter != null && SelectedCharacter == c)
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