using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.GUI;
using Game.Mechanics.Battle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AttackData = Game.Mechanics.AttackData;

public class ChooseTargetUI : MonoBehaviour, IChooseTargetUI, IClickedReceiver
{
    private Canvas canvas;

    public GameObject SelectedCharacterCirclePrefab;

    public Transform CharacterCircleSpawnParent;

    public TextMeshProUGUI nameText;
    public Image icon;
    public TextMeshProUGUI descriptionText;
    public SelectionUI selectionUI;
  //  public LayoutGroup topLayout;
    [SerializeField]private AttackPreviewUI attackPreviewUI;
    public LayoutGroup bottomLayout;

    [SerializeField]private BattlePreview battlePreviewSo;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public event Action OnBackClicked;

    public void Show(Unit selectedUnit, ITargetableObject targetableObject)
    {
        CharacterCircleSpawnParent.DeleteAllChildren();
        var go = Instantiate(SelectedCharacterCirclePrefab, transform);
        var uiController = go.GetComponent<CharacterUIController>();
        uiController.parentController = this;
        uiController.ShowActive(selectedUnit);
           
        nameText.SetText(targetableObject.GetName());
        descriptionText.SetText(targetableObject.GetDescription());
        icon.sprite=targetableObject.GetIcon();
  
        //LayoutRebuilder.ForceRebuildLayoutImmediate(topLayout.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(bottomLayout.transform as RectTransform);
        canvas.enabled = true;
        selectionUI.ShowUndo();
    }

    public void Hide()
    {
        canvas.enabled = false;
        CharacterCircleSpawnParent.DeleteAllChildren();
        selectionUI.HideUndo();
    }

    public void ShowSkillPreview(SingleTargetMixin stm, Unit selectedUnit, Unit target)
    {
      
        int totalDamage = stm.GetDamageDone(selectedUnit, target);
        int totalHeal = stm.GetHealingDone(selectedUnit, target);
        int hpAfter = selectedUnit.Hp - stm.GetHpCost(stm.skill.level);
        NoNameYet(selectedUnit, target,stm.skill.Name,totalDamage,totalHeal , hpAfter);
       
    }

    private void NoNameYet(Unit selectedUnit, Unit target,string skillName, int totalDamage, int totalHeal, int hpAfter)
    {
        Debug.Log("TOTALHEAL: "+totalHeal);
        battlePreviewSo.Attacker = selectedUnit;
        battlePreviewSo.Defender = target;
        int hit = 100;
        int crit = 0;
        int attackCount = 1;
     
        int hpTargetAfter = target.Hp - totalDamage + totalHeal;
        if (hpTargetAfter < 0)
            hpTargetAfter = 0;
        if (hpTargetAfter > target.MaxHp)
            hpTargetAfter = target.MaxHp;
        battlePreviewSo.AttackerStats = new BattlePreviewStats(0, 0, 0, 0, 0, totalHeal,totalDamage, 100, crit, attackCount,
            selectedUnit.Hp, selectedUnit.MaxHp, hpAfter, true);
        battlePreviewSo.DefenderStats = new BattlePreviewStats(0, 0, 0, 0, 0,0, 0, 0, 0, 0,
            target.Hp, target.MaxHp, hpTargetAfter, false);
        battlePreviewSo.AttacksData = new List<AttackData>();
        battlePreviewSo.AttacksData.Add(new AttackData()
        {
            attacker =  true,
            Dmg = totalDamage,
            Heal = totalHeal,
            hit=true,
            crit=false,
            activatedAttackSkills =  new List<Skill>(),
            activatedDefenseSkills = new List<Skill>()
        });
        attackPreviewUI.Show(battlePreviewSo, selectedUnit, target, skillName);
    }

    public void ShowSkillPreview(IPosTargeted posTargetSkill, Unit selectedUnit, Unit target)
    {
        int totalDamage = posTargetSkill.GetDamageDone(selectedUnit, target);
        int totalHeal = posTargetSkill.GetHealingDone(selectedUnit, target);
        int hpAfter = selectedUnit.Hp - posTargetSkill.GetHpCost();
        NoNameYet(selectedUnit, target,posTargetSkill.GetName(), totalDamage, totalHeal, hpAfter);
    }

    public void HideSkillPreview(SingleTargetMixin stm)
    {
        attackPreviewUI.Hide();
    }

    public void HideSkillPreview(IPosTargeted posTargetSkill)
    {
        attackPreviewUI.Hide();
    }

    public void BackClicked()
    {
        OnBackClicked?.Invoke();
    }

    public void Clicked(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public void PlusClicked(Unit unit)
    {
        //
    }
}
