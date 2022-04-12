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

        public static event Action<IGridActor> OnEnemySelected;
        public static event Action<Skill> OnSkillSelected;
        public static event Action<Item> OnItemSelected;

        private GridGameManager gridGameManager;
        public IGridActor SelectedCharacter { get; set; }
        public Skill SelectedSkill { get; set; }
        public Item SelectedItem { get; set; }

        private GameplayInput gameplayInput;
        private ISelectionUI selectionUI;
        

        public void Init()
        {
            gameplayInput = new GameplayInput();
            gridGameManager = GridGameManager.Instance;
            selectionUI = GameObject.FindObjectOfType<SelectionUI>();
           
            //BattleState.OnExit += DeselectActiveCharacter;
        }

        public void Deactivate()
        {
            GameplayInput.OnSelectSkill -= SelectSkill;
            GameplayInput.OnDeselectSkill -= DeselectSkill;
            GameplayInput.OnSelectItem -= SelectItem;
            GameplayInput.OnDeselectItem -= DeselectItem;
            GameplayInput.OnSelectUnit -= SelectUnit;
            GameplayInput.OnDeselectUnit -= DeselectActiveCharacter;
            gridGameManager.GetSystem<TurnSystem>().OnEndTurn -= DeselectActiveCharacter;
        }

        public void Activate()
        {
            GameplayInput.OnSelectSkill += SelectSkill;
            GameplayInput.OnDeselectSkill += DeselectSkill;
            GameplayInput.OnSelectItem += SelectItem;
            GameplayInput.OnDeselectItem += DeselectItem;
            GameplayInput.OnSelectUnit += SelectUnit;
            GameplayInput.OnDeselectUnit += DeselectActiveCharacter;
            gridGameManager.GetSystem<TurnSystem>().OnEndTurn += DeselectActiveCharacter;
        }

        private void SelectSkill(Skill skill)
        {
            SelectedSkill = skill;
            OnSkillSelected?.Invoke(skill);
        }
        private void DeselectSkill()
        {
            SelectedSkill = null;
        }
        private void SelectItem(Item item)
        {
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

        public void DeselectActiveCharacter()
        {
           // Debug.Log("DeselectACtiveCharacterGridSys");
            if (SelectedCharacter != null)
            {
                SelectedCharacter.GridComponent.ResetPosition();
                SelectedCharacter.TurnStateManager.IsSelected = false;
                if (SelectedCharacter.TurnStateManager.HasMoved)
                {
                    gameplayInput.Wait(SelectedCharacter);
                    gameplayInput.ExecuteInputActions(null);
                }
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