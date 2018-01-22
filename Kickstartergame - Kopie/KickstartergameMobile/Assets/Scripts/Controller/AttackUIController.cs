﻿using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackUIController : MonoBehaviour {

    const float HP_BAR_OFFSET_DELAY = 0.005f;
    const float MISS_TEXT_FADE_IN_SPEED = 0.12f;
    const float MISS_TEXT_FADE_OUT_SPEED = 0.02f;
    const float MISS_TEXT_VISIBLE_DURATION = 1.5f;

    [Header("Input Fields")]
    [SerializeField]
    AttackPatternUI attackReactionUI;
    [SerializeField]
    Image attackerSprite;
    [SerializeField]
    Image defenderSprite;
    [SerializeField]
    private Text attackerHP;
    [SerializeField]
    private Text defenderHP;
    [SerializeField]
    private Text attackerDMG;
    [SerializeField]
    private Text attackerHIT;
    [SerializeField]
    private Text attackCountText;
    [SerializeField]
    private Text attackerMaxDMG;
    [SerializeField]
    private Text attackerMaxHIT;
    [SerializeField]
    private Text attackerSP;
    [SerializeField]
    private Text attackerSP2;
    [SerializeField]
    private Text missedText;
    [SerializeField]
    private Text damageText;
    [SerializeField]
    private Image attackerHPBar;
    [SerializeField]
    private Image attackerLosingHPBar;
    [SerializeField]
    private Image defenderHPBar;
    [SerializeField]
    private Image defenderLosingHPBar;
    [SerializeField]
    private Slider hitSlider;
    [SerializeField]
    private Slider attackSlider;
    [SerializeField]
    private GameObject attackTutorial;
    [SerializeField]
    private Text counterMissedText;
    [SerializeField]
    private Text counterDamageText;
    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private GameObject fastAttackGO;
    [SerializeField]
    private GameObject strongAttackGO;
    [SerializeField]
    private GameObject idleAttackGO;
    [SerializeField]
    private GameObject swipeAttackGO;
    [Header("Configuration")]
    [SerializeField]
    private float healthSpeed;
    [SerializeField]
    private float healthLoseSpeed;

    private int attackCount;
    private float currentAllyHPValue;
    private float currentEnemyHPValue;
    private float delayedAllyHPValue = 1;
    private float delayedEnemyHPValue=1;
    private float allyFillAmount = 1;
    private float enemyFillAmount = 1;
    private float allyLoseFillAmount = 1;
    private float enemyLoseFillAmount = 1;
    private int currentHitSliderValue = 0;
    private int currentAtkSliderValue = 0;
    private LivingObject attacker;
    private LivingObject defender;

    void Start () {
       
	}
    public void SetAttackCountText()
    {
        
    }
    void OnEnable()
    {
        OnSliderValueChanged();
       // attackTutorial.SetActive(true);
        HPValueChanged();
        swipeAttackGO.SetActive(true);
        EventContainer.hpValueChanged += HPValueChanged;
        EventContainer.attackUIVisible(true);
        EventContainer.swipeRightEvent += ShowStrongAttack;
        EventContainer.swipeLeftEvent += ShowFastAttack;
        EventContainer.swipeIdleEvent += ShowIdleAttack;
        EventContainer.swipeLeftConfirmedEvent += AttackButtonClicked;
        EventContainer.swipeRightConfirmedEvent += AttackButtonClicked;
        attackerSprite.sprite = attacker.Sprite;
        defenderSprite.sprite = defender.Sprite;
        attackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
    }
    public void ShowAttackReaction(string user, string reactionName)
    {
        Debug.Log("Show Reaction");
        
        attackReactionUI.Show(user,reactionName);
    }
    private void OnDisable()
    {
        EventContainer.hpValueChanged -= HPValueChanged;
        EventContainer.attackUIVisible(false);
        EventContainer.swipeRightEvent -= ShowStrongAttack;
        EventContainer.swipeLeftEvent -= ShowFastAttack;
        EventContainer.swipeIdleEvent -= ShowIdleAttack;
        EventContainer.swipeLeftConfirmedEvent -= AttackButtonClicked;
        EventContainer.swipeRightConfirmedEvent -= AttackButtonClicked;
    }

    void Update () {
        allyFillAmount = Mathf.Lerp(allyFillAmount, currentAllyHPValue, Time.deltaTime * healthSpeed);
        enemyFillAmount = Mathf.Lerp(enemyFillAmount, currentEnemyHPValue, Time.deltaTime * healthSpeed);
        enemyLoseFillAmount = Mathf.Lerp(enemyLoseFillAmount, delayedEnemyHPValue, Time.deltaTime * healthLoseSpeed);
        allyLoseFillAmount = Mathf.Lerp(allyLoseFillAmount, delayedAllyHPValue, Time.deltaTime * healthLoseSpeed);
        attackerHPBar.fillAmount = allyFillAmount;
        defenderHPBar.fillAmount = enemyFillAmount;
        defenderLosingHPBar.fillAmount = enemyLoseFillAmount;
        attackerLosingHPBar.fillAmount = allyLoseFillAmount;
        if (attackCount == 1)
            attackCountText.text = attackCount + " Attack";
        else
            attackCountText.text = attackCount+" Attacks";
    }
    
    public void Show(LivingObject attacker, LivingObject defender)
    {
        this.attacker = attacker;
        this.defender = defender;
        attackButton.interactable = true;
        gameObject.SetActive(true);
        currentAtkSliderValue = 0;
        currentHitSliderValue = 0;
        hitSlider.value = 0;
        attackSlider.value = 0;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    IEnumerator ActivateSwipeAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        swipeAttackGO.SetActive(true);
    }
    public void AttackButtonClicked()
    {
        swipeAttackGO.SetActive(false);
        
        attackButton.interactable = false;
        if (attackCount > 0)
        {
            Debug.Log("StartAttack!");
            EventContainer.attacktButtonCLicked();
            attackCount--;
        }
        if(attackCount <=0)
        {
            swipeAttackGO.SetActive(false);
        }
        else
            StartCoroutine(ActivateSwipeAttack(0.0f));
    }

    void ShowFastAttack()
    {
        idleAttackGO.SetActive(false);
        strongAttackGO.SetActive(false);
        fastAttackGO.SetActive(true);
    }
    void ShowStrongAttack()
    {
        idleAttackGO.SetActive(false);
        strongAttackGO.SetActive(true);
        fastAttackGO.SetActive(false);
    }
    void ShowIdleAttack()
    {
        idleAttackGO.SetActive(true);
        strongAttackGO.SetActive(false);
        fastAttackGO.SetActive(false);
    }
    public void ShowCounterMissText()
    {
        StartCoroutine(TextAnimation(counterMissedText));
    }
    public void ShowCounterDamageText(int damage)
    {
        counterDamageText.text = "-" + damage;
        StartCoroutine(TextAnimation(counterDamageText));
    }
    public void ShowMissText()
    {
        StartCoroutine(TextAnimation(missedText));
    }
    public void ShowDamageText(int damage)
    {
        damageText.text = "-" + damage;
        StartCoroutine(TextAnimation(damageText));
    }

    void UpdateHit()
    {
        int hit = Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender) + 10 * currentHitSliderValue, 0, 100);
        attackerMaxHIT.text = "" + Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender) + hitSlider.maxValue * 10, 0, 100) + "%";
        attackerHIT.text = "" + hit + "%";
        if (EventContainer.attackerHitChanged != null)
            EventContainer.attackerHitChanged(hit);
        //attackerSP2.text = "" + attacker.Stats.SP;
    }
    void UpdateDamage()
    {
        int damage = attacker.BattleStats.GetDamage() + 1 * currentAtkSliderValue;
        int dmg = (defender.BattleStats.GetReceivedDamage(damage));
        int maxdmg = (defender.BattleStats.GetReceivedDamage(attacker.BattleStats.GetDamage() + (int)attackSlider.maxValue));
        attackerMaxDMG.text = "" + maxdmg;
        attackerDMG.text = "" + dmg;
        attackerSP.text = "" + attacker.Stats.SP;
        int bonusDmg = 1 * currentAtkSliderValue;
        if (EventContainer.attackerDmgChanged != null)
            EventContainer.attackerDmgChanged(bonusDmg);
    }
    public void OnSliderValueChanged()
    {
        if(currentHitSliderValue != (int)hitSlider.value)
        {
            attacker.Stats.SP = attacker.Stats.SP - ((int)hitSlider.value - currentHitSliderValue);
            currentHitSliderValue = (int)hitSlider.value;
        }
        else if (currentAtkSliderValue != (int)attackSlider.value)
        {
            attacker.Stats.SP = attacker.Stats.SP - ((int)attackSlider.value - currentAtkSliderValue);
            currentAtkSliderValue = (int)attackSlider.value;
        }
        attackTutorial.SetActive(false);
        attackSlider.maxValue = (int)Mathf.Clamp(attackSlider.value + attacker.Stats.SP, 0, 3);
        hitSlider.maxValue = (int)Mathf.Clamp(hitSlider.value + attacker.Stats.SP, 0, 3);
        UpdateHit();
        UpdateDamage();
    }

    private void HPValueChanged()
    {
        defenderHP.text = defender.Stats.HP + " / " + defender.Stats.MaxHP;
        attackerHP.text = attacker.Stats.HP + " / " + attacker.Stats.MaxHP;
        currentAllyHPValue = MathUtility.MapValues(attacker.Stats.HP, 0f, attacker.Stats.MaxHP, 0f, 1f);
        currentEnemyHPValue = MathUtility.MapValues(defender.Stats.HP, 0f, defender.Stats.MaxHP, 0f, 1f);

        StartCoroutine(DelayedAllyHP());
        StartCoroutine(DelayedEnemyHP());
    }

    #region COROUTINES
    IEnumerator DelayedAllyHP()
    {
        while (Mathf.Abs(allyFillAmount - currentAllyHPValue) >= HP_BAR_OFFSET_DELAY)
        {
            yield return null;
        }
        delayedAllyHPValue = MathUtility.MapValues(attacker.Stats.HP, 0f, attacker.Stats.MaxHP, 0f, 1f);
    }
    IEnumerator DelayedEnemyHP()
    {
        while (Mathf.Abs(enemyFillAmount - currentEnemyHPValue) >= HP_BAR_OFFSET_DELAY)
        {
            yield return null;
        }
        delayedEnemyHPValue = MathUtility.MapValues(defender.Stats.HP, 0f, defender.Stats.MaxHP, 0f, 1f);
    }

    IEnumerator TextAnimation(Text text)
    {
        float alpha = 0;
        text.gameObject.SetActive(true);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        while (alpha < 1)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha += MISS_TEXT_FADE_IN_SPEED;
            yield return new WaitForSeconds(0.01f);
        }
        alpha = 1;
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        yield return new WaitForSeconds(MISS_TEXT_VISIBLE_DURATION);
        while (alpha > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha -= MISS_TEXT_FADE_OUT_SPEED;
            yield return new WaitForSeconds(0.01f);
        }
        alpha = 0;
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        text.gameObject.SetActive(false);

    }
    #endregion
}
