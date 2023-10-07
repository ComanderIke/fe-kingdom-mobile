using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.GUI
{
    public class CharacterUIController :  ICharacterUI
    {
        // [SerializeField]
        // private UIStatPanel statPanel;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Animator animator;
        [SerializeField]
        private Image faceSprite;
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private IStatBar hpBar;

        [SerializeField] private Transform hpBarToScale;
        [SerializeField] private Vector3 selectedScaleHpBarBackground;
        [SerializeField] private Vector3 selectedScale;
        [SerializeField] private Vector3 normalScale;
        [SerializeField] private Vector3 expSelectedScale;
        [SerializeField]
        private  Vector2 selectedSize;
        [SerializeField]
        private  Vector2 normalSize;
         [SerializeField]
                private  Vector2 normalSizeBars;
                [SerializeField]
                private  Vector2 selectedSizeBars;
                [SerializeField]
                private  Vector2 normalSizeExpBar;
                [SerializeField]
                private  Vector2 selectedSizeExpBar;
        [SerializeField]
        private RectTransform attractor;
        public IClickedReceiver parentController;
        [SerializeField] private MMF_Player lowHealthFeedback;
        [SerializeField] private MMF_Player normalHealthFeedback;

       // [SerializeField]
      //  private IStatBar spBar;
        // [SerializeField]
        // private ISPBarRenderer spBars;

        [SerializeField] private ExpBarController expBar = default;
        // [SerializeField] private TextMeshProUGUI expLabel = default;
        [FormerlySerializedAs("unitBp")] public Unit unit;
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Active = Animator.StringToHash("Active");
        [SerializeField]private float lowHealthThreshold=0.25f;
        [SerializeField] private int sortOrder = 0;
        
        private void Start()
        {
           
        }

        public void ShowActive(Unit unit)
        {
            this.unit = unit;
            Debug.Log("Show UnitCharacterUI ACTIVE: "+unit.name);
            unit.HpValueChanged -= UpdateValues;
            unit.HpValueChanged += UpdateValues;
            unit.ExperienceManager.ExpGained -= UpdateExp;
            unit.ExperienceManager.ExpGained += UpdateExp;
            UpdateValues();
            gameObject.SetActive(true);
            hpBar.transform.localScale = selectedScale;
            hpBarToScale.transform.localScale = selectedScaleHpBarBackground;
            expBar.transform.localScale = expSelectedScale;
             GetComponent<RectTransform>().sizeDelta = selectedSize;
            // expBar.GetComponent<RectTransform>().sizeDelta = selectedSizeExpBar;
            // hpBar.GetComponent<RectTransform>().sizeDelta = selectedSizeBars;
            animator.SetBool(Dead, !unit.IsAlive());
            animator.SetBool(Active, true);
            canvas.sortingOrder = sortOrder+10;
            //GameplayInput.SelectUnit(unit);



        }
        public override void Show(Unit unit)
        {
            this.unit = unit;
            canvas.sortingOrder =sortOrder;
           // Debug.Log("Show UnitCharacterUI: "+unit.name);
            unit.HpValueChanged -= UpdateValues;
            unit.HpValueChanged += UpdateValues;
            unit.ExperienceManager.ExpGained -= UpdateExp;
            unit.ExperienceManager.ExpGained += UpdateExp;
            UpdateValues();
            gameObject.SetActive(true);
            hpBar.transform.localScale = normalScale;
            hpBarToScale.transform.localScale = normalScale;
            GetComponent<RectTransform>().sizeDelta = normalSize;
             expBar.transform.localScale = normalScale;
            // expBar.GetComponent<RectTransform>().sizeDelta = normalSizeExpBar;
            // hpBar.GetComponent<RectTransform>().sizeDelta = normalSizeBars;
            
            animator.SetBool(Dead, !unit.IsAlive());
            animator.SetBool(Active, false);


        }

        private void UpdateExp(int expbefore, int expgained)
        {
            expBar.UpdateInstant(expbefore);
            expBar.UpdateWithAnimatedTextOnly(expgained);
        }

        

        // public void PlusClicked()
        // {
        //     
        //     parentController.PlusClicked(unit);
        // }

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

       

        private void OnDisable()
        {
            if (unit == null)
                return;
            unit.HpValueChanged -= UpdateValues;
            unit.ExperienceManager.ExpGained -= UpdateExp;
        }

        void UpdateValues()
        {
            if (unit == null)
                return;
            characterName.SetText(unit.name);
           // Debug.Log("Update Unit Values: "+unit.name);
            expBar.UpdateInstant(unit.ExperienceManager.Exp);
            hpBar.SetValue(unit.Hp, unit.MaxHp, true);
           
            if ((unit.Hp*1.0f) / unit.MaxHp <= lowHealthThreshold)
            {
                normalHealthFeedback.StopFeedbacks();
                lowHealthFeedback.PlayFeedbacks();
                
            }
            else
            {
                lowHealthFeedback.StopFeedbacks();
                normalHealthFeedback.PlayFeedbacks();
            }
            if(unit.visuals.CharacterSpriteSet!=null)
                faceSprite.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
            animator.SetBool(Dead, !unit.IsAlive());
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