using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using Assets.Scripts.Utility;
using System.Collections;
using TMPro;
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
    private TextMeshProUGUI attackerHP;
    [SerializeField]
    private TextMeshProUGUI defenderHP;
    [SerializeField]
    private TextMeshProUGUI attackerDmg;
    [SerializeField]
    private Image attackerHPBar;
    [SerializeField]
    private Image attackerLosingHPBar;
    [SerializeField]
    private Image defenderHPBar;
    [SerializeField]
    private Image defenderLosingHPBar;
    [SerializeField]
    private float healthSpeed;
    [SerializeField]
    private float healthLoseSpeed;
    [SerializeField]
    Transform targetPointParent;
    [SerializeField]
    private TextMeshProUGUI dodgeValueText;
    [SerializeField]
    private TextMeshProUGUI guardValueText;
    [SerializeField]
    private TextMeshProUGUI counterDmgText;


    private Unit attacker;
    private Unit defender;

    private float currentAllyHPValue;
    private float currentEnemyHPValue;
    private float delayedAllyHPValue;
    private float delayedEnemyHPValue;
    private float allyFillAmount = 1;
    private float allyLoseFillAmount = 1;
    private float enemyLoseFillAmount = 1;
    private float enemyFillAmount = 1;
	
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
        HPValueChanged();
        Unit.onHpValueChanged += HPValueChanged;
        UISystem.onReactUIVisible(true);
        attackerImage.sprite = attacker.Sprite;
        defenderImage.sprite = defender.Sprite;
        UpdateDmg();
    }

    private int currentDodgeValue;
    private int currentGuardValue;
    private int currentCounterAttackValue;
    private int currentCounterHitValue;

    private void UpdateDmg()
    {
        attackerDmg.text = ""+defender.BattleStats.GetReceivedDamage(attacker.BattleStats.GetDamage())+ " incoming damage";
    }

    private void OnDisable()
    {
        Unit.onHpValueChanged -= HPValueChanged;
        UISystem.onReactUIVisible(false);
    }

    public void ShowCounterMissText()
    {
        MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextGreen("Missed", attackerImage.transform);
    }
    public void ShowCounterDamageText(int damage)
    {
        MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextRed("" + damage, attackerImage.transform);
    }
    public void ShowMissText()
    {
        MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextGreen("Missed", targetPointParent.transform);
    }
    public void ShowDamageText(int damage, bool magic = false)
    {
        if (magic)
            MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextBlue("" + damage, targetPointParent.transform);
        else
            MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextRed("" + damage, targetPointParent.transform);


    }

    public void Show(Unit attacker, Unit defender)
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

        StartCoroutine(DelayedAllyHP());
        StartCoroutine(DelayedEnemyHP());
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void DodgeConfirmed()
    {
        UISystem.onDodgeClicked();
        FightState.onStartAttack();
    }
    public void GuardConfirmed()
    {
        UISystem.onGuardClicked();
        FightState.onStartAttack();
    }
    public void CounterConfirmed()
    {
        UISystem.onCounterClicked();
        FightState.onStartAttack();
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
   
    #endregion
}
