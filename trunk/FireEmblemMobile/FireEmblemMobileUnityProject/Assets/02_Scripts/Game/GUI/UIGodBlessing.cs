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
        [SerializeField] private Image face;
        [SerializeField] private GameObject selected;
        [SerializeField] private Image godColorImage;
        public event Action<int> onClicked;

        [SerializeField]
        private Vector3 normalScale;
        [SerializeField]
        private Vector3 selectedScale;

        private int index = 0;
        public void Show(Unit unit, God god, int index, bool selected)
        {
            this.index = index;
            face.sprite = god.DialogSpriteSet.FaceSprite;
            godColorImage.color = god.Color;
            transform.localScale = normalScale;
            if(selected)
                Select();
            else
            {
                Deselect();
            }
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
