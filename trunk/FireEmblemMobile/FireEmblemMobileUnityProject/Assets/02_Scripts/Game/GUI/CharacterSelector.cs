using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI selectedUnitCount;
        [SerializeField] private Transform characterContainer;

        [SerializeField] private GameObject selectableCharacterPrefab;

        [SerializeField] private UICharacterViewController characterView;
        private List<Unit> selectableUnits;
        private List<SelectableCharacterUI> selectableCharacterUis;
        public void Show(List<Unit> selectableUnits)
        {
            this.selectableUnits = selectableUnits;
            int cnt = 0;
            selectableCharacterUis = new List<SelectableCharacterUI>();
            foreach (var unit in selectableUnits)
            {
                var go = Instantiate(selectableCharacterPrefab, characterContainer);
                var last = go.GetComponent<SelectableCharacterUI>();
                last.SetCharacter(unit);
                last.onClicked += UnitClicked;
                if(cnt==0)
                    Select(last);
                cnt++;
                selectableCharacterUis.Add(last);

            }

            UpdatePartySizeText();

        }

        void UpdatePartySizeText()
        {
            selectedUnitCount.text = "Party Size "+ Player.Instance.Party.members.Count+"/"+Player.Instance.startPartyMemberCount;
        }
        public void Select(SelectableCharacterUI unit)
        {
            if (Player.Instance.Party.IsFull()||Player.Instance.Party.members.Count>= Player.Instance.startPartyMemberCount)
                return;
            Player.Instance.Party.AddMember(unit.unit);
            unit.Select();
            characterView.Show(unit.unit);
            UpdatePartySizeText();
            if (Player.Instance.Party.IsFull()||Player.Instance.Party.members.Count>= Player.Instance.startPartyMemberCount)
            {
                foreach (var selectableCharacter in selectableCharacterUis)
                {
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
        public void Deselect(SelectableCharacterUI unit)
        {
            Player.Instance.Party.RemoveMember(unit.unit);
            unit.Deselect();
            UpdatePartySizeText();
            SetAllInteractable();

        }

        private void SetAllInteractable()
        {
            foreach (var selectableCharacter in selectableCharacterUis)
            {
                selectableCharacter.SetInteractable(true);
            }
        }

        public void UnitClicked(SelectableCharacterUI unit)
        {
            if (Player.Instance.Party.members.Contains(unit.unit))
            {
                Deselect(unit);
            }
            else
            {
                Select(unit);
            }
        }
    }
}
