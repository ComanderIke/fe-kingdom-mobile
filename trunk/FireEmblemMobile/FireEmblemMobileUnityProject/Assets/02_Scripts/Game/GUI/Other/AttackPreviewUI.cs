using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.GUI.Text;
using Game.Mechanics.Battle;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class AttackPreviewUI : IAttackPreviewUI
    {
        [SerializeField] private Canvas canvas = default;
        [SerializeField] private CanvasGroup canvasGroup = default;
        
        [SerializeField] private UIAttackPreviewContainer left = default;
        [SerializeField] private UIAttackPreviewContainer right = default;

        [SerializeField] private UIAttackOrder attackOrderUI;
        
        UILoopPingPongFade[] fadeAnimations;
        ScaleAnimation[] scaleAnimations;
        private RectTransform rectTransform;
        private Camera Camera;
        
        [SerializeField] private GameObject turnCount;
        [SerializeField] 
        private BattlePreview battlePreview;

        private Sprite attackerSprite;
        private Sprite defenderSprite;
        private bool ally = false;
        private string attackLabel;

        private void UpdateValues()
        {
             if(rectTransform==null)
                rectTransform = GetComponent<RectTransform>();
             if (Camera == null)
                 Camera = Camera.main;
             
         //    Debug.Log(battlePreview.AttackerStats.AttackCount+" "+battlePreview.DefenderStats.AttackCount);
             left.Show(attackerSprite, battlePreview.AttackerStats.Heal>0?battlePreview.AttackerStats.Heal:battlePreview.AttackerStats.TotalDamage,battlePreview.AttackerStats.Hit,battlePreview.AttackerStats.Crit, battlePreview.AttackerStats.MaxHp,battlePreview.AttackerStats.CurrentHp, battlePreview.AttackerStats.AfterBattleHp, true);
             right.Show(defenderSprite, battlePreview.DefenderStats.TotalDamage,battlePreview.DefenderStats.Hit,battlePreview.DefenderStats.Crit, battlePreview.DefenderStats.MaxHp,battlePreview.DefenderStats.CurrentHp, battlePreview.DefenderStats.AfterBattleHp,battlePreview.DefenderStats.CanCounter, !ally);
             attackOrderUI.Show(battlePreview.AttacksData, attackLabel,true);
           }

        public override void Show(BattlePreview battlePreview, Unit attacker, Unit defender, string attackLabel)
        {
          
            ally = !attacker.IsEnemy(defender);
            Show(battlePreview, attacker, attackLabel,defender.visuals.CharacterSpriteSet.FaceSprite);
        }
        public override void Show(BattlePreview battlePreview, Unit attacker, string attackLabel,Sprite attackableObjectSprite)
        {
            // Debug.Log("ATTACKPREVIEWLABEL: " + attackLabel);
            this.attackLabel = attackLabel;
            attackerSprite = attacker.visuals.CharacterSpriteSet.FaceSprite;
            defenderSprite = attackableObjectSprite;
            this.battlePreview = battlePreview;
            this.gameObject.SetActive(true);
            UpdateValues();
            turnCount.SetActive(false);
         

        }
        
        public void Show(float yPos)
        {

            canvas.enabled = true;
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
            LeanTween.moveLocalX(left.gameObject, -rectTransform.rect.width / 2, 0.3f).setEaseOutQuad();
            LeanTween.moveLocalX(right.gameObject, rectTransform.rect.width / 2, 0.3f).setEaseOutQuad();
        }
        private void ClearTweens()
        {
            LeanTween.cancel(gameObject);
            LeanTween.cancel(left.gameObject);
            LeanTween.cancel(right.gameObject);
        }
        

        void OnEnable()
        {
            UpdateValues();
            if (fadeAnimations == null)
                fadeAnimations = GetComponentsInChildren<UILoopPingPongFade>();
            if (scaleAnimations == null)
                scaleAnimations = GetComponentsInChildren<ScaleAnimation>();
        }
       
        public override void Hide()
        {
            turnCount.SetActive(true);
            this.gameObject.SetActive(false);
           
        }
    }
}