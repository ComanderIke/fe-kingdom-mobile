using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.GUI.Text;
using Game.Mechanics.Battle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    [ExecuteInEditMode]
    public class AttackPreviewUI : IAttackPreviewUI
    {
        [SerializeField] private Canvas canvas = default;
        [SerializeField] private CanvasGroup canvasGroup = default;

        [Header("Left")]
        [SerializeField] private GameObject left = default;
        // [SerializeField] private TextMeshProUGUI atkValue = default;
        // [SerializeField] private TextMeshProUGUI spdValue = default;
        // [SerializeField] private TextMeshProUGUI defLabel = default;
        // [SerializeField] private TextMeshProUGUI defValue = default;
        // [SerializeField] private TextMeshProUGUI sklValue = default;
        [SerializeField] private Image faceSpriteLeft = default;
        [SerializeField] private TextMeshProUGUI dmgValue = default;
        [SerializeField] private TextMeshProUGUI hitValue = default;
        [SerializeField] private TextMeshProUGUI attackCount = default;
        [SerializeField] private GameObject attackCountX = default;
        [SerializeField] private AttackPreviewStatBar hpBar = default;
        // [SerializeField] private AttackPreviewStatBar spBar = default;
        [SerializeField] private ISPBarRenderer spBars;
        [Header("Right")]
        [SerializeField] private GameObject right = default;
        // [SerializeField] private TextMeshProUGUI atkValueRight = default;
        // [SerializeField] private TextMeshProUGUI spdValueRight = default;
        // [SerializeField] private TextMeshProUGUI defLabelRight = default;
        // [SerializeField] private TextMeshProUGUI defValueRight = default;
        // [SerializeField] private TextMeshProUGUI sklValueRight = default;
        [SerializeField] private Image faceSpriteRight = default;
        [SerializeField] private TextMeshProUGUI dmgValueRight = default;
        [SerializeField] private TextMeshProUGUI hitValueRight = default;
        [SerializeField] private TextMeshProUGUI attackCountRight = default;
        [SerializeField] private GameObject attackCountRightX = default;
        [SerializeField] private AttackPreviewStatBar hpBarRight = default;
        // [SerializeField] private AttackPreviewStatBar spBarRight = default;
        [SerializeField] private ISPBarRenderer spBarsRight;
        RawImageUVOffsetAnimation[] uvAnimations;
        UILoopPingPongFade[] fadeAnimations;
        ScaleAnimation[] scaleAnimations;
        private RectTransform rectTransform;
        private Camera Camera;
        //private bool visible = true;
        
        [SerializeField] 
        private Sprite attackerSprite;
        [SerializeField] 
        private Sprite defenderSprite;

        [SerializeField] 
        private BattlePreview battlePreview;

        private void UpdateValues()
        {
             if(rectTransform==null)
                rectTransform = GetComponent<RectTransform>();
             if (Camera == null)
                 Camera = Camera.main;
           

             faceSpriteLeft.sprite = attackerSprite;
             faceSpriteRight.sprite = defenderSprite;

             dmgValue.text = "" + battlePreview.AttackerStats.Damage;
             hitValue.text = "" + battlePreview.AttackerStats.Hit;
             attackCountX.SetActive(battlePreview.AttackerStats.AttackCount > 1);
             attackCount.gameObject.SetActive(battlePreview.AttackerStats.AttackCount > 1);
             attackCount.text = "" + battlePreview.AttackerStats.AttackCount;
             List<int> attackerDmg = new List<int>();
             List<int> defenderDmg = new List<int>();
             for (int i = 0; i < battlePreview.AttacksData.Count; i++)
             {
                 if (battlePreview.AttacksData[i].attacker)
                 {
                     attackerDmg.Add(battlePreview.AttacksData[i].Dmg);
                 }
                 else
                 {
                     defenderDmg.Add(battlePreview.AttacksData[i].Dmg);
                 }
             }
             hpBar.UpdateValues(battlePreview.AttackerStats.MaxHp, battlePreview.AttackerStats.CurrentHp, battlePreview.AttackerStats.AfterBattleHp, 
                 attackerDmg);
             spBars.SetPreviewValue(battlePreview.AttackerStats.CurrentSpBars,battlePreview.AttackerStats.AfterSpBars,battlePreview.AttackerStats.MaxSpBars);
             //spBar.UpdateValues(battlePreview.AttackerStats.MaxSp, battlePreview.AttackerStats.CurrentSp, battlePreview.AttackerStats.AfterBattleSp, battlePreview.AttackerStats.IncomingSpDamage);
             dmgValueRight.text = "" + battlePreview.DefenderStats.Damage;
             hitValueRight.text = "" + battlePreview.DefenderStats.Hit;
             attackCountRightX.SetActive(battlePreview.DefenderStats.AttackCount > 1);
             attackCountRight.gameObject.SetActive(battlePreview.DefenderStats.AttackCount > 1);
             attackCountRight.text = "" + battlePreview.DefenderStats.AttackCount;
             hpBarRight.UpdateValues(battlePreview.DefenderStats.MaxHp, battlePreview.DefenderStats.CurrentHp, battlePreview.DefenderStats.AfterBattleHp,
                 defenderDmg);
             spBarsRight.SetPreviewValue(battlePreview.DefenderStats.CurrentSpBars,battlePreview.DefenderStats.AfterSpBars,battlePreview.DefenderStats.MaxSpBars);
             //spBarRight.UpdateValues(battlePreview.DefenderStats.MaxSp, battlePreview.DefenderStats.CurrentSp, battlePreview.DefenderStats.AfterBattleSp, battlePreview.DefenderStats.IncomingSpDamage);
            
             faceSpriteRight.color = new Color(1, 1, 1, 1);

             //spdValue.text = "" + battlePreview.Attacker.Speed;
             //defLabel.text = battlePreview.Attacker.IsPhysical ? "Def" : "Res";
             //defValue.text = "" + battlePreview.Attacker.Defense;
             //sklValue.text = "" + battlePreview.Attacker.Skill;
           
             //spdValueRight.text = "" + battlePreview.Defender.Speed;
             //defLabelRight.text = battlePreview.Defender.IsPhysical ? "Def" : "Res";
             //defValueRight.text = "" + battlePreview.Defender.Defense;
             //sklValueRight.text = "" + battlePreview.Defender.Skill;
        }

        public override void Show(BattlePreview battlePreview, UnitVisual attackerVisual, UnitVisual defenderVisual)
        {
            attackerSprite = attackerVisual.CharacterSpriteSet.FaceSprite;
            defenderSprite = defenderVisual.CharacterSpriteSet.FaceSprite;
            this.battlePreview = battlePreview;
            this.gameObject.SetActive(true);
            UpdateValues();
           
        }
        public void Show(float yPos)
        {

            canvas.enabled = true;
            foreach (var animation in uvAnimations)
            {
                animation.enabled = true;
            }
            foreach (var animation in fadeAnimations)
            {
                animation.enabled = true;
            }
            foreach (var animation in scaleAnimations)
            {
                animation.enabled = true;
            }
            ClearTweens();

            canvasGroup.alpha = 0;
            LeanTween.alphaCanvas(canvasGroup, 1, 0.3f).setEaseOutQuad();
            left.transform.localPosition = new Vector3(-rectTransform.rect.width, 0, 0);
            right.transform.localPosition = new Vector3(rectTransform.rect.width, 0, 0);
            
            transform.localPosition = new Vector3(transform.localPosition.x, yPos - Screen.height / 2, transform.localPosition.z);
            LeanTween.moveLocalX(left, -rectTransform.rect.width / 2, 0.3f).setEaseOutQuad();
            LeanTween.moveLocalX(right, rectTransform.rect.width / 2, 0.3f).setEaseOutQuad();
        }
        private void ClearTweens()
        {
            LeanTween.cancel(gameObject);
            LeanTween.cancel(left);
            LeanTween.cancel(right);
        }

        private void Update()
        {
            //UpdateValues();//TODO Remove after Testing
        }

        void OnEnable()
        {
            UpdateValues();
            if (uvAnimations == null)
                uvAnimations = GetComponentsInChildren<RawImageUVOffsetAnimation>();
            if (fadeAnimations == null)
                fadeAnimations = GetComponentsInChildren<UILoopPingPongFade>();
            if (scaleAnimations == null)
                scaleAnimations = GetComponentsInChildren<ScaleAnimation>();
        }
       
        public override void Hide()
        {
            this.gameObject.SetActive(false);
            // if (!visible)
            //     return;
            // if (rectTransform == null)
            //     rectTransform = GetComponent<RectTransform>();
            // visible = false;
            // ClearTweens();
            // LeanTween.alphaCanvas(canvasGroup, 0, 0.2f).setEaseOutQuad();
            //
            // LeanTween.moveLocalX(left, -rectTransform.rect.width, 0.2f).setEaseOutQuad();
            // LeanTween.moveLocalX(right, rectTransform.rect.width, 0.2f).setEaseOutQuad().setOnComplete(() => {
            //     canvas.enabled = false;
            //     foreach(var animation in uvAnimations)
            //     {
            //         animation.enabled = false;
            //     }
            //     foreach (var animation in fadeAnimations)
            //     {
            //         animation.enabled = false;
            //     }
            //     foreach (var animation in scaleAnimations)
            //     {
            //         animation.enabled = false;
            //     }
            // }) ;
        }
    }
}