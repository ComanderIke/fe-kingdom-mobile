using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using GameEngine;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SkillRarity
{
    Common, Rare, Epic, Legendary, Mythic
}

[CreateAssetMenu(menuName = "GameData/SkillGenerationConfiguration", fileName = "SkillGenerationConfiguration")]
public class SkillGenerationConfiguration : ScriptableObject
{
    [SerializeField] private float rareChance;
    [SerializeField] private float epicChance;
    [SerializeField] private float legendaryChance;
    [SerializeField] private float mythicChance;
    [SerializeField] private List<SkillBP> commonSkillPool;
    [SerializeField] private List<SkillBP> rangerSkillPool;
    [SerializeField] private List<SkillBP> cavalierSkillPool;
    [SerializeField] private List<SkillBP> clericSkillPool;
    [SerializeField] private List<SkillBP> scorpionRiderSkillPool;
    [SerializeField] private List<SkillBP> mercSkillPool;
    [SerializeField] private List<SkillBP> witchSkillPool;
    
    public float RareChance => rareChance;
    public float EpicChance => epicChance;

    public float LegendaryChance => legendaryChance;

    public float MythicChance => mythicChance;
    
    public List<SkillBP> CommonSkillPool => commonSkillPool;

    public List<SkillBP> RangerSkillPool => rangerSkillPool;

    public List<SkillBP> CavalierSkillPool => cavalierSkillPool;

    public List<SkillBP> ClericSkillPool => clericSkillPool;

    public List<SkillBP> ScorpionRiderSkillPool => scorpionRiderSkillPool;

    public List<SkillBP> MercSkillPool => mercSkillPool;
    
    public List<SkillBP> WitchSkillPool => witchSkillPool;
    private Dictionary<RpgClass, List<SkillBP>> classPools;

    private void SetUpClassPools()
    {
        classPools = new Dictionary<RpgClass, List<SkillBP>>();
        classPools.Add(RpgClass.Cavalier, cavalierSkillPool);
        classPools.Add(RpgClass.Mercenary, mercSkillPool);
        classPools.Add(RpgClass.Ranger, rangerSkillPool);
        classPools.Add(RpgClass.Witch, witchSkillPool);
        classPools.Add(RpgClass.ScorpionRider, scorpionRiderSkillPool);
        classPools.Add(RpgClass.Cleric, clericSkillPool);
    }

    public IEnumerable<SkillBP> GetClassSkillPool(RpgClass unitRpgClass)
    {
        if (classPools == null)
        {
            SetUpClassPools();
        }
        return classPools[unitRpgClass];
    }
}
public class SkillSystem : IEngineSystem
{
    private SkillGenerationConfiguration config;

    public SkillSystem(SkillGenerationConfiguration config)
    {
        this.config = config;
    }
    public void Init()
    {
        
    }

    public List<Skill> GenerateSkills(Unit unit)
    {
        List<Skill> skills = new List<Skill>();
        while (skills.Count != 3)
        {
            var skill = GenerateSkill(unit);
            if (!skills.Contains(skill))
            {
                skills.Add(skill);
            }
        }

        return skills;
    }
    public Skill GenerateSkill(Unit unit)
    {
        var skillPool = config.CommonSkillPool;
        skillPool.AddRange(config.GetClassSkillPool(unit.rpgClass));
        int rng = Random.Range(0, skillPool.Count);
        var skill = skillPool[rng].Create();
        GenerateSkillRarity(skill);
        return skill;
    }
    public void GenerateSkillRarity(Skill skill)
    {
        float rng = Random.value;
        float epicChance = config.EpicChance;
        if (rng <= config.MythicChance)
        {
            skill.Level = 5;
        }
        else if (rng <= config.LegendaryChance)
        {
            skill.Level = 4;
        }
        else if (rng <= config.EpicChance)
        {
            skill.Level = 3;
        }
        else if (rng <= config.RareChance)
        {
            skill.Level = 2;
        }
        else
        {
            skill.Level = 1;
        }
    }

    public void Deactivate()
    {
        
    }

    public void Activate()
    {
        
    }
}