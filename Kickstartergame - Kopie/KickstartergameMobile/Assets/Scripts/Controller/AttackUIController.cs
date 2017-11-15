using Assets.Scripts.Characters;
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
    private Text attackerHP;
    [SerializeField]
    private Text defenderHP;
    [SerializeField]
    private Text attackerDMG;
    [SerializeField]
    private Text attackerHIT;
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
    private Image attackerHPBar;
    [SerializeField]
    private Image defenderHPBar;
    [SerializeField]
    private Image defenderLosingHPBar;
    [SerializeField]
    private Slider hitSlider;
    [SerializeField]
    private Slider attackSlider;
    [Header("Configuration")]
    [SerializeField]
    private float healthSpeed;
    [SerializeField]
    private float healthLoseSpeed;

    private float currentAllyHPValue;
    private float currentEnemyHPValue;
    private float delayedEnemyHPValue=1;
    private float allyFillAmount = 1;
    private float enemyFillAmount = 1;
    private float enemyLoseFillAmount = 1;
    private int currentHitSliderValue = 0;
    private int currentAtkSliderValue = 0;
    private LivingObject attacker;
    private LivingObject defender;

    void Start () {
        LivingObject.hpValueChanged += HPValueChanged;
	}

    void OnEnable()
    {
        OnSliderValueChanged();
        HPValueChanged();
    }

	void Update () {
        allyFillAmount = Mathf.Lerp(allyFillAmount, currentAllyHPValue, Time.deltaTime * healthSpeed);
        enemyFillAmount = Mathf.Lerp(enemyFillAmount, currentEnemyHPValue, Time.deltaTime * healthSpeed);
        enemyLoseFillAmount = Mathf.Lerp(enemyLoseFillAmount, delayedEnemyHPValue, Time.deltaTime * healthLoseSpeed);
        attackerHPBar.fillAmount = allyFillAmount;
        defenderHPBar.fillAmount = enemyFillAmount;
        defenderLosingHPBar.fillAmount = enemyLoseFillAmount;
    }
    
    public void Show(LivingObject attacker, LivingObject defender)
    {
        this.attacker = attacker;
        this.defender = defender;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowMissText()
    {
        StartCoroutine(MissTextAnimation());
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
        attackerSP.text = "" + attacker.Stats.SP;
        attackerSP2.text = "" + attacker.Stats.SP;
        attackerMaxDMG.text = "" + (attacker.BattleStats.GetDamageAgainstTarget(defender) + 3);
        attackerDMG.text = "" + (attacker.BattleStats.GetDamageAgainstTarget(defender) + 1 * currentAtkSliderValue);
        attackerMaxHIT.text = "" + Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender) + 30, 0, 100) + "%";
        attackerHIT.text = "" + Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender) + 10 * currentHitSliderValue, 0, 100) + "%";
        Debug.Log(attacker.BattleStats.GetHitAgainstTarget(defender));
    }

    private void HPValueChanged()
    {
        defenderHP.text = defender.Stats.HP + " / " + defender.Stats.MaxHP;
        attackerHP.text = attacker.Stats.HP + " / " + attacker.Stats.MaxHP;
        currentAllyHPValue = MathUtility.MapValues(attacker.Stats.HP, 0f, attacker.Stats.MaxHP, 0f, 1f);
        currentEnemyHPValue = MathUtility.MapValues(defender.Stats.HP, 0f, defender.Stats.MaxHP, 0f, 1f);

        StartCoroutine(DelayedHP());
    }

    #region COROUTINES
    IEnumerator DelayedHP()
    {
        while (Mathf.Abs(enemyFillAmount - currentEnemyHPValue) >= HP_BAR_OFFSET_DELAY)
        {
            yield return null;
        }
        delayedEnemyHPValue = MathUtility.MapValues(defender.Stats.HP, 0f, defender.Stats.MaxHP, 0f, 1f);
    }

    IEnumerator MissTextAnimation()
    {
        float alpha = 0;
        missedText.gameObject.SetActive(true);
        missedText.color = new Color(missedText.color.r, missedText.color.g, missedText.color.b, alpha);
        while (alpha < 1)
        {
            missedText.color = new Color(missedText.color.r, missedText.color.g, missedText.color.b, alpha);
            alpha += MISS_TEXT_FADE_IN_SPEED;
            yield return new WaitForSeconds(0.01f);
        }
        alpha = 1;
        missedText.color = new Color(missedText.color.r, missedText.color.g, missedText.color.b, alpha);
        yield return new WaitForSeconds(MISS_TEXT_VISIBLE_DURATION);
        while (alpha > 0)
        {
            missedText.color = new Color(missedText.color.r, missedText.color.g, missedText.color.b, alpha);
            alpha -= MISS_TEXT_FADE_OUT_SPEED;
            yield return new WaitForSeconds(0.01f);
        }
        alpha = 0;
        missedText.color = new Color(missedText.color.r, missedText.color.g, missedText.color.b, alpha);
        missedText.gameObject.SetActive(false);

    }
    #endregion
}
