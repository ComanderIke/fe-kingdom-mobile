using System;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Manager;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class UnitSelectionSystem : MonoBehaviour, IEngineSystem
    {
        public static event Action<IGridActor> OnDeselectCharacter;

        public static event Action<IGridActor> OnSelectedCharacter;

        public static event Action<IGridActor> OnSelectedInActiveCharacter;

        public static event Action<IGridActor> OnEnemySelected;

        private GridGameManager gridGameManager;
        public IGridActor SelectedCharacter { get; set; }

        public void Init()
        {
            gridGameManager = GridGameManager.Instance;

           
            //BattleState.OnExit += DeselectActiveCharacter;
        }

        public void Deactivate()
        {
            GameplayInput.OnSelectUnit -= SelectUnit;
            GameplayInput.OnDeselectUnit -= DeselectActiveCharacter;
            TurnSystem.OnEndTurn -= DeselectActiveCharacter;
        }

        public void Activate()
        {
            GameplayInput.OnSelectUnit += SelectUnit;
            GameplayInput.OnDeselectUnit += DeselectActiveCharacter;
            TurnSystem.OnEndTurn += DeselectActiveCharacter;
        }

        public void Update()
        {
            
        }

        private void SameCharacterSelected()
        {
            SelectCharacter(SelectedCharacter);
        }

        public void DeselectActiveCharacter()
        {
            Debug.Log("DeselectACtiveCharacterGridSys");
            if (SelectedCharacter != null)
            {
                SelectedCharacter.GridComponent.ResetPosition();
                SelectedCharacter.TurnStateManager.IsSelected = false;
            }
            OnDeselectCharacter?.Invoke(SelectedCharacter);
            SelectedCharacter = null;
        }

        private void SelectCharacter(IGridActor c)
        {
            Debug.Log("SELECT CHARACTER "+c);
            if (SelectedCharacter != null)
            {
                DeselectActiveCharacter();
            }
            SelectedCharacter = c;
            c.TurnStateManager.IsSelected = true;
            
            OnSelectedCharacter?.Invoke(SelectedCharacter);
        }

        private void EnemySelected(IGridActor c)
        {
            Debug.Log("enemy selected " + c);
            if (SelectedCharacter != null)
            {
                SelectedCharacter.TurnStateManager.IsSelected = false;
            }
            OnEnemySelected?.Invoke(c);
        }

        private void SelectInActiveCharacter(IGridActor c)
        {
            Debug.Log("SelectInactiveCharacter");
            OnSelectedInActiveCharacter?.Invoke(c);
        }
        private void SelectUnit(IGridActor c)
        {
            if (gridGameManager.FactionManager.IsActiveFaction(c.Faction.Id))
            {
                if (!c.TurnStateManager.IsWaiting)
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