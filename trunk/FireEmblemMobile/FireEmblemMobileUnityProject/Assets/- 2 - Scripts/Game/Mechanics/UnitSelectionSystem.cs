using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameInput;
using Assets.GUI;
using System.Collections.Generic;
using Assets.GameActors.Units.Humans;
using UnityEngine;
using Assets.Map;
using System;
using Assets.GameEngine;
using Assets.Game.Manager;
using Assets.Game.GameStates;

namespace Assets.Mechanics
{
    public class UnitSelectionSystem : MonoBehaviour, IEngineSystem
    {
        public static event Action OnDeselectCharacter;

        public static event Action<Unit> OnSelectedCharacter;

        public static event Action<Unit> OnSelectedInActiveCharacter;

        public static event Action<Unit> OnEnemySelected;

        private GridGameManager gridGameManager;
        public Unit SelectedCharacter { get; set; }

        private void Start()
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

        private void SelectCharacter(Unit c)
        {
            if (SelectedCharacter != null)
            {
                DeselectActiveCharacter();
            }
            SelectedCharacter = c;
            c.UnitTurnState.Selected = true;
            OnSelectedCharacter(SelectedCharacter);
        }

        private void EnemySelected(Unit c)
        {
            Debug.Log("enemy selected " + c.Name);
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
                if (SelectedCharacter.GameTransform.GameObject != null)
                    SelectedCharacter = null;
            }
            OnEnemySelected?.Invoke(c);
        }

        private void SelectInActiveCharacter(Unit c)
        {
            Debug.Log("SelectInactiveCharacter");
            OnSelectedInActiveCharacter?.Invoke(c);
        }
        private void SelectUnit(Unit c)
        {
            if (gridGameManager.FactionManager.ActiveFaction.Units.Contains(c))
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