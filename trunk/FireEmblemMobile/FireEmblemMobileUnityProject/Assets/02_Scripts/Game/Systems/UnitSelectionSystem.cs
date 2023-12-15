using System;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
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

        public static event Action<IGridActor> OnEnemyDeselected;
        public static event Action<IGridActor> OnEnemySelected;
        public static event Action<Skill> OnSkillSelected;
        public static event Action<Skill> OnSkillDeselected;
        public static event Action<Item> OnItemSelected;

        private GridGameManager gridGameManager;
        public IGridActor SelectedCharacter { get; set; }
        public IGridActor SelectedEnemy { get; set; }
        public Skill SelectedSkill { get; set; }
        public Item SelectedItem { get; set; }

        private GameplayCommands gameplayCommands;
        private ISelectionUI selectionUI;
        

        public void Init()
        {
            gameplayCommands = new GameplayCommands();
            gridGameManager = GridGameManager.Instance;
            selectionUI = GameObject.FindObjectOfType<SelectionUI>();
           
            //BattleState.OnExit += DeselectActiveCharacter;
        }

        private void OnDisable()
        {
            Deactivate();
        }

        public void Deactivate()
        {
            GameplayCommands.OnSelectSkill -= SelectSkill;
            GameplayCommands.OnDeselectSkill -= DeselectSkill;
            GameplayCommands.OnSelectItem -= SelectItem;
            GameplayCommands.OnDeselectItem -= DeselectItem;
            GameplayCommands.OnSelectUnit -= SelectUnit;
            GameplayCommands.OnDeselectUnit -= DeselectActiveCharacter;
            gridGameManager.GetSystem<TurnSystem>().OnEndTurn -= EndTurnDeselectUnit;
        }

        public void Activate()
        {
            GameplayCommands.OnSelectSkill += SelectSkill;
            GameplayCommands.OnDeselectSkill += DeselectSkill;
            GameplayCommands.OnSelectItem += SelectItem;
            GameplayCommands.OnDeselectItem += DeselectItem;
            GameplayCommands.OnSelectUnit += SelectUnit;
            GameplayCommands.OnDeselectUnit += DeselectActiveCharacter;
            gridGameManager.GetSystem<TurnSystem>().OnEndTurn += EndTurnDeselectUnit;
        }

        void EndTurnDeselectUnit()
        {
            DeselectActiveCharacter(SelectedCharacter);
        }
        private void SelectSkill(Skill skill)
        {
            SelectedSkill = skill;
            OnSkillSelected?.Invoke(skill);
        }
        private void DeselectSkill()
        {
            OnSkillDeselected?.Invoke(SelectedSkill);
            SelectedSkill = null;
           
        }
        private void SelectItem(Item item)
        {
            Debug.Log("Select Item: "+item.Name);
            SelectedItem = item;
            OnItemSelected?.Invoke(item);
        }
        private void DeselectItem()
        {
            SelectedItem = null;
        }


        public void Update()
        {
            
        }

        private void SameCharacterSelected()
        {
            SelectCharacter(SelectedCharacter);
        }

        public void DeselectActiveCharacter(IGridActor unit)
        {
           // Debug.Log("DeselectACtiveCharacterGridSys");
            if (SelectedCharacter != null)
            {
                SelectedCharacter.GridComponent.ResetPosition();
                SelectedCharacter.TurnStateManager.IsSelected = false;
                // if (SelectedCharacter.TurnStateManager.HasMoved)
                // {
                //     gameplayCommands.Wait(SelectedCharacter);
                //     gameplayCommands.ExecuteInputActions(null);
                // }
            }
            OnDeselectCharacter?.Invoke(SelectedCharacter);
            SelectedCharacter = null;
        }

        private void SelectCharacter(IGridActor c)
        {
           // Debug.Log("Select Character!"+c);
            if (SelectedCharacter != null)
            {
              //  Debug.Log("DeSelect Character!" +SelectedCharacter);
                DeselectActiveCharacter(SelectedCharacter);
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
            if (SelectedEnemy != null)
            {
                DeselectEnemy(SelectedEnemy);
            }
            SelectedEnemy = c;
            //c.TurnStateManager.IsSelected = true;
            OnEnemySelected?.Invoke(c);
        }

        void DeselectEnemy(IGridActor enemy)
        {
            
            OnEnemyDeselected?.Invoke(SelectedEnemy);
            SelectedEnemy = null;
        }

        private void SelectInActiveCharacter(IGridActor c)
        {
            Debug.Log("SelectInactiveCharacter");
            OnSelectedInActiveCharacter?.Invoke(c);
        }
        private void SelectUnit(IGridActor c)
        {
            //Debug.Log(c.Faction.Id+" X "+gridGameManager.FactionManager.ActiveFaction.Id);
           // Debug.Log("IsActive: "+gridGameManager.FactionManager.IsActiveFaction(c.Faction));
            if (gridGameManager.FactionManager.IsActiveFaction(c.Faction))
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