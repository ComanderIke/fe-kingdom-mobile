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
        [SerializeField] private Image godColorImage;
        [SerializeField] private Image blessedChar;
        public event Action<int> onClicked;

        [SerializeField]
        private Vector3 normalScale;
        [SerializeField]
        private Vector3 selectedScale;

        private int index = 0;
        private Unit unit;
        private God god;
        public void Show(Unit unit, God god, int index, int addedExp, bool selected, Unit blessedUnit)
        {
            name.SetText(god.name);
            this.unit = unit;
            if(blessedUnit!=null)
                blessedChar.sprite = blessedUnit.FaceSprite;
            this.god = god;
            this.index = index;
            face.sprite = god.Face;
            lv.SetText("Lv. "+unit.Bonds.GetBondLevel(god));
            godColorImage.color = god.Color;
            if(selected)
                expBarController.UpdatePreview(unit.Bonds.GetBondExperience(god)+addedExp);
            else
            {
                expBarController.UpdatePreview(0);
            }
            expBarController.UpdateInstant(unit.Bonds.GetBondExperience(god));
            blessed.gameObject.SetActive(blessedUnit!=null);
            transform.localScale = normalScale;
        }

        // public void UpdateExpPreview(int addedExp)
        // {
        //     expBarController.UpdatePreview(unit.Bonds.GetBondExperience(god)+addedExp);
        // }
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
