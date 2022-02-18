using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.GUI
{
    
    public class CharacterUIController :  ICharacterUI
    {
        // [SerializeField]
        // private UIStatPanel statPanel;
        [SerializeField]
        private Image faceSprite;
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private IStatBar hpBar;
        [SerializeField]
        private  Vector2 selectedSize;
        [SerializeField]
        private  Vector2 normalSize;

        public UIPartyCharacterCircleController parentController;

       // [SerializeField]
      //  private IStatBar spBar;
        // [SerializeField]
        // private ISPBarRenderer spBars;

        [SerializeField] private ExpBarController expBar = default;
        // [SerializeField] private TextMeshProUGUI expLabel = default;
        public Unit unit;
    
        

       
        public void ShowActive(Unit unit)
        {
            this.unit = unit;
            UpdateValues();
            gameObject.SetActive(true);
            GetComponent<RectTransform>().sizeDelta = selectedSize;
            
        }
        public override void Show(Unit unit)
        {
            this.unit = unit;
            UpdateValues();
            gameObject.SetActive(true);
            GetComponent<RectTransform>().sizeDelta = normalSize;
            
        }

        public void Clicked()
        {
            parentController.Clicked(unit);
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
           
            expBar.SetText(unit.ExperienceManager.Exp);
            expBar.SetFillAmount(unit.ExperienceManager.Exp);
            hpBar.SetValue(unit.Hp, unit.Stats.MaxHp);
            //  spBars.SetValue(unit.SpBars, unit.MaxSpBars);
            faceSprite.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
        }

        
    }
}