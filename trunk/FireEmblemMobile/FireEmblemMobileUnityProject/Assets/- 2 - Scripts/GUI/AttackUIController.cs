using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Humans;
using Assets.GUI.PopUpText;
using Assets.Mechanics;
using Assets.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class AttackUiController : MonoBehaviour
    {
        public const float HP_BAR_OFFSET_DELAY = 0.005f;
        public const float MISS_TEXT_FADE_IN_SPEED = 0.12f;
        public const float MISS_TEXT_FADE_OUT_SPEED = 0.02f;
        public const float MISS_TEXT_VISIBLE_DURATION = 1.5f;

        #region InspectorFields

        [Header("Input Fields")] [SerializeField]
        private Transform targetPointParent;

        [SerializeField] private Image attackerSprite;
        [SerializeField] private Image defenderSprite;
        [SerializeField] private TextMeshProUGUI attackerHp;
        [SerializeField] private TextMeshProUGUI defenderHp;
        [SerializeField] private TextMeshProUGUI attackCountText;
        [SerializeField] private TextMeshProUGUI attackerDmg;
        [SerializeField] private Image attackerHpBar;
        [SerializeField] private Image attackerLosingHpBar;
        [SerializeField] private Image defenderHpBar;
        [SerializeField] private Image defenderLosingHpBar;
        [SerializeField] private GameObject attackContainer;
        [SerializeField] private GameObject frontalAttackGo;
        [SerializeField] private GameObject surpriseAttackGo;

        [Header("Configuration")] [SerializeField]
        private float healthSpeed;

        [SerializeField] private float healthLoseSpeed;

        #endregion

        private int attackCount;
        private float currentAllyHpValue;
        private float currentEnemyHpValue;
        private float delayedAllyHpValue = 1;
        private float delayedEnemyHpValue = 1;
        private float allyFillAmount = 1;
        private float enemyFillAmount = 1;
        private float allyLoseFillAmount = 1;
        private float enemyLoseFillAmount = 1;
        private Unit attacker;
        private Unit defender;
        private bool frontAttack;
        private bool surpriseAttack;

        private void Start()
        {
        }

        private void OnEnable()
        {
            // attackTutorial.SetActive(true);
            HpValueChanged();

            Unit.HpValueChanged += HpValueChanged;
            UiSystem.OnAttackUiVisible(true);
            BattleSystem.OnEnemyTakesDamage += ShowDamageText;
            BattleSystem.OnAllyTakesDamage += ShowCounterDamageText;
            //attackerSprite.sprite = attacker.Sprite;
            defenderSprite.sprite = defender.CharacterSpriteSet.FaceSprite;
            attackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
        }

        private void OnDisable()
        {
            Unit.HpValueChanged -= HpValueChanged;
            BattleSystem.OnEnemyTakesDamage -= ShowDamageText;
            BattleSystem.OnAllyTakesDamage -= ShowCounterDamageText;
            UiSystem.OnAttackUiVisible(false);
        }

        private void Update()
        {
            allyFillAmount = Mathf.Lerp(allyFillAmount, currentAllyHpValue, Time.deltaTime * healthSpeed);
            enemyFillAmount = Mathf.Lerp(enemyFillAmount, currentEnemyHpValue, Time.deltaTime * healthSpeed);
            enemyLoseFillAmount =
                Mathf.Lerp(enemyLoseFillAmount, delayedEnemyHpValue, Time.deltaTime * healthLoseSpeed);
            allyLoseFillAmount = Mathf.Lerp(allyLoseFillAmount, delayedAllyHpValue, Time.deltaTime * healthLoseSpeed);
            attackerHpBar.fillAmount = allyFillAmount;
            defenderHpBar.fillAmount = enemyFillAmount;
            defenderLosingHpBar.fillAmount = enemyLoseFillAmount;
            attackerLosingHpBar.fillAmount = allyLoseFillAmount;
            if (attackCount == 1)
                attackCountText.text = attackCount + " Attack";
            else
                attackCountText.text = attackCount + " Attacks";
        }

        public void Show(Unit attacker, Unit defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            frontAttack = false;
            surpriseAttack = false;
            gameObject.SetActive(true);
            Debug.Log(attacker.BattleStats.IsFrontalAttack(defender));
            if (attacker.BattleStats.IsFrontalAttack(defender))
            {
                frontAttack = true;
                frontalAttackGo.SetActive(true);
            }
            else if (attacker.BattleStats.IsBackSideAttack(defender))
            {
                surpriseAttack = true;
                surpriseAttackGo.SetActive(true);
            }

            //chooseTargetTutorial.SetActive(true);
            foreach (var child in targetPointParent.GetComponentsInChildren<Transform>())
            {
                if (child != targetPointParent)
                    Destroy(child.gameObject);
            }

            UpdateDamage();
        }

        public void Hide()
        {
            StartCoroutine(DelayHide(0.25f));
        }

        private IEnumerator DelayHide(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }

        public void AttackButtonCLicked()
        {
            StartAttack();
        }

        public void StartAttack()
        {
            attackContainer.SetActive(false);

            if (attackCount > 0)
            {
                Debug.Log("StartAttack!");
                BattleSystem.OnStartAttack();
                attackCount--;
            }

            if (attackCount <= 0)
            {
                attackContainer.SetActive(false);
            }
        }

        public void ShowCounterDamageText(int damage, bool magic = false)
        {
            MainScript.Instance.GetSystem<PopUpTextSystem>()
                .CreateAttackPopUpTextRed("" + damage, attackerSprite.transform);
        }

        public void ShowDamageText(int damage, bool magic = false)
        {
            if (magic)
                MainScript.Instance.GetSystem<PopUpTextSystem>()
                    .CreateAttackPopUpTextBlue("" + damage, targetPointParent.transform);
            else
                MainScript.Instance.GetSystem<PopUpTextSystem>()
                    .CreateAttackPopUpTextRed("" + damage, targetPointParent.transform);
            //activeTarget.ShowDamageText(damage);
        }

        private void UpdateDamage()
        {
            var humanAttacker = (Human) attacker;
            float multiplier = 1; //attackTarget.DamageMultiplier;
            if (frontAttack)
            {
                multiplier = humanAttacker.BattleStats.FrontalAttackModifier;
            }

            var attackMultiplier = new List<float> {multiplier};
            int damage = attacker.BattleStats.GetDamage(attackMultiplier);
            int dmg = (defender.BattleStats.GetReceivedDamage(damage));
            attackerDmg.text = "" + dmg;
        }

        private void HpValueChanged()
        {
            defenderHp.text = defender.Hp + " / " + defender.Stats.MaxHp;
            attackerHp.text = attacker.Hp + " / " + attacker.Stats.MaxHp;
            currentAllyHpValue = MathUtility.MapValues(attacker.Hp, 0f, attacker.Stats.MaxHp, 0f, 1f);
            currentEnemyHpValue = MathUtility.MapValues(defender.Hp, 0f, defender.Stats.MaxHp, 0f, 1f);

            StartCoroutine(DelayedAllyHp());
            StartCoroutine(DelayedEnemyHp());
        }

        #region COROUTINES

        private IEnumerator DelayedAllyHp()
        {
            while (Mathf.Abs(allyFillAmount - currentAllyHpValue) >= HP_BAR_OFFSET_DELAY)
            {
                yield return null;
            }

            delayedAllyHpValue = MathUtility.MapValues(attacker.Hp, 0f, attacker.Stats.MaxHp, 0f, 1f);
        }

        private IEnumerator DelayedEnemyHp()
        {
            while (Mathf.Abs(enemyFillAmount - currentEnemyHpValue) >= HP_BAR_OFFSET_DELAY)
            {
                yield return null;
            }

            delayedEnemyHpValue = MathUtility.MapValues(defender.Hp, 0f, defender.Stats.MaxHp, 0f, 1f);
        }

        #endregion
    }
}