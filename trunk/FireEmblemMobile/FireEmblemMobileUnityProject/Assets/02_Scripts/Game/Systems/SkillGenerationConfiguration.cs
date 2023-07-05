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
    [SerializeField] private List<SkillBp> commonSkillPool;
    [SerializeField] private List<SkillBp> rangerSkillPool;
    [SerializeField] private List<SkillBp> cavalierSkillPool;
    [SerializeField] private List<SkillBp> clericSkillPool;
    [SerializeField] private List<SkillBp> scorpionRiderSkillPool;
    [SerializeField] private List<SkillBp> mercSkillPool;
    [SerializeField] private List<SkillBp> witchSkillPool;
    
    public float RareChance => rareChance;
    public float EpicChance => epicChance;

    public float LegendaryChance => legendaryChance;

    public float MythicChance => mythicChance;
    
    public List<SkillBp> CommonSkillPool => commonSkillPool;

    public List<SkillBp> RangerSkillPool => rangerSkillPool;

    public List<SkillBp> CavalierSkillPool => cavalierSkillPool;

    public List<SkillBp> ClericSkillPool => clericSkillPool;

    public List<SkillBp> ScorpionRiderSkillPool => scorpionRiderSkillPool;

    public List<SkillBp> MercSkillPool => mercSkillPool;
    
    public List<SkillBp> WitchSkillPool => witchSkillPool;
    private Dictionary<RpgClass, List<SkillBp>> classPools;

    private void SetUpClassPools()
    {
        classPools = new Dictionary<RpgClass, List<SkillBp>>();
        classPools.Add(RpgClass.Cavalier, cavalierSkillPool);
        classPools.Add(RpgClass.Mercenary, mercSkillPool);
        classPools.Add(RpgClass.Ranger, rangerSkillPool);
        classPools.Add(RpgClass.Witch, witchSkillPool);
        classPools.Add(RpgClass.ScorpionRider, scorpionRiderSkillPool);
        classPools.Add(RpgClass.Cleric, clericSkillPool);
       
    }

    public IEnumerable<SkillBp> GetClassSkillPool(RpgClass unitRpgClass)
    {
        if (classPools == null)
        {
            SetUpClassPools();
        }
        return classPools[unitRpgClass];
    }
}