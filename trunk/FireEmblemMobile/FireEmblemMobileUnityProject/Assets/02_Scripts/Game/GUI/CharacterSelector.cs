using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Game.GameActors.Players;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private Transform characterContainer;

        [SerializeField] private GameObject selectableCharacterPrefab;

        [SerializeField] private UICharacterViewController characterView;
        private List<Unit> selectableUnits;
        public void Show(List<Unit> selectableUnits)
        {
            this.selectableUnits = selectableUnits;
            int cnt = 0;
            foreach (var unit in selectableUnits)
            {
                var go = Instantiate(selectableCharacterPrefab, characterContainer);
                var last = go.GetComponent<SelectableCharacterUI>();
                last.SetCharacter(unit);
                last.onClicked += UnitClicked;
                if(cnt==0)
                    Select(last);
                cnt++;

            }
           
        }

        public void Select(SelectableCharacterUI unit)
        {
            Player.Instance.Party.AddMember(unit.unit);
            unit.Select();
            characterView.Show(unit.unit);
        }
        public void Deselect(SelectableCharacterUI unit)
        {
            Player.Instance.Party.RemoveMember(unit.unit);
            unit.Deselect();
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
