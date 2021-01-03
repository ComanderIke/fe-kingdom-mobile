using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GUI.Text;
using Game.Mechanics.Battle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class AttackPreviewUI : MonoBehaviour
    {
        [SerializeField] private Canvas canvas = default;
        [SerializeField] private CanvasGroup canvasGroup = default;

        [Header("Left")]
        [SerializeField] private GameObject left = default;
        [SerializeField] private TextMeshProUGUI atkValue = default;
        [SerializeField] private TextMeshProUGUI spdValue = default;
        [SerializeField] private TextMeshProUGUI defLabel = default;
        [SerializeField] private TextMeshProUGUI defValue = default;
        [SerializeField] private TextMeshProUGUI sklValue = default;
        [SerializeField] private Image faceSpriteLeft = default;
        [SerializeField] private TextMeshProUGUI dmgValue = default;
        [SerializeField] private TextMeshProUGUI attackCount = default;
        [SerializeField] private GameObject attackCountX = default;
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
        [SerializeField] private TextMeshProUGUI attackCountRight = default;
        [SerializeField] private GameObject attackCountRightX = default;
        [SerializeField] private AttackPreviewStatBar hpBarRight = default;
        [SerializeField] private AttackPreviewStatBar spBarRight = default;
        RawImageUVOffsetAnimation[] uvAnimations;
        UILoopPingPongFade[] fadeAnimations;
        ScaleAnimation[] scaleAnimations;
        private RectTransform rectTransform;
        private Camera Camera;
        private bool visible = true;
        public void UpdateValues (Unit attacker, Unit defender, BattlePreview battlePreview, Sprite attackerSprite, Sprite defenderSprite)
        {
            
            if(rectTransform==null)
                rectTransform = GetComponent<RectTransform>();
            if (Camera == null)
                Camera = Camera.main;
            float yPos = Camera.WorldToScreenPoint(new Vector3(defender.GridPosition.X, defender.GridPosition.Y+1f, 0)).y;

            if (yPos >= Screen.height - (306) - rectTransform.rect.height)//306 is UiHeight and height of this object
            {
                yPos = Camera.WorldToScreenPoint(new Vector3(defender.GridPosition.X, defender.GridPosition.Y , 0)).y;
                yPos -= rectTransform.rect.height;
            }

            Show(yPos);
            faceSpriteLeft.sprite = attackerSprite;
            faceSpriteRight.sprite = defenderSprite;
            if (!defender.IsVisible)
            {
                dmgValue.text = "?";
                dmgValueRight.text = "?";
                attackCount.text = "";
                attackCountRight.text = "";
                hpBar.UpdateValues(battlePreview.Attacker.MaxHp, battlePreview.Attacker.CurrentHp, -1, new List<int>(), true);
                spBar.UpdateValues(battlePreview.Attacker.MaxSp, battlePreview.Attacker.CurrentSp, -1, new List<int>(), true);
                attackCountX.SetActive(false);
                attackCountRightX.SetActive(false);
                attackCount.gameObject.SetActive(false);
                attackCountRight.gameObject.SetActive(false);
                hpBarRight.UpdateValues(battlePreview.Defender.MaxHp, battlePreview.Defender.CurrentHp, -1, new List<int>());
                spBarRight.UpdateValues(battlePreview.Defender.MaxSp, battlePreview.Defender.CurrentSp, -1, new List<int>());
                faceSpriteRight.color = new Color(0, 0, 0, 1);
            }
            else
            {
                dmgValue.text = "" + battlePreview.Attacker.Damage;
                attackCountX.SetActive(battlePreview.Attacker.AttackCount > 1);
                attackCount.gameObject.SetActive(battlePreview.Attacker.AttackCount > 1);
                attackCount.text = "" + battlePreview.Attacker.AttackCount;
                hpBar.UpdateValues(battlePreview.Attacker.MaxHp, battlePreview.Attacker.CurrentHp, battlePreview.Attacker.AfterBattleHp, battlePreview.Attacker.IncomingDamage);
                spBar.UpdateValues(battlePreview.Attacker.MaxSp, battlePreview.Attacker.CurrentSp, battlePreview.Attacker.AfterBattleSp, battlePreview.Attacker.IncomingSpDamage);
                dmgValueRight.text = "" + battlePreview.Defender.Damage;
                attackCountRightX.SetActive(battlePreview.Defender.AttackCount > 1);
                attackCountRight.gameObject.SetActive(battlePreview.Defender.AttackCount > 1);
                attackCountRight.text = "" + battlePreview.Defender.AttackCount;
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
        public void Show(float yPos)
        {
            visible = true;
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
        void OnEnable()
        {
            if (uvAnimations == null)
                uvAnimations = GetComponentsInChildren<RawImageUVOffsetAnimation>();
            if (fadeAnimations == null)
                fadeAnimations = GetComponentsInChildren<UILoopPingPongFade>();
            if (scaleAnimations == null)
                scaleAnimations = GetComponentsInChildren<ScaleAnimation>();
        }
       
        public void Hide()
        {
            if (!visible)
                return;
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            visible = false;
            ClearTweens();
            LeanTween.alphaCanvas(canvasGroup, 0, 0.2f).setEaseOutQuad();

            LeanTween.moveLocalX(left, -rectTransform.rect.width, 0.2f).setEaseOutQuad();
            LeanTween.moveLocalX(right, rectTransform.rect.width, 0.2f).setEaseOutQuad().setOnComplete(() => {
                canvas.enabled = false;
                foreach(var animation in uvAnimations)
                {
                    animation.enabled = false;
                }
                foreach (var animation in fadeAnimations)
                {
                    animation.enabled = false;
                }
                foreach (var animation in scaleAnimations)
                {
                    animation.enabled = false;
                }
            }) ;
        }
    }
}