using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.GUI
{
    
    public class CharacterUIController :  ICharacterUI
    {
        [SerializeField]
        private UIStatPanel statPanel;
        [SerializeField]
        private Image faceSprite;
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private IStatBar hpBar;
        [SerializeField]
        private IStatBar spBar;

        public Unit unit;
    
        

        public void CharacterImageClicked()
        {
            statPanel.gameObject.SetActive(!statPanel.gameObject.activeSelf);
        }

        public override void Show(Unit unit)
        {
            this.unit = unit;
            UpdateValues();
            gameObject.SetActive(true);
            
        }
        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
           UpdateValues();
            
        }

        void UpdateValues()
        {
            if (unit == null)
                return;
            characterName.SetText(unit.name);
            statPanel.SetStats(unit.Stats);
            hpBar.SetValue(unit.Hp, unit.Stats.MaxHp);
            spBar.SetValue(unit.Sp, unit.Stats.MaxSp);
            faceSprite.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
        }

        
    }
}