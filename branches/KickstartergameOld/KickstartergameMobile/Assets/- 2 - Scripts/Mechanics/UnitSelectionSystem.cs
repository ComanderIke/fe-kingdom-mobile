using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class UnitSelectionSystem : MonoBehaviour, EngineSystem
    {
        MainScript mainScript;
        public Unit SelectedCharacter { get; set; }


        void Start()
        {
            mainScript = MainScript.instance;
            InputSystem.onUnitClickedConfirmed += UnitClicked;
            InputSystem.onEndDragOverNothing += DeselectActiveCharacter;
            mainScript.GetSystem<UISystem>().onDeselectClicked.AddListener(DeselectActiveCharacter);
            //EventContainer.deselectButtonClicked += DeselectActiveCharacter;
        }
       
        void SameCharacterSelected(Unit c)
        {
            
            DeselectActiveCharacter();
         }
        public void DeselectActiveCharacter()
        {
            if (SelectedCharacter != null)
                SelectedCharacter.ResetPosition();
            if (SelectedCharacter != null)
                SelectedCharacter.UnitTurnState.Selected = false;
            UnitActionSystem.onDeselectCharacter();
            mainScript.GetSystem<UISystem>().HideAttackableField();
            SelectedCharacter = null;

            mainScript.GetSystem<global::MapSystem>().HideMovement();
            mainScript.GetSystem<UISystem>().ShowAllActiveUnitEffects();

        }
        void SelectCharacter(Unit c)
        {
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
            }
            
            SelectedCharacter = c;
            c.UnitTurnState.Selected = true;
            global::MapSystem s = mainScript.GetSystem<global::MapSystem>();
            mainScript.GetSystem<UISystem>().ShowTopUI(c);
            s.HideMovement();
            if (!SelectedCharacter.UnitTurnState.HasMoved)
            {
                s.ShowMovement(c);
                s.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            } 
            else
            {
                if(!SelectedCharacter.UnitTurnState.HasAttacked)
                    s.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            }
            mainScript.GetSystem<UISystem>().HideAllActiveUnitEffects();
            if (!SelectedCharacter.UnitTurnState.HasMoved)
                Unit.onUnitShowActiveEffect(SelectedCharacter, true, false);
            UnitActionSystem.onSelectedCharacter();
        }

        void EnemySelected(Unit c)
        {
            Debug.Log("enemy selected " + c.Name);
            global::MapSystem gridScript = mainScript.GetSystem<global::MapSystem>();
            Debug.Log(mainScript.name);
            Debug.Log(gridScript.name);
            gridScript.HideMovement();
            gridScript.ShowMovement(c);

            gridScript.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            mainScript.GetSystem<UISystem>().ShowTopUI(c);
            gridScript.GridLogic.ResetActiveFields();
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
                if (SelectedCharacter.GameTransform.GameObject != null)
                    SelectedCharacter = null;
            }
        }

        public void UnitClicked(Unit c, bool confirm)//TODO will be called by CHaracterClicked
        {
            if (SelectedCharacter != null && SelectedCharacter.GameTransform.GameObject != null && c != SelectedCharacter)
            {

                if (c.Player.ID != SelectedCharacter.Player.ID)//Clicked On Enemy
                {
                    if (confirm || mainScript.GetSystem<global::MapSystem>().GridLogic.GetAttackTargetsAtGameObjectPosition(SelectedCharacter).Contains(c))
                    {
                        SelectedCharacter.ResetPosition();
                        mainScript.GetSystem<UnitActionSystem>().GoToEnemy(SelectedCharacter, c, false);
                    }
                    else
                    {
                        InputSystem.onEnemyClicked(c);
                        
                    }
                    return;
                 }

            }
            if (mainScript.PlayerManager.ActivePlayer.Units.Contains(c))
            {
                if (!c.UnitTurnState.IsWaiting)
                {
                    if ( SelectedCharacter != null && SelectedCharacter == c)
                    {
                        SameCharacterSelected(c);
                    }
                    else
                    {
                        if (SelectedCharacter!=null)
                            SelectedCharacter.ResetPosition();
                        mainScript.GetSystem<InputSystem>().ResetAll();
                        mainScript.GetSystem<global::MapSystem>().HideMovement();
                        SelectCharacter(c);
                    }
                }
                else //if (c.UnitTurnState.HasMoved && !c.UnitTurnState.HasAttacked && c.)
                {

                    mainScript.GetSystem<UISystem>().ShowTopUI(c);
                    if (SelectedCharacter == null)
                    {
                        mainScript.GetSystem<InputSystem>().ResetAll();
                        mainScript.GetSystem<global::MapSystem>().HideMovement();
                        global::MapSystem s = mainScript.GetSystem<global::MapSystem>();
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
