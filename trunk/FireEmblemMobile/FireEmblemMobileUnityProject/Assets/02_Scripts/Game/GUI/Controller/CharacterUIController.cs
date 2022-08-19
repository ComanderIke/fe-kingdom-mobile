using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
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
         [SerializeField]
                private  Vector2 normalSizeBars;
                [SerializeField]
                private  Vector2 selectedSizeBars;
        [SerializeField]
        private RectTransform attractor;
        public IClickedReceiver parentController;

       // [SerializeField]
      //  private IStatBar spBar;
        // [SerializeField]
        // private ISPBarRenderer spBars;

        [SerializeField] private ExpBarController expBar = default;
        // [SerializeField] private TextMeshProUGUI expLabel = default;
        public Unit unit;


        private void Start()
        {
           
        }

        public void ShowActive(Unit unit)
        {
            this.unit = unit;
          
            UpdateValues();
            gameObject.SetActive(true);
            GetComponent<RectTransform>().sizeDelta = selectedSize;
            expBar.GetComponent<RectTransform>().sizeDelta = selectedSizeBars;
            hpBar.GetComponent<RectTransform>().sizeDelta = selectedSizeBars;
            
            //GameplayInput.SelectUnit(unit);
           
            

        }
        public override void Show(Unit unit)
        {
            this.unit = unit;
            unit.HpValueChanged -= UpdateValues;
            unit.HpValueChanged += UpdateValues;
            UpdateValues();
            gameObject.SetActive(true);
            GetComponent<RectTransform>().sizeDelta = normalSize;
            expBar.GetComponent<RectTransform>().sizeDelta = normalSizeBars;
            hpBar.GetComponent<RectTransform>().sizeDelta = normalSizeBars;
           
            
        }

        public void PlusClicked()
        {
            Debug.Log("UnitCirclePlusClicked: "+unit);
            parentController.PlusClicked(unit);
        }

        public void Clicked()
        {
            Debug.Log("UnitCircleClicked: "+unit);
            parentController.Clicked(unit);
            FindObjectOfType<PlayerPhaseUI>()?.UnitCircleClicked(unit);
        }
        public override void Hide()
        {
            gameObject.SetActive(false);
            unit.HpValueChanged -= UpdateValues;
        }

        public override ExpBarController GetExpRenderer()
        {
            return expBar;
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
            hpBar.SetValue(unit.Hp, unit.MaxHp);
            //  spBars.SetValue(unit.SpBars, unit.MaxSpBars);
            faceSprite.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
        }

        public RectTransform GetUnitParticleAttractorTransform()
        {
           
            return attractor;
        }
       

        public ExpBarController GetExpBarController()
        {
            return expBar;
        }
    }
}