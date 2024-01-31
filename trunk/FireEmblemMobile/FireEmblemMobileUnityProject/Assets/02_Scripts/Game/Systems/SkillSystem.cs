using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using GameEngine;
using LostGrace;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public enum SkillRarity
{
    Common, Rare, Epic, Legendary, Mythic
}

public class SkillSystem : IEngineSystem
{
    private SkillGenerationConfiguration config;
    private ISkillUIRenderer renderer;

    public SkillSystem(SkillGenerationConfiguration config, ISkillUIRenderer renderer)
    {
        this.config = config;
        this.renderer = renderer;
    }
    public void Init()
    {
        

    }

    public void LearnNewSkill(Unit unit, List<SkillBp> skillPool)
    {
        MyDebug.LogLogic("Choose new SKill: "+unit);
        
       
        
       
        AnimationQueue.Add(()=>
        {
            MyDebug.LogTest(("Learn new Skill: "+unit.Name));
            var skills = GenerateSkills(unit, skillPool);
            renderer.OnFinished -= FinishedAnimation;
            renderer.OnFinished += FinishedAnimation;
            skillClickedDelegate=(skill)=>
            {
                renderer.Hide();
                LearnSkill(unit, skill);
                MyDebug.LogTest("Skill Clicked Delegate" + unit.Name);
                renderer.onSkillChosen -= skillClickedDelegate;
            };
            renderer.onSkillChosen += skillClickedDelegate;
            
            renderer.Show(unit, skills[0], skills[1], skills[2]);
        });
    }

   
    Action<Skill> skillClickedDelegate = null;
    public void LearnNewSkill(Unit unit, Skill skill1, Skill skill2, Skill skill3)
    {
        Debug.LogError("Should not be called?");
        // renderer.OnFinished -= FinishedAnimation;
        // renderer.OnFinished += FinishedAnimation;
        // Debug.Log("LEARN NEW SKILL");
        // skillClickedDelegate = ( skill) =>
        // {
        //     Debug.Log("LEARN SKILL DELEGATE!");
        //     renderer.Hide();
        //     LearnSkill(unit, skill);
        //     renderer.onSkillChosen -= skillClickedDelegate;
        // };
        // renderer.onSkillChosen += skillClickedDelegate;
        // AnimationQueue.Add(()=>renderer.Show(unit, skill1, skill2, skill3));
    }

    void LearnSkill(Unit unit, Skill skill)
    {
        if(!unit.SkillManager.IsFull())
            unit.SkillManager.LearnSkill(skill);
        else
        {
            Debug.Log("Cant learn Skill! Skilllist is full!");
           // renderer.ShowReplaceSkillUI(skill, unit);
        }
    }

    void FinishedAnimation()
    {
        renderer.OnFinished -= FinishedAnimation;
       // renderer.onSkillChosen -= skillClickedDelegate;
        MyDebug.LogTest("Choose Skill Animation Finished");
        AnimationQueue.OnAnimationEnded?.Invoke();
    }

    private float chanceIndividualSkillUpgrade = .1f;
    private int maxUpgrades = 2;
    private List<Skill> GenerateSkills(Unit unit, List<SkillBp> skillPool, bool includeUpgrades=true)
    {
        //Debug.Log("GENERATE SKILLS");
        List<Skill> skills = new List<Skill>();
        int upgradeCount = 0;
        int Rounds = 30;
        while (skills.Count != 3&& Rounds>0)
        {
            Skill skill = null;
            Rounds--;
            bool upgrade = false;
            if (upgradeCount < maxUpgrades&& includeUpgrades)
            {
                foreach (var upgSkill in unit.SkillManager.Skills)
                {
                    if(upgSkill is Blessing)//Dont allow Blessing Upgrades outside of church
                        continue;
                    if (upgSkill.Upgradable()&&Random.value < chanceIndividualSkillUpgrade&&!skills.Contains(upgSkill))
                    {
                        skill = upgSkill.Clone();
                        //skill.Level++;
                        upgradeCount++;
                        upgrade = true;
                        //Debug.Log("Upgrade: "+skill.Name);
                        break;
                    }
                }
            }

            if (skill == null)
            {
                skill = GenerateSkill(unit, skillPool);
              //  Debug.Log("NewSkill: "+skill.Name);
            }

            if (!skills.Contains(skill)&&(!unit.SkillManager.HasSkill(skill)||upgrade))
            {
               // Debug.Log("Adding Skill: "+skill.Name);
                skills.Add(skill);
            }
        }

        return skills;
    }
    private Skill GenerateSkill(Unit unit, List<SkillBp> skPool)
    {
        var skillPool = skPool==null?new List<SkillBp>(config.CommonSkillPool):skPool;
        skillPool.AddRange(config.GetClassSkillPool(unit.rpgClass));
        //Debug.Log("SKILLPOOL SIZE: "+skillPool.Count);
        int rng = Random.Range(0, skillPool.Count);
       // Debug.Log("RNG: "+rng);
        var skill = skillPool[rng].Create();
        GenerateSkillRarity(skill);
        return skill;
    }
    private void GenerateSkillRarity(Skill skill)
    {
        float rng = Random.value;
        float epicChance = config.EpicChance;
        
        if (rng <= config.MythicChance)
        {
            skill.Tier = 0;
            skill.Level = 4;
        }
        else if (rng <= config.LegendaryChance)
        {
            skill.Tier = 1;
            skill.Level = 3;
        }
        else if (rng <= config.EpicChance)
        {
            skill.Tier = 2;
            skill.Level = 2;
        }
        else if (rng <= config.RareChance)
        {
            skill.Tier = 3;
            skill.Level = 1;
        }
        else
        {
            skill.Tier = 4;
            skill.Level = 0;
        }
    }

    public void Deactivate()
    {
        
    }

    public void Activate()
    {
        
    }

    public void RemoveSkill(Unit u, Skill skill)
    {
        u.SkillManager.RemoveSkill(skill);
    }
}