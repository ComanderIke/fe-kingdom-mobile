using Assets.GameActors.Units;
using Assets.Mechanics.Battle;
using Assets.Utility;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class AttackPreviewUI : MonoBehaviour
    {

        [SerializeField] private CanvasGroup canvas = default;

        [Header("Left")]
        [SerializeField] private GameObject left = default;
        [SerializeField] private TextMeshProUGUI atkValue = default;
        [SerializeField] private TextMeshProUGUI spdValue = default;
        [SerializeField] private TextMeshProUGUI defLabel = default;
        [SerializeField] private TextMeshProUGUI defValue = default;
        [SerializeField] private TextMeshProUGUI sklValue = default;
        [SerializeField] private Image faceSpriteLeft = default;
        [SerializeField] private TextMeshProUGUI dmgValue = default;
        [SerializeField] private GameObject attackCount = default;
        [SerializeField] private AttackPreviewStatBar hpBar = default;
        [SerializeField] private AttackPreviewStatBar spBar = default;
        [Header("Right")]
        [SerializeField] private GameObject right = default;
        [SerializeField] private TextMeshProUGUI atkValueRight = default;
        [SerializeField] private TextMeshProUGUI spdValueRight = default;
        [SerializeField] private TextMeshProUGUI defLabelRight = default;
        [SerializeField] private TextMeshProUGUI defValueRight = default;
        [SerializeField] private TextMeshProUGUI sklValueRight = default;
        [SerializeField] private Image faceSpriteRight = default;
        [SerializeField] private TextMeshProUGUI dmgValueRight = default;
        [SerializeField] private GameObject attackCountRight = default;
        [SerializeField] private AttackPreviewStatBar hpBarRight = default;
        [SerializeField] private AttackPreviewStatBar spBarRight = default;

        public void UpdateValues (Unit attacker, Unit defender, BattlePreview battlePreview, Sprite attackerSprite, Sprite defenderSprite)
        {
            
            RectTransform rectTransform = GetComponent<RectTransform>();
            float yPos = Camera.main.WorldToScreenPoint(new Vector3(defender.GridPosition.X, defender.GridPosition.Y + 1f, 0)).y;
            if (yPos - (Screen.height / 2) >= Screen.height / 2 - (306) - rectTransform.rect.height)//306 is UiHeight and height of this object
            { 
                yPos = Camera.main.WorldToScreenPoint(new Vector3(defender.GridPosition.X, defender.GridPosition.Y , 0)).y;
                yPos -= rectTransform.rect.height;
            }
            else
            {
            }

            Show(yPos);
            faceSpriteLeft.sprite = attackerSprite;
            faceSpriteRight.sprite = defenderSprite;
            if (!defender.IsVisible)
            {
                dmgValue.text = "?";
                dmgValueRight.text = "?";
                hpBar.UpdateValues(battlePreview.Attacker.MaxHp, battlePreview.Attacker.CurrentHp, -1, new List<int>());
                spBar.UpdateValues(battlePreview.Attacker.MaxSp, battlePreview.Attacker.CurrentSp, -1, new List<int>());
                attackCount.SetActive(false);
                attackCountRight.SetActive(false);
                hpBarRight.UpdateValues(battlePreview.Defender.MaxHp, battlePreview.Defender.CurrentHp, -1, new List<int>());
                spBarRight.UpdateValues(battlePreview.Defender.MaxSp, battlePreview.Defender.CurrentSp, -1, new List<int>());
                faceSpriteRight.color = new Color(0, 0, 0, 1);
            }
            else
            {
                dmgValue.text = "" + battlePreview.Attacker.Damage;
                attackCount.SetActive(battlePreview.Attacker.AttackCount > 1);
                hpBar.UpdateValues(battlePreview.Attacker.MaxHp, battlePreview.Attacker.CurrentHp, battlePreview.Attacker.AfterBattleHp, battlePreview.Attacker.IncomingDamage);
                spBar.UpdateValues(battlePreview.Attacker.MaxSp, battlePreview.Attacker.CurrentSp, battlePreview.Attacker.AfterBattleSp, battlePreview.Attacker.IncomingSpDamage);
                dmgValueRight.text = "" + battlePreview.Defender.Damage;
                attackCountRight.SetActive(battlePreview.Defender.AttackCount > 1);
                hpBarRight.UpdateValues(battlePreview.Defender.MaxHp, battlePreview.Defender.CurrentHp, battlePreview.Defender.AfterBattleHp, battlePreview.Defender.IncomingDamage);
                spBarRight.UpdateValues(battlePreview.Defender.MaxSp, battlePreview.Defender.CurrentSp, battlePreview.Defender.AfterBattleSp, battlePreview.Defender.IncomingSpDamage);
                faceSpriteRight.color = new Color(1, 1, 1, 1);
            }

            //spdValue.text = "" + battlePreview.Attacker.Speed;
            //defLabel.text = battlePreview.Attacker.IsPhysical ? "Def" : "Res";
            //defValue.text = "" + battlePreview.Attacker.Defense;
            //sklValue.text = "" + battlePreview.Attacker.Skill;
           
            
            
            
            
            //spdValueRight.text = "" + battlePreview.Defender.Speed;
            //defLabelRight.text = battlePreview.Defender.IsPhysical ? "Def" : "Res";
            //defValueRight.text = "" + battlePreview.Defender.Defense;
            //sklValueRight.text = "" + battlePreview.Defender.Skill;
            
          
        }
        float yPos;
        public void Show(float yPos)
        {
            
            gameObject.SetActive(true);
            ClearTweens();
            foreach(var comp in GetComponentsInChildren<UILoopPingPongFade>())
            {
                comp.StartAnimation();
            }
            RectTransform rectTransform = GetComponent<RectTransform>();
            canvas.alpha = 0;
            LeanTween.alphaCanvas(canvas, 1, 0.3f).setEaseOutQuad();
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
       
        public void Hide()
        {
            ClearTweens();
 
            RectTransform rectTransform = GetComponent<RectTransform>();
            LeanTween.alphaCanvas(canvas, 0, 0.2f).setEaseOutQuad();

            LeanTween.moveLocalX(left, -rectTransform.rect.width, 0.2f).setEaseOutQuad();
            LeanTween.moveLocalX(right, rectTransform.rect.width, 0.2f).setEaseOutQuad().setOnComplete(()=>gameObject.SetActive(false));
        }
    }
}