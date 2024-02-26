using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Players;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Flags", fileName = "MetaUpgrade1")]
public class FlagsMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<FlagType, bool> flags;

    public override void Activate(int level)
    {
        foreach(KeyValuePair<FlagType, bool> valuePair in flags)
        {
            switch (valuePair.Key)
            {
                case FlagType.BoonBane:
                    Player.Instance.Flags.BoonBaneUnlocked = valuePair.Value;break;
                case FlagType.WeakestAttributeIncrease:
                    Player.Instance.Flags.WeakestAttributeIncrease = valuePair.Value;break;
                case FlagType.EventPreviews:
                    Player.Instance.Flags.EventPreviewsUnlocked = valuePair.Value;break;
                case FlagType.MoralityVisible:
                    Player.Instance.Flags.MoralityVisible = valuePair.Value;break;
                case FlagType.StartingRelic:
                    Player.Instance.Flags.StartingRelic = valuePair.Value;break;
                case FlagType.StartingSkill:
                    Player.Instance.Flags.StartingSkill = valuePair.Value;break;
                case FlagType.StartingUpgrade:
                    Player.Instance.Flags.StartingUpgrade = valuePair.Value;break;
                case FlagType.RerollSkills:
                    Player.Instance.Flags.RerollSkills = valuePair.Value;break;
                case FlagType.SpecialUpgrade:
                    Player.Instance.Flags.SpecialUpgradeUnlocked = valuePair.Value;break;
                case FlagType.GluttonyForceEat:
                    Player.Instance.Flags.GluttonyForceEat = valuePair.Value;break;
                case FlagType.RerollLevelUps:
                    Player.Instance.Flags.RerollLevelUps = valuePair.Value;break;
                case FlagType.RevivalStoneStart:
                    Player.Instance.Flags.RevivalStoneStart = valuePair.Value;break;
                case FlagType.StrongestAttributeIncrease:
                    Player.Instance.Flags.StrongestAttributeIncrease = valuePair.Value;break;
            }
        }
    }
    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        foreach (var entry in flags)
        {
            list.Add(new EffectDescription(""+TextUtility.EnumToString(entry.Key)+":", entry.Value?"Unlocked": "Deactivated", ""+entry.Value));
        }
       
        return list;
    }
}

public enum FlagType
{
    BoonBane,
    StrongestAttributeIncrease,
    WeakestAttributeIncrease,
    StartingSkill,
    StartingRelic,
    StartingUpgrade,
    SpecialUpgrade,
    EventPreviews,
    RerollLevelUps,
    RerollSkills,
    RevivalStoneStart,
    MoralityVisible,
    GluttonyForceEat,
}