using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.States;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Utility;


public interface ISkillUIRenderer:IAnimation
{
    public event Action<Skill> onSkillChosen;
    public void Show(Unit unit, Skill skill1, Skill skill2, Skill skill3);
    public void Hide();
    public Action OnFinished { get; set; }
}
public class ChooseSkillUI : MonoBehaviour, ISkillUIRenderer
{
   
    public Action OnFinished { get; set; }


    [SerializeField] private SkillsUI skillsUI;

    [SerializeField] private UIAnimationSpriteSwapper uiAnimation;
    [SerializeField] private Animator uIIdleAnimation;
    [SerializeField] private ChooseSkillButtonUI chooseSkill1;
    [SerializeField] private ChooseSkillButtonUI chooseSkill2;
    [SerializeField] private ChooseSkillButtonUI chooseSkill3;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI headline;

    public event Action<Skill> onSkillChosen;
    private Unit unit;
    private Skill skill1, skill2, skill3;
    public void Show(Unit unit, Skill skill1, Skill skill2, Skill skill3)
    {
        BottomUI.Hide();
        PlayerPhaseUI.StaticHide();
        canvasGroup.gameObject.SetActive(true);
        TweenUtility.FadeIn(canvasGroup);
        this.unit = unit;
        this.skill1 = skill1;
        this.skill2 = skill2;
        skillChosen = false;
        this.skill3 = skill3;
        selected = null;
        chooseSkill1.OnClick -= SkillButtonClicked;
        chooseSkill2.OnClick -= SkillButtonClicked;
        chooseSkill3.OnClick -= SkillButtonClicked;
        chooseSkill1.OnClick += SkillButtonClicked;
        chooseSkill2.OnClick += SkillButtonClicked;
        chooseSkill3.OnClick += SkillButtonClicked;
        uiAnimation.Init(unit.visuals.CharacterSpriteSet);
        uIIdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
        unit.SkillManager.OnSkillsChanged -= UpdateUI;
        unit.SkillManager.OnSkillsChanged += UpdateUI;
        UpdateUI();
    }

    private ChooseSkillButtonUI selected;
    void SkillButtonClicked(ChooseSkillButtonUI button)
    {
        if (selected != null)
            selected.Deselect();
        selected = button;
        button.Select();
    }

    private bool skillChosen = false;
    public void ChooseClicked()
    {
        if (selected == null||skillChosen)
            return;
        skillChosen = true;
        Debug.Log("SkillSelected");

        if (unit != null)
            skillCount = unit.SkillManager.Skills.Count;
        int index = skillCount;
        if (selected.upgrade)
            index = unit.SkillManager.GetSkillIndex(selected.skill);
        float extraOffset = index * xOffsetPerSkill;
        selected.MoveSkill(new Vector3(skillsUI.transform.position.x+ xOffsetFixed+extraOffset, skillsUI.transform.position.y, skillsUI.transform.position.z),()=> skillsUI.AddSkill(chooseSkill1.skill));
       
        MonoUtility.DelayFunction(()=>onSkillChosen?.Invoke(selected.skill), 1.5f);
    }

    void UpdateUI()
    {
        //Debug.Log("Unit has Skill: "+skill1.Name+" "+unit.SkillManager.HasSkill(skill1));
        chooseSkill1.SetSkill(skill1,unit.Blessing!=null,unit.SkillManager.HasSkill(skill1),unit.SkillManager.IsFull());
        
        //Debug.Log("Unit has Skill: "+skill2.Name+" "+unit.SkillManager.HasSkill(skill2));
        chooseSkill2.SetSkill(skill2,unit.Blessing!=null,unit.SkillManager.HasSkill(skill2),unit.SkillManager.IsFull());
        //Debug.Log("Unit has Skill: "+skill3.Name+" "+unit.SkillManager.HasSkill(skill3));
        chooseSkill3.SetSkill(skill3,unit.Blessing!=null,unit.SkillManager.HasSkill(skill3),unit.SkillManager.IsFull());
        if (skill1 is Blessing && skill2 is Blessing && skill3 is Blessing)
        {
            headline.SetText("Choose Blessing");
        }
        else
        {
            headline.SetText("Choose Skill");
        }
       

        skillsUI.Show(unit, unit.SkillManager.IsFull());
       
    }

    public void Hide()
    {
        if(unit!=null)
            unit.SkillManager.OnSkillsChanged -= UpdateUI;
        TweenUtility.FadeOut(canvasGroup).setOnComplete(()=>
        {
           
            OnFinished?.Invoke();
            chooseSkill1.Hide();
            chooseSkill2.Hide();
            chooseSkill3.Hide();
            canvasGroup.gameObject.SetActive(false);
        });
        
    }



    [SerializeField] private float xOffsetFixed = 0.05f;
    [SerializeField] private float xOffsetPerSkill = 0.05f;
    [SerializeField]int skillCount = 1;
    public void Skill1Clicked()
    {
        
        Debug.Log("Skill1Clicked");
       // Debug.Log("Move to: "+skillsUI.transform.position);
       
        if (unit != null)
            skillCount = unit.SkillManager.Skills.Count;
        float extraOffset = skillCount * xOffsetPerSkill;
        chooseSkill1.MoveSkill(new Vector3(skillsUI.transform.position.x+ xOffsetFixed+extraOffset, skillsUI.transform.position.y, skillsUI.transform.position.z),()=> skillsUI.AddSkill(chooseSkill1.skill));
       
        MonoUtility.DelayFunction(()=>onSkillChosen?.Invoke(chooseSkill1.skill), 1.5f);
        
    }

    public void Skill2Clicked()
    {
        Debug.Log("Skill2Clicked");
        if (unit != null)
            skillCount = unit.SkillManager.Skills.Count;
        float extraOffset = skillCount * xOffsetPerSkill;
        chooseSkill2.MoveSkill(new Vector3(skillsUI.transform.position.x+ xOffsetFixed+extraOffset, skillsUI.transform.position.y, skillsUI.transform.position.z),()=> skillsUI.AddSkill(chooseSkill2.skill));
        MonoUtility.DelayFunction(()=>onSkillChosen?.Invoke(chooseSkill2.skill), 1.5f);
        
    }

    public void Skill3Clicked()
    {
        Debug.Log("Skill3Clicked");
        if (unit != null)
            skillCount = unit.SkillManager.Skills.Count;
        float extraOffset = skillCount * xOffsetPerSkill;
        chooseSkill3.MoveSkill(new Vector3(skillsUI.transform.position.x+ xOffsetFixed+extraOffset, skillsUI.transform.position.y, skillsUI.transform.position.z),()=> skillsUI.AddSkill(chooseSkill3.skill));
        MonoUtility.DelayFunction(()=>onSkillChosen?.Invoke(chooseSkill3.skill), 1.5f);
    }
}