using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactUIController : MonoBehaviour {
    const float HP_BAR_OFFSET_DELAY = 0.005f;
    const float MISS_TEXT_FADE_IN_SPEED = 0.12f;
    const float MISS_TEXT_FADE_OUT_SPEED = 0.02f;
    const float MISS_TEXT_VISIBLE_DURATION = 1.5f;
    [Header("Input Fields")]
    [SerializeField]
    private Image attackerImage;
    [SerializeField]
    private Image defenderImage;
    [SerializeField]
    private Text attackerHP;
    [SerializeField]
    private Text defenderHP;
    [SerializeField]
    private Text attackerDmg;
    [SerializeField]
    private Text attackerHit;
    [SerializeField]
    private Text attackerBonusDmg;
    [SerializeField]
    private Text attackerBonusHit;
    [SerializeField]
    private Image attackerHPBar;
    [SerializeField]
    private Image attackerLosingHPBar;
    [SerializeField]
    private Image defenderHPBar;
    [SerializeField]
    private Image defenderLosingHPBar;
    [SerializeField]
    private GameObject dodgeContainer;
    [SerializeField]
    private GameObject guardContainer;
    [SerializeField]
    private GameObject counterContainer;
    [SerializeField]
    private Color positiveColor;
    [SerializeField]
    private Color negativeColor;
    [SerializeField]
    private Color neutralColor;
    [SerializeField]
    private float healthSpeed;
    [SerializeField]
    private float healthLoseSpeed;
    [SerializeField]
    private GameObject reactTutorial;
    [SerializeField]
    private Text DodgeSPText;
    [SerializeField]
    private Slider dodgeSlider;
    [SerializeField]
    private Text dodgeValueText;
    [SerializeField]
    private Text dodgeMaxValueText;
    [SerializeField]
    private Slider guardSlider;
    [SerializeField]
    private Text guardSPText;
    [SerializeField]
    private Text guardValueText;
    [SerializeField]
    private Text guardMaxValueText;
    [SerializeField]
    private Text missedText;
    [SerializeField]
    private Text damageText;
    [SerializeField]
    private Text counterMissedText;
    [SerializeField]
    private Text counterDamageText;
    [SerializeField]
    private Slider counterAttackSlider;
    [SerializeField]
    private Text counterAttackMaxValueText;
    [SerializeField]
    private Text counterAttackValueText;
    [SerializeField]
    private Slider counterHitSlider;
    [SerializeField]
    private Text counterHitMaxValueText;
    [SerializeField]
    private Text counterHitValueText;
    [SerializeField]
    private Text counterSPText;
    [SerializeField]
    private GameObject infoMessage;


    private LivingObject attacker;
    private LivingObject defender;

    private float currentAllyHPValue;
    private float currentEnemyHPValue;
    private float delayedAllyHPValue;
    private float delayedEnemyHPValue;
    private float allyFillAmount = 1;
    private float allyLoseFillAmount = 1;
    private float enemyLoseFillAmount = 1;
    private float enemyFillAmount = 1;
    private float bonusDmg = 0;
    private float bonusHit = 0;
    private int StartSP;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        allyFillAmount = Mathf.Lerp(allyFillAmount, currentAllyHPValue, Time.deltaTime * healthSpeed);
        enemyFillAmount = Mathf.Lerp(enemyFillAmount, currentEnemyHPValue, Time.deltaTime * healthSpeed);
        allyLoseFillAmount = Mathf.Lerp(allyLoseFillAmount, delayedAllyHPValue, Time.deltaTime * healthLoseSpeed);
        enemyLoseFillAmount = Mathf.Lerp(enemyLoseFillAmount, delayedEnemyHPValue, Time.deltaTime * healthLoseSpeed);
        attackerHPBar.fillAmount = allyFillAmount;
        defenderHPBar.fillAmount = enemyFillAmount;
        defenderLosingHPBar.fillAmount = allyLoseFillAmount;
        attackerLosingHPBar.fillAmount = enemyLoseFillAmount;
    }
    private void OnEnable()
    {
        guardContainer.SetActive(false);
        dodgeContainer.SetActive(false);
        counterContainer.SetActive(false);
        reactTutorial.SetActive(true);
        HPValueChanged();
        EventContainer.hpValueChanged += HPValueChanged;
        EventContainer.reactUIVisible(true);
        attackerImage.sprite = attacker.Sprite;
        defenderImage.sprite = defender.Sprite;
        UpdateDmg();
    }

    private int currentDodgeValue;
    
    public void OnDodgeSliderValueChanged()
    {
        if (currentDodgeValue != (int)dodgeSlider.value)
        {
            defender.Stats.SP = defender.Stats.SP - ((int)dodgeSlider.value - currentDodgeValue);
            currentDodgeValue = (int)dodgeSlider.value;
        }
        dodgeSlider.maxValue = (int)Mathf.Clamp(dodgeSlider.value + defender.Stats.SP, 0, 3);
        bonusHit = -20 -10*currentDodgeValue;
        bonusDmg = 0;
        DodgeSPText.text = ""+ defender.Stats.SP;
        dodgeMaxValueText.text = "" +( -20 + -10 * (int)dodgeSlider.maxValue);
        dodgeValueText.text = "" + bonusHit;
        UpdateDmg();
    }
    private int currentGuardValue;
    public void OnGuardSliderValueChanged()
    {
        if (currentGuardValue != (int)guardSlider.value)
        {
            defender.Stats.SP = defender.Stats.SP - ((int)guardSlider.value - currentGuardValue);
            currentGuardValue = (int)guardSlider.value;
        }
        guardSlider.maxValue = (int)Mathf.Clamp(guardSlider.value + defender.Stats.SP, 0, 3);
        currentGuardValue = (int)guardSlider.value;
        bonusHit = +20 ;
        bonusDmg = -2 - currentGuardValue;
        guardSPText.text = "" + defender.Stats.SP;
        guardMaxValueText.text = "" + (-2 + -(int)guardSlider.maxValue);
        guardValueText.text = "" + bonusDmg;
        UpdateDmg();
    }
    private int currentCounterAttackValue;
    private int currentCounterHitValue;
    public void OnCounterSliderValueChanged()
    {
        if (currentCounterAttackValue != (int)counterAttackSlider.value)
        {
            defender.Stats.SP = defender.Stats.SP - ((int)counterAttackSlider.value - currentCounterAttackValue);
            currentCounterAttackValue = (int)counterAttackSlider.value;
        }
        counterAttackSlider.maxValue = (int)Mathf.Clamp(counterAttackSlider.value + defender.Stats.SP, 0, 3);
        currentCounterAttackValue = (int)counterAttackSlider.value;

        if (currentCounterHitValue != (int)counterHitSlider.value)
        {
            defender.Stats.SP = defender.Stats.SP - ((int)counterHitSlider.value - currentCounterHitValue);
            currentCounterHitValue = (int)counterHitSlider.value;
        }
        counterHitSlider.maxValue = (int)Mathf.Clamp(counterHitSlider.value + defender.Stats.SP, 0, 3);
        currentCounterHitValue = (int)counterHitSlider.value;


        bonusHit = 0;
        bonusDmg = 0;

        counterSPText.text = "" + defender.Stats.SP;

        int damage = defender.BattleStats.GetDamage() + 1 * currentCounterAttackValue;
        int dmg = (attacker.BattleStats.GetReceivedDamage(damage));
        int maxdmg = (attacker.BattleStats.GetReceivedDamage(defender.BattleStats.GetDamage() + (int)counterAttackSlider.maxValue));
        counterAttackMaxValueText.text = "" + maxdmg;
        counterAttackValueText.text = "" + dmg;
        int bonusDmg2 = 1 * currentCounterAttackValue;
        //if (EventContainer.attackerDmgChanged != null)
        //    EventContainer.attackerDmgChanged(bonusDmg2);


        int hit = Mathf.Clamp(defender.BattleStats.GetHitAgainstTarget(attacker) + 10 * currentCounterHitValue, 0, 100);
        counterHitMaxValueText.text = "" + Mathf.Clamp(defender.BattleStats.GetHitAgainstTarget(attacker) + counterHitSlider.maxValue * 10, 0, 100) + "%";
        counterHitValueText.text = "" + hit + "%";
        //if (EventContainer.attackerHitChanged != null)
        //    EventContainer.attackerHitChanged(hit);
        UpdateDmg();
    }
    private void UpdateDmg()
    {
        attackerDmg.text = ""+defender.BattleStats.GetReceivedDamage(attacker.BattleStats.GetDamage());
        attackerHit.text = "" + attacker.BattleStats.GetHitAgainstTarget(defender);
        
        if(bonusDmg==0)
            attackerBonusDmg.text = "";
        else
        {
            if (bonusDmg > 0)
            {
                attackerBonusDmg.text = "+ " + bonusDmg;
                attackerBonusDmg.color = positiveColor;
            }
            else
            {
                attackerBonusDmg.text = "- " + +Mathf.Abs(bonusDmg);
                attackerBonusDmg.color = negativeColor;
            }
        }
        if (bonusHit == 0)
            attackerBonusHit.text = "";
        else
        {
            if (bonusHit > 0)
            {
                attackerBonusHit.text = "+ " + bonusHit;
                attackerBonusHit.color = positiveColor;
            }
            else
            {
                attackerBonusHit.text = "- " +Mathf.Abs(bonusHit);
                attackerBonusHit.color = negativeColor;
            }
        }
    }

    private void OnDisable()
    {
        EventContainer.hpValueChanged -= HPValueChanged;
        EventContainer.reactUIVisible(false);
    }



    public void Show(LivingObject attacker, LivingObject defender)
    {
        StartSP = defender.Stats.SP;
        this.attacker = attacker;
        this.defender = defender;
        gameObject.SetActive(true);
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

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void DodgeConfirmed()
    {
        Debug.Log("DODGE");
        EventContainer.dodgeClicked(currentDodgeValue);
        EventContainer.attacktButtonCLicked();
    }
    public void GuardConfirmed()
    {
        EventContainer.guardClicked(currentGuardValue);
        EventContainer.attacktButtonCLicked();
    }
    public void CounterConfirmed()
    {
        EventContainer.counterClicked(currentCounterAttackValue,currentCounterHitValue);
        EventContainer.attacktButtonCLicked();
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
    public void DodgeClicked()
    {
        dodgeContainer.SetActive(true);
        counterContainer.SetActive(false);
        guardContainer.SetActive(false);
        infoMessage.SetActive(false);
    }
    public void GuardClicked()
    {
        guardContainer.SetActive(true);
        counterContainer.SetActive(false);
        dodgeContainer.SetActive(false);
        infoMessage.SetActive(false);
    }
    public void CounterClicked()
    {
        counterContainer.SetActive(true);
        guardContainer.SetActive(false);
        dodgeContainer.SetActive(false);
        infoMessage.SetActive(false);
    }
    
    #region COROUTINES
    IEnumerator DelayedAllyHP()
    {
        while (Mathf.Abs(enemyFillAmount - currentEnemyHPValue) >= HP_BAR_OFFSET_DELAY)
        {
            yield return null;
        }
        delayedAllyHPValue = MathUtility.MapValues(defender.Stats.HP, 0f, defender.Stats.MaxHP, 0f, 1f);
    }
    IEnumerator DelayedEnemyHP()
    {
        while (Mathf.Abs(allyFillAmount - currentAllyHPValue) >= HP_BAR_OFFSET_DELAY)
        {
            yield return null;
        }
        delayedEnemyHPValue = MathUtility.MapValues(attacker.Stats.HP, 0f, attacker.Stats.MaxHP, 0f, 1f);
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
            missedText.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha -= MISS_TEXT_FADE_OUT_SPEED;
            yield return new WaitForSeconds(0.01f);
        }
        alpha = 0;
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        text.gameObject.SetActive(false);

    }
    #endregion
}
