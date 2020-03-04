using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameInput;
using Assets.GUI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Mechanics
{
    public class UnitSelectionSystem : MonoBehaviour, IEngineSystem
    {
        private MainScript mainScript;
        public Unit SelectedCharacter { get; set; }

        private void Start()
        {
            mainScript = MainScript.Instance;
            InputSystem.OnUnitClickedConfirmed += UnitClicked;
            InputSystem.OnEndDragOverNothing += DeselectActiveCharacter;
            UiSystem.OnDeselectButtonClicked+=DeselectActiveCharacter;
            //EventContainer.deselectButtonClicked += DeselectActiveCharacter;
        }

        private void SameCharacterSelected(Unit c)
        {
            DeselectActiveCharacter();
        }

        public void DeselectActiveCharacter()
        {
            if (SelectedCharacter != null)
                SelectedCharacter.ResetPosition();
            if (SelectedCharacter != null)
                SelectedCharacter.UnitTurnState.Selected = false;
            UnitActionSystem.OnDeselectCharacter();
            mainScript.GetSystem<UiSystem>().HideAttackableField();
            SelectedCharacter = null;

            mainScript.GetSystem<Map.MapSystem>().HideMovement();
            mainScript.GetSystem<UiSystem>().ShowAllActiveUnitEffects();
        }

        private void SelectCharacter(Unit c)
        {
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
            }

            SelectedCharacter = c;
            c.UnitTurnState.Selected = true;
            var s = mainScript.GetSystem<Map.MapSystem>();
            mainScript.GetSystem<UiSystem>().ShowTopUi(c);
            s.HideMovement();
            if (!SelectedCharacter.UnitTurnState.HasMoved)
            {
                s.ShowMovement(c);
                s.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            }
            else
            {
                if (!SelectedCharacter.UnitTurnState.HasAttacked)
                    s.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            }

            mainScript.GetSystem<UiSystem>().HideAllActiveUnitEffects();
            if (!SelectedCharacter.UnitTurnState.HasMoved)
                Unit.UnitShowActiveEffect(SelectedCharacter, true, false);
            UnitActionSystem.OnSelectedCharacter();
        }

        private void EnemySelected(Unit c)
        {
            Debug.Log("enemy selected " + c.Name);
            var gridScript = mainScript.GetSystem<Map.MapSystem>();
            Debug.Log(mainScript.name);
            Debug.Log(gridScript.name);
            gridScript.HideMovement();
            gridScript.ShowMovement(c);

            gridScript.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            mainScript.GetSystem<UiSystem>().ShowTopUi(c);
            gridScript.GridLogic.ResetActiveFields();
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
                if (SelectedCharacter.GameTransform.GameObject != null)
                    SelectedCharacter = null;
            }
        }

        public void UnitClicked(Unit c, bool confirm) //TODO will be called by CHaracterClicked
        {
            if (SelectedCharacter != null && SelectedCharacter.GameTransform.GameObject != null &&
                c != SelectedCharacter)
            {
                if (c.Player.Id != SelectedCharacter.Player.Id) //Clicked On Enemy
                {
                    if (confirm || mainScript.GetSystem<Map.MapSystem>().GridLogic
                        .GetAttackTargetsAtGameObjectPosition(SelectedCharacter).Contains(c))
                    {
                        SelectedCharacter.ResetPosition();
                        mainScript.GetSystem<UnitActionSystem>().GoToEnemy(SelectedCharacter, c, false);
                    }
                    else
                    {
                        InputSystem.OnEnemyClicked(c);
                    }

                    return;
                }
            }

            if (mainScript.PlayerManager.ActivePlayer.Units.Contains(c))
            {
                if (!c.UnitTurnState.IsWaiting)
                {
                    if (SelectedCharacter != null && SelectedCharacter == c)
                    {
                        SameCharacterSelected(c);
                    }
                    else
                    {
                        if (SelectedCharacter != null)
                            SelectedCharacter.ResetPosition();
                        mainScript.GetSystem<InputSystem>().ResetAll();
                        mainScript.GetSystem<Map.MapSystem>().HideMovement();
                        SelectCharacter(c);
                    }
                }
                else //if (c.UnitTurnState.HasMoved && !c.UnitTurnState.HasAttacked && c.)
                {
                    mainScript.GetSystem<UiSystem>().ShowTopUi(c);
                    if (SelectedCharacter == null)
                    {
                        mainScript.GetSystem<InputSystem>().ResetAll();
                        mainScript.GetSystem<Map.MapSystem>().HideMovement();
                        var s = mainScript.GetSystem<Map.MapSystem>();
                        s.ShowMovement(c);
                        s.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
                    }

                    //if (SelectedCharacter != null)
                    //    SelectedCharacter.ResetPosition();
                    //mainScript.GetSystem<MouseManager>().ResetAll();
                    //mainScript.gridManager.HideMovement();
                    //DeselectActiveCharacter();
                    ////SelectedCharacter = c;
                    //SelectCharacter(c);
                    //mainScript.gridManager.ShowAttackRange(c);
                }
            }
            else
            {
                SelectedCharacter = null;
                EnemySelected(c);
            }
        }
    }
}