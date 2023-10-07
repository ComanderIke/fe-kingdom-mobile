using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIGodBlessing : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image face;
        [SerializeField] private GameObject selected;
        [SerializeField] private GameObject blessed;
        [SerializeField] private TextMeshProUGUI lv;
        [SerializeField] private ExpBarController expBarController;
        public event Action<int> onClicked;

        [SerializeField]
        private Vector3 normalScale;
        [SerializeField]
        private Vector3 selectedScale;

        private int index = 0;
        public void Show(Unit unit, God god, int index)
        {
            name.SetText(god.name);
            this.index = index;
            face.sprite = god.Face;
            lv.SetText("Lv. "+unit.Bonds.GetBondLevel(god));
            expBarController.UpdateInstant(unit.Bonds.GetBondExperience(god));
            blessed.gameObject.SetActive(unit.Blessing!=null && unit.Blessing.God==god);
            transform.localScale = normalScale;
        }

        public void Select()
        {
            selected.gameObject.SetActive(true);
            transform.localScale = selectedScale;
        }

        public void Deselect()
        {
            selected.gameObject.SetActive(false);
            transform.localScale = normalScale;
        }

        public void Clicked()
        {
            onClicked?.Invoke(index);
        }
    }
}
