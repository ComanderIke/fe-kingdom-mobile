using Assets.Scripts.Characters;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Characters.Monsters;
using TMPro;
using Assets.Scripts.GameStates;

public class AttackUIController : MonoBehaviour {

    public const float HP_BAR_OFFSET_DELAY = 0.005f;
    public const float MISS_TEXT_FADE_IN_SPEED = 0.12f;
    public const float MISS_TEXT_FADE_OUT_SPEED = 0.02f;
    public const float MISS_TEXT_VISIBLE_DURATION = 1.5f;


    
    
    #region InspectorFields
    [Header("Input Fields")]
    [SerializeField]
    Transform targetPointParent;

    [SerializeField]
    Image attackerSprite;
    [SerializeField]
    Image defenderSprite;
    [SerializeField]
    private TextMeshProUGUI attackerHP;
    [SerializeField]
    private TextMeshProUGUI defenderHP;
    [SerializeField]
    private TextMeshProUGUI attackCountText;

    [Header("Fast Attack")]
    [SerializeField]
    private TextMeshProUGUI attackerDMG;
    [SerializeField]
    private TextMeshProUGUI attackerHIT;

    [Header("Special Attack")]
    [SerializeField]
    private TextMeshProUGUI specialAttackDMG;
    [SerializeField]
    private TextMeshProUGUI specialAttackHit;
    [Header("Strong Attack")]
    [SerializeField]
    private TextMeshProUGUI strongAttackDamage;
    [SerializeField]
    private TextMeshProUGUI strongAttackHit;
    [SerializeField]
    private Image attackerHPBar;
    [SerializeField]
    private Image attackerLosingHPBar;
    [SerializeField]
    private Image defenderHPBar;
    [SerializeField]
    private Image defenderLosingHPBar;
    [SerializeField]
    private GameObject AttackContainer;
    [SerializeField]
    private GameObject frontalAttackGO;
    [SerializeField]
    private GameObject surpriseAttackGO;
    [Header("Configuration")]
    [SerializeField]
    private float healthSpeed;
    [SerializeField]
    private float healthLoseSpeed;
    #endregion

    private int attackCount;
    private float currentAllyHPValue;
    private float currentEnemyHPValue;
    private float delayedAllyHPValue = 1;
    private float delayedEnemyHPValue=1;
    private float allyFillAmount = 1;
    private float enemyFillAmount = 1;
    private float allyLoseFillAmount = 1;
    private float enemyLoseFillAmount = 1;
    private Unit attacker;
    private Unit defender;
    private TargetPoint attackTarget;
    private AttackType attackType;
    private bool frontAttack = false;
    private bool surpriseAttack = false;

    void Start () {
       
	}
   
    void OnEnable()
    {

       // attackTutorial.SetActive(true);
        HPValueChanged();

        Unit.onHpValueChanged += HPValueChanged;
        UISystem.onAttackUIVisible(true);
        UISystem.onFrontalAttackAnimationEnd += EnableSwipeAttack;
        //attackerSprite.sprite = attacker.Sprite;
        defenderSprite.sprite = defender.Sprite;
        attackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
    }
    
    private void OnDisable()
    {
        Unit.onHpValueChanged -= HPValueChanged;
        UISystem.onAttackUIVisible(false);

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
            attackCountText.text = attackCount+" Attack";
        else
            attackCountText.text = attackCount + " Attacks";
    }

    void EnableSwipeAttack()
    {
        StartCoroutine(ActivateSwipeAttack(0.2f));
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
            frontalAttackGO.SetActive(true);
        }
        else if (attacker.BattleStats.IsBackSideAttack(defender))
        {
            surpriseAttack = true;
            surpriseAttackGO.SetActive(true);
        }
        else
            StartCoroutine(ActivateSwipeAttack(.3f));
       
        //chooseTargetTutorial.SetActive(true);
        foreach (Transform child in targetPointParent.GetComponentsInChildren<Transform>())
        {
            if(child!=targetPointParent)
                GameObject.Destroy(child.gameObject);
        }
        UpdateDamage();
        UpdateHit();
        UpdateSpecial();
    }

    public void Hide()
    {
        StartCoroutine(DelayHide(0.25f));
        
    }
    IEnumerator DelayHide(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    IEnumerator ActivateSwipeAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        AttackContainer.SetActive(true);
        //targetPointParent.gameObject.SetActive(true);
    }
    public void StrongAttack()
    {
        Debug.Log("Strong Attack!");
        attackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "StrongAttack");
        StartAttack(attackType);
    }
    public void FastAttack()
    {
        Debug.Log("Fast Attack!");
        attackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "FastAttack");
        StartAttack(attackType);
    }
    public void SpecialAttack()
    {
        Debug.Log("Special Attack!");
        attackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "SpecialAttack");
        StartAttack(attackType);
    }

  
    public void AttackButtonCLicked()
    {
        StartAttack(attackType);
    }
    public void StartAttack(AttackType attackType)
    {
        AttackContainer.SetActive(false);

        if (attackCount > 0)
        {
            Debug.Log("StartAttack!");
            FightState.onStartAttack(attackType);
            attackCount--;
        }
        if(attackCount <=0)
        {
            AttackContainer.SetActive(false);
        }
        else
            StartCoroutine(ActivateSwipeAttack(0.35f));
    }

    public void ShowCounterMissText()
    {
        MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextGreen("Missed", attackerSprite.transform);
    }
    public void ShowCounterDamageText(int damage)
    {
        MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextRed("" + damage, attackerSprite.transform);
    }
    public void ShowMissText()
    {
        //activeTarget.ShowMissedText();
        //FindObjectOfType<PopUpTextController>().CreateAttackPopUpTextGreen("Missed", activeTargetPoint.transform);
        MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextGreen("Missed", targetPointParent.transform);
    }
    public void ShowDamageText(int damage, bool magic=false)
    {
        if(magic)
            MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextBlue("" + damage, targetPointParent.transform);
        else
            MainScript.instance.GetSystem<PopUpTextSystem>().CreateAttackPopUpTextRed("" + damage, targetPointParent.transform);
        //activeTarget.ShowDamageText(damage);

    }


    void UpdateHit()
    {
        Human humanAttacker = (Human)attacker;
        int bonusHit = 0;
        if (surpriseAttack)
        {
            bonusHit += humanAttacker.BattleStats.SurpriseAttackBonusHit;
        }
        AttackType fastAttackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "FastAttack");
        int hitInfluence = bonusHit;// attackTarget.HIT_INFLUENCE;
        hitInfluence += fastAttackType.Hit;
        int hit = Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender)+hitInfluence, 0, 100);// + 10 * currentHitSliderValue, 0, 100);
        attackerHIT.text = "" + hit + "%";

        AttackType strongAttackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "StrongAttack");
        hitInfluence = bonusHit;// attackTarget.HIT_INFLUENCE;
        hitInfluence += strongAttackType.Hit;
        hit = Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender) + hitInfluence, 0, 100);// + 10 * currentHitSliderValue, 0, 100);
        strongAttackHit.text = "" + hit + "%";


        //if (EventContainer.attackerHitChanged != null)
        //    EventContainer.attackerHitChanged(hit);
    }
    void UpdateDamage()
    {
        Human humanAttacker = (Human)attacker;
        float multiplier = 1;//attackTarget.DamageMultiplier;
        if (frontAttack)
        {
            multiplier = humanAttacker.BattleStats.FrontalAttackModifier;
        }
        AttackType fastAttackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "FastAttack");
        
        List<float> attackMultiplier = new List<float>();
        attackMultiplier.Add(multiplier);
        attackMultiplier.Add(fastAttackType.DamageMultiplier);
        int damage = attacker.BattleStats.GetDamage(attackMultiplier);
        int dmg = (int)((defender.BattleStats.GetReceivedDamage(damage)));
        attackerDMG.text = "" + dmg;
      
        AttackType strongAttackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "StrongAttack");

        attackMultiplier.Clear();
        attackMultiplier.Add(multiplier);
        attackMultiplier.Add(strongAttackType.DamageMultiplier);
        damage = attacker.BattleStats.GetDamage(attackMultiplier);
        dmg = (int)((defender.BattleStats.GetReceivedDamage(damage)));
        strongAttackDamage.text = "" + dmg;
    }
    void UpdateSpecial()
    {
        Human humanAttacker = (Human)attacker;
        float multiplier = 1;//attackTarget.DamageMultiplier;
        if (frontAttack)
        {
            multiplier = humanAttacker.BattleStats.FrontalAttackModifier;
        }
        
        List<float> attackMultiplier = new List<float>();
        attackMultiplier.Add(multiplier);
        int damage = attacker.BattleStats.GetDamage(attackMultiplier);
        int dmg = (int)(humanAttacker.SpecialAttackManager.equippedSpecial.GetSpecialDmg(attacker, damage,defender));
        specialAttackDMG.text = "" + dmg;
        int bonusHit = 0;
        if (surpriseAttack)
        {
            bonusHit += humanAttacker.BattleStats.SurpriseAttackBonusHit;
        }
        int hitInfluence = bonusHit;// attackTarget.HIT_INFLUENCE;
        int normalHit = attacker.BattleStats.GetHitAgainstTarget(defender)+hitInfluence;
        
        int hit = Mathf.Clamp(humanAttacker.SpecialAttackManager.equippedSpecial.GetSpecialHit(attacker, normalHit, defender), 0, 100);// + 10 * currentHitSliderValue, 0, 100);
        specialAttackHit.text = "" + hit + "%";
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

  
    #endregion
}
