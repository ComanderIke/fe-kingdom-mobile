using System;
using System.Collections.Generic;
using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.GUI.CharacterScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI selectedUnitCount;
        [SerializeField] private Transform characterContainer;

        [SerializeField] private GameObject selectableCharacterPrefab;

        [SerializeField] private UICharacterViewController characterView;
        private List<Unit> selectableUnits;
        private List<SelectableCharacterUI> selectableCharacterUis;
        private List<Unit> unlockedUnits;
        public static SelectableCharacterUI lastSelected;
        public void Show(List<Unit> selectableUnits)
        {
            lastSelected = null;
            this.selectableUnits = selectableUnits;
            int cnt = 0;
            selectableCharacterUis = new List<SelectableCharacterUI>();
            unlockedUnits = new List<Unit>();
            foreach (var unit in selectableUnits)
            {
                if(Player.Instance.UnlockedCharacterIds.Contains(unit.bluePrintID))
                    unlockedUnits.Add(unit);
            }

            foreach (var unit in selectableUnits)
            {
                var go = Instantiate(selectableCharacterPrefab, characterContainer);
                var last = go.GetComponent<SelectableCharacterUI>();
                // Debug.Log(unit.bluePrintID);
                // Debug.Log(Player.Instance.UnlockedCharacterIds.Count);
                last.SetCharacter(unit,!Player.Instance.UnlockedCharacterIds.Contains(unit.bluePrintID));
                last.onClicked += UnitClicked;
                if (cnt == 0)
                {
                    // Debug.Log("Select Last: "+last.unit);
                    //AddUnit(last);
                    SelectUnit(last);
                    characterView.Show(last.unit);
                }

                cnt++;
                selectableCharacterUis.Add(last);

            }

            UpdatePartySizeText();
           
        }

        [SerializeField] private float scrollNormalizedOffset;
        [SerializeField] private ScrollRect scrollRect;
        
        public void LeftClicked()
        {
           // characterContainer.Translate(-arrowXDistance,0,0);
           scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition-scrollNormalizedOffset);
        }

       

        public void RightClicked()
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition+scrollNormalizedOffset);
        }
        void UpdatePartySizeText()
        {
            selectedUnitCount.text = "Party Size "+ Player.Instance.Party.members.Count+"/"+Player.Instance.startPartyMemberCount;
        }
        public void AddUnit(SelectableCharacterUI unit)
        {
            // Debug.Log(Player.Instance.Party.IsFull()+" "+Player.Instance.Party.members.Count+" "+Player.Instance.startPartyMemberCount);
            if (Player.Instance.Party.IsFull()||Player.Instance.Party.members.Count>= Player.Instance.startPartyMemberCount)
                return;
            // Debug.Log("Before Add");
            Player.Instance.Party.AddMember(unit.unit);
            SelectUnit(unit);
            // Debug.Log("Before Select");
            unit.Add();
            // Debug.Log("Actually Selected: "+unit.unit);
            
            UpdatePartySizeText();
            if (Player.Instance.Party.IsFull()||Player.Instance.Party.members.Count>= Player.Instance.startPartyMemberCount)
            {
                foreach (var selectableCharacter in selectableCharacterUis)
                {
                    if(!Player.Instance.UnlockedCharacterIds.Contains(selectableCharacter.unit.bluePrintID))
                        continue;
                    if(!selectableCharacter.IsSelected())
                        selectableCharacter.SetInteractable(false);
                    else
                        selectableCharacter.SetInteractable(true);
                }
            }
            else
            {
                SetAllInteractable();
            }
        }
        public void RemoveUnit(SelectableCharacterUI unit)
        {
            Player.Instance.Party.RemoveMember(unit.unit);
            unit.Remove();
            UpdatePartySizeText();
            SetAllInteractable();
            Debug.Log("Actually Deselect: "+unit.unit);

        }

        private void SetAllInteractable()
        {
            foreach (var selectableCharacter in selectableCharacterUis)
            {
                if(!Player.Instance.UnlockedCharacterIds.Contains(selectableCharacter.unit.bluePrintID))
                    continue;
                selectableCharacter.SetInteractable(true);
            }
        }

        public void AddRemoveUnit()
        {
            if (Player.Instance.Party.members.Contains(lastSelected.unit))
            {
                // Debug.Log("Deselect: "+lastSelected.unit);
                RemoveUnit(lastSelected);
            }
            else
            {
                // Debug.Log("Select: "+lastSelected.unit);
                AddUnit(lastSelected);
            }
            if(Player.Instance.Party.members.Contains(lastSelected.unit))
                addRemoveButton.ShowRemove();
            else
            {
                addRemoveButton.ShowAdd();
            }
        }

        void SelectUnit(SelectableCharacterUI unit)
        {
            MyDebug.LogLogic("Char Selector: Select "+unit.unit);
            if(lastSelected!=null)
                lastSelected.Deselect();
            lastSelected = unit;
            unit.Select();
            if (Player.Instance.Party.members.Contains(unit.unit))
            {
                addRemoveButton.ShowRemove();
                //Player.Instance.Party.SetActiveUnit(unit.unit);
            }
            else
            {
                addRemoveButton.ShowAdd();
            }
            OnUnitSelected?.Invoke(unit.unit);
            
        }
     
        [SerializeField] private UIAddRemoveButton addRemoveButton;
        public void UnitClicked(SelectableCharacterUI unit)
        {
            SelectUnit(unit);
            // Debug.Log("Show unit: "+unit.unit);
            characterView.Show(unit.unit);
            
        }

        public event Action<Unit> OnUnitSelected;

        public void UpdateUI()
        {
            foreach (var unit in selectableCharacterUis)
            {
                if (Player.Instance.Party.ActiveUnit != null&&Player.Instance.Party.ActiveUnit.Equals(unit.unit))
                {
                    if(lastSelected!=unit)
                        SelectUnit(unit);
                }
            }
            
        }
    }
}
