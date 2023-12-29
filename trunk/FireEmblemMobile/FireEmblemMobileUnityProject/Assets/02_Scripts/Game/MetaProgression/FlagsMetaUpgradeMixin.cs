using _02_Scripts.Game.GUI.Utility;
using PlasticGui.WorkspaceWindow.QueryViews.Attributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Flags", fileName = "MetaUpgrade1")]
public class FlagsMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<FlagType, bool> flags;
}

public enum FlagType
{
    BoonBane,
    StrongestAttributeIncrease,
    WeakestAttributeIncrease,
    StartingSkill,
    StartingRelic,
    SpecialUpgrade,
    EventPreviews,
    RerollLevelUps,
    RerollSkills,
    RevivalStoneStart,
    MoralityVisible,
    GluttonyForceEat,
}