using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

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