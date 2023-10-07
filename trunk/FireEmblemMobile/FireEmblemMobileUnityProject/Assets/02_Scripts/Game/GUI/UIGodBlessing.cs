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
    public class God
    {
        public string name;
        public Sprite face;
        public Sprite Body;
    }
    public class UIGodBlessing : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image face;
        [SerializeField] private GameObject selected;
        [SerializeField] private GameObject blessed;
        [SerializeField] private TextMeshProUGUI lv;
        [SerializeField] private ExpBarController expBarController;
        public event Action onClicked;

        public void Show(Unit unit, God god)
        {
            name.SetText(god.name);
            face.sprite = god.face;
            lv.SetText("Lv. "+unit.Bonds.GetBondLevel(god));
            expBarController.UpdateInstant(unit.Bonds.GetBondExperience(god));
            blessed.gameObject.SetActive(unit.Blessing.God==god);
        }

        public void Select()
        {
            selected.gameObject.SetActive(true);
        }

        public void Deselect()
        {
            selected.gameObject.SetActive(false);
        }

        public void Clicked()
        {
            onClicked?.Invoke();
        }
    }
}
