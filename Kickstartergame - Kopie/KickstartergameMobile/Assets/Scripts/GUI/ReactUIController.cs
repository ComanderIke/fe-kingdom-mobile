using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactUIController : MonoBehaviour {
    const float HP_BAR_OFFSET_DELAY = 0.005f;
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
    private Slider dodgeSlider;
    [SerializeField]
    private Slider guardSlider;
    [SerializeField]
    private Slider counterAttackSlider;
    [SerializeField]
    private Slider counterHitSlider;

    private LivingObject attacker;
    private LivingObject defender;

    private float currentAllyHPValue;
    private float currentEnemyHPValue;
    private float delayedAllyHPValue;
    private float allyFillAmount = 1;
    private float allyLoseFillAmount = 1;
    private float enemyFillAmount = 1;
    private float bonusDmg = 0;
    private float bonusHit = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        allyFillAmount = Mathf.Lerp(allyFillAmount, currentAllyHPValue, Time.deltaTime * healthSpeed);
        enemyFillAmount = Mathf.Lerp(enemyFillAmount, currentEnemyHPValue, Time.deltaTime * healthSpeed);
        allyLoseFillAmount = Mathf.Lerp(allyLoseFillAmount, delayedAllyHPValue, Time.deltaTime * healthLoseSpeed);
        attackerHPBar.fillAmount = allyFillAmount;
        defenderHPBar.fillAmount = enemyFillAmount;
        defenderLosingHPBar.fillAmount = allyLoseFillAmount;
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


    public void OnDodgeSliderValueChanged()
    {
        int currentDodgeValue = (int)dodgeSlider.value;
        bonusHit = -20 -10*currentDodgeValue;
        bonusDmg = 0;
    }
    public void OnGuardSliderValueChanged()
    {
        int currentDodgeValue = (int)dodgeSlider.value;
        bonusHit = -20 - 10 * currentDodgeValue;
        bonusDmg = 0;
    }
    public void OnCounterSliderValueChanged()
    {
        int currentDodgeValue = (int)dodgeSlider.value;
        bonusHit = -20 - 10 * currentDodgeValue;
        bonusDmg = 0;
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

        StartCoroutine(DelayedHP());
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void DodgeConfirmed()
    {

    }
    public void GuardConfirmed()
    {

    }
    public void CounterConfirmed()
    {

    }
    public void DodgeClicked()
    {
        dodgeContainer.SetActive(true);
        counterContainer.SetActive(false);
        guardContainer.SetActive(false);
        reactTutorial.SetActive(false);
        OnDodgeSliderValueChanged();
    }
    public void GuardClicked()
    {
        guardContainer.SetActive(true);
        counterContainer.SetActive(false);
        dodgeContainer.SetActive(false);
        reactTutorial.SetActive(false);
        bonusHit = +20;
        bonusDmg = -2;
        UpdateDmg();
    }
    public void CounterClicked()
    {
        counterContainer.SetActive(true);
        guardContainer.SetActive(false);
        dodgeContainer.SetActive(false);
        reactTutorial.SetActive(false);
        bonusHit = 0;
        bonusDmg = 0;
        UpdateDmg();
    }
    #region COROUTINES
    IEnumerator DelayedHP()
    {
        while (Mathf.Abs(enemyFillAmount - currentEnemyHPValue) >= HP_BAR_OFFSET_DELAY)
        {
            yield return null;
        }
        delayedAllyHPValue = MathUtility.MapValues(defender.Stats.HP, 0f, defender.Stats.MaxHP, 0f, 1f);
    }
    #endregion
}
