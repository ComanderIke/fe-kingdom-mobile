using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.Characters.Monsters;
using TMPro;

public class AttackUIController : MonoBehaviour {

    public const float HP_BAR_OFFSET_DELAY = 0.005f;
    public const float MISS_TEXT_FADE_IN_SPEED = 0.12f;
    public const float MISS_TEXT_FADE_OUT_SPEED = 0.02f;
    public const float MISS_TEXT_VISIBLE_DURATION = 1.5f;


    
    
    #region InspectorFields
    [Header("Input Fields")]
    [SerializeField]
    Animator animator;
    [SerializeField]
    GameObject targetPointPrefab;
    [SerializeField]
    Transform targetPointParent;
    [SerializeField]
    GameObject targetInfoObject;
    [SerializeField]
    GameObject attackTextGO;
    [SerializeField]
    GameObject chooseTargetTutorial;
    [SerializeField]
    AttackPatternUI attackReactionUI;
    [SerializeField]
    Image attackerSprite;
    [SerializeField]
    Image defenderSprite;
    [SerializeField]
    private TextMeshProUGUI attackerHP;
    [SerializeField]
    private TextMeshProUGUI defenderHP;
    [SerializeField]
    private TextMeshProUGUI attackerDMG;
    [SerializeField]
    private TextMeshProUGUI targetName;
    [SerializeField]
    private TextMeshProUGUI attackerHIT;
    [SerializeField]
    private TextMeshProUGUI attackCountText;
    [SerializeField]
    private TextMeshProUGUI attackCountTextText;
    [SerializeField]
    private Text attackerMaxDMG;
    [SerializeField]
    private TextMeshProUGUI strongAttackDamage;
    [SerializeField]
    private TextMeshProUGUI strongAttackHit;
    [SerializeField]
    private Text attackerMaxHIT;
    [SerializeField]
    private Text attackerSP;
    [SerializeField]
    private Text attackerSP2;
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
    private int currentHitSliderValue = 0;
    private int currentAtkSliderValue = 0;
    private LivingObject attacker;
    private LivingObject defender;
    private AttackTargetPoint activeTargetPoint;
    private TargetPoint attackTarget;
    private List<AttackTargetPoint> targetPoints;
    private AttackType attackType;

    void Start () {
       
	}

    void OnEnable()
    {

       // attackTutorial.SetActive(true);
        HPValueChanged();
        swipeAttackGO.SetActive(true);
        EventContainer.hpValueChanged += HPValueChanged;
        EventContainer.attackUIVisible(true);
        EventContainer.swipeIdleEvent += ShowIdleAttack;
        
        //attackerSprite.sprite = attacker.Sprite;
        defenderSprite.sprite = defender.Sprite;
        attackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
    }
    
    private void OnDisable()
    {
        EventContainer.hpValueChanged -= HPValueChanged;
        EventContainer.attackUIVisible(false);
        EventContainer.swipeIdleEvent -= ShowIdleAttack;

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
        attackCountText.text = "" + attackCount;
        if (attackCount == 1)
            attackCountTextText.text = "Attack";
        else
            attackCountTextText.text = "Attacks";
    }
    
    public void Show(LivingObject attacker, LivingObject defender)
    {
        this.attacker = attacker;
        this.defender = defender;
        attackButton.interactable = true;
        gameObject.SetActive(true);
        chooseTargetTutorial.SetActive(true);
        swipeAttackGO.SetActive(false);
        currentAtkSliderValue = 0;
        currentHitSliderValue = 0;
        hitSlider.value = 0;
        attackSlider.value = 0;
        foreach(Transform child in targetPointParent.GetComponentsInChildren<Transform>())
        {
            if(child!=targetPointParent)
                GameObject.Destroy(child.gameObject);
        }
        targetPoints = new List<AttackTargetPoint>();
        int cnt = 0;
        if (defender is Monster)
        {
            Monster m = (Monster)defender;
            foreach(TargetPoint p in m.TargetPoints)
            {
                GameObject instantiatedPoint = GameObject.Instantiate(targetPointPrefab, targetPointParent);
                instantiatedPoint.transform.localPosition = new Vector3(p.XPos, p.YPos, 0);
                instantiatedPoint.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * p.Scale, 100 * p.Scale);
                instantiatedPoint.GetComponent<AttackTargetPoint>().ID = cnt++;
                targetPoints.Add(instantiatedPoint.GetComponent<AttackTargetPoint>());
            }
        }
    }

    public void Hide()
    {
        StartCoroutine(DelayHide(0.25f));
        animator.SetTrigger("Outro");
    }
    public void ShowAttackReaction(string user, string reactionName)
    {
        Debug.Log("Show Reaction");

        attackReactionUI.Show(user, reactionName);
    }
    IEnumerator DelayHide(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    IEnumerator ActivateSwipeAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        swipeAttackGO.SetActive(true);
        //targetPointParent.gameObject.SetActive(true);
    }
    public void SetAttackCountText()
    {

    }
    public void StrongAttack()
    {
        Human human = (Human)attacker;
        Debug.Log("Strong Attack!");
        attackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "StrongAttack");
        StartAttack(attackType);
    }
    public void FastAttack()
    {
        Human human = (Human)attacker;
        Debug.Log("Fast Attack!");
        attackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "FastAttack");
        StartAttack(attackType);
    }
    public void ResetAttack()
    {
        attackButton.gameObject.SetActive(false);
       // attackTextGO.SetActive(false);
    }
    public void StrongAttackPreview()
    {
        attackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "StrongAttack");
        UpdateHit();
        UpdateDamage();
        attackButton.gameObject.SetActive(true);
        //attackTextGO.SetActive(true);
    }
    public void FastAttackPreview()
    {
        attackType = attacker.GetType<Human>().AttackTypes.Find(a => a.Name == "FastAttack");
        UpdateHit();
        UpdateDamage();
        attackButton.gameObject.SetActive(true);
        //attackTextGO.SetActive(true);
    }
    public void AttackButtonCLicked()
    {
        StartAttack(attackType);
    }
    public void StartAttack(AttackType attackType)
    {
        swipeAttackGO.SetActive(false);
        attackButton.gameObject.SetActive(false);
        foreach (AttackTargetPoint point in targetPoints)
        {
            if (point != activeTargetPoint)
            {
                point.gameObject.SetActive(false);
            }
        }
        if (attackCount > 0)
        {
            Debug.Log("StartAttack!");
            EventContainer.startAttack(attackType, attackTarget);
            attackCount--;
        }
        if(attackCount <=0)
        {
            swipeAttackGO.SetActive(false);
            activeTargetPoint.gameObject.SetActive(false);
            //targetInfoObject.SetActive(false);
        }
        else
            StartCoroutine(ActivateSwipeAttack(0.35f));
    }
    public void TargetPointSelected(int id)
    {
        swipeAttackGO.SetActive(true);
        chooseTargetTutorial.SetActive(false);
       // targetInfoObject.SetActive(true);
        
        foreach (AttackTargetPoint point in targetPoints)
        {
            if (id == point.ID)
            {
                point.PlayAnimation();
                activeTargetPoint = point;
            }
            else
            {
                point.StopAnimation();
            }
        }
        attackTarget = ((Monster)defender).TargetPoints[activeTargetPoint.ID];
        targetName.text = attackTarget.Name;
        UpdateDamage();
        UpdateHit();
    }
    void ShowIdleAttack()
    {
        idleAttackGO.SetActive(true);
        strongAttackGO.SetActive(false);
        fastAttackGO.SetActive(false);
    }
    public void ShowCounterMissText()
    {
        FindObjectOfType<PopUpTextController>().CreateAttackPopUpTextGreen("Missed", attackerSprite.transform);
    }
    public void ShowCounterDamageText(int damage)
    {
        FindObjectOfType<PopUpTextController>().CreateAttackPopUpTextRed("" + damage, attackerSprite.transform);
    }
    public void ShowMissText()
    {
        //activeTarget.ShowMissedText();
        FindObjectOfType<PopUpTextController>().CreateAttackPopUpTextGreen("Missed", activeTargetPoint.transform);
    }
    public void ShowDamageText(int damage)
    {
        FindObjectOfType<PopUpTextController>().CreateAttackPopUpTextRed("" + damage, activeTargetPoint.transform);
        //activeTarget.ShowDamageText(damage);
        
    }

    void UpdateHit()
    {
        int hitInfluence = attackTarget.HIT_INFLUENCE;
        if (attackType != null)
            hitInfluence += attackType.Hit;

        int hit = Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender)+hitInfluence, 0, 100);// + 10 * currentHitSliderValue, 0, 100);
       // attackerMaxHIT.text = "" + Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender) + hitSlider.maxValue * 10, 0, 100) + "%";
        attackerHIT.text = "" + hit + "%";
        if (hit >= 80)
        {
            attackerHIT.color = FindObjectOfType<ColorContainer>().mainGreenColor;
        }
        else if(hit <=40)
        {
            attackerHIT.color = FindObjectOfType<ColorContainer>().mainRedColor;
        }
        else
        {
            attackerHIT.color = FindObjectOfType<ColorContainer>().mainWhiteColor;
        }
        if (EventContainer.attackerHitChanged != null)
            EventContainer.attackerHitChanged(hit);
        if (attackType != null)
            strongAttackHit.text = Mathf.Clamp(attacker.BattleStats.GetHitAgainstTarget(defender)+attackType.Hit,0,100) + " %";
        //attackerSP2.text = "" + attacker.Stats.SP;
    }
    void UpdateDamage()
    {
        float multiplier = attackTarget.DamageMultiplier;
        List<float> attackMultiplier = new List<float>();
        attackMultiplier.Add(multiplier);
        if(attackType!=null)
            attackMultiplier.Add(attackType.DamageMultiplier);
        int damage = attacker.BattleStats.GetDamage(attackMultiplier);
        int dmg = (int)((defender.BattleStats.GetReceivedDamage(damage)));
        attackerDMG.text = "" + dmg;
        Human human =(Human) attacker;
        strongAttackDamage.text = "" + (attacker.BattleStats.GetDamageAgainstTarget(defender, attackMultiplier));
        if (dmg >= 5)
        {
            attackerDMG.color = FindObjectOfType<ColorContainer>().mainGreenColor;
        }
        else if (dmg <= 1)
        {
            attackerDMG.color = FindObjectOfType<ColorContainer>().mainRedColor;
        }
        else
        {
            attackerDMG.color = FindObjectOfType<ColorContainer>().mainWhiteColor;
        }
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
