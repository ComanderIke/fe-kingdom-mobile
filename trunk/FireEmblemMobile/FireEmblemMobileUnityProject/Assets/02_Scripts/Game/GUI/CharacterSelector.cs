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

        private List<Unit> selectableUnits;
        public void Show(List<Unit> selectableUnits)
        {
            this.selectableUnits = selectableUnits;
            foreach (var unit in selectableUnits)
            {
                var go = Instantiate(selectableCharacterPrefab, characterContainer);
                go.GetComponent<SelectableCharacterUI>().SetCharacter(unit);
                go.GetComponent<SelectableCharacterUI>().onClicked += UnitClicked;
            }
            Select(selectableUnits[0]);
        }

        public void Select(Unit unit)
        {
            Player.Instance.Party.AddMember(unit);
        }
        public void Deselect(Unit unit)
        {
            Player.Instance.Party.RemoveMember(unit);
        }

        public void UnitClicked(Unit unit)
        {
            if (Player.Instance.Party.members.Contains(unit))
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
