using System.Collections.Generic;
using Game.GameActors.Player;
using Game.GameActors.Units.Skills.Base;
using Game.GUI.Utility;
using UnityEngine;
using TextUtility = Game.Dialog.DialogSystem.TextUtility;

namespace Game.MetaProgression
{
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
                    case FlagType.SmithingBonds:
                        Player.Instance.Flags.SmithingBonds = valuePair.Value;break;
                    case FlagType.EventBonds:
                        Player.Instance.Flags.EventBonds = valuePair.Value;break;
                    case FlagType.InnBonds:
                        Player.Instance.Flags.InnBonds = valuePair.Value;break;
                    case FlagType.InnSupplies:
                        Player.Instance.Flags.InnSupplies = valuePair.Value;break;
                    case FlagType.CombatSkills:
                        Player.Instance.Flags.CombatSkills = valuePair.Value;break;
                    case FlagType.LiquorBonds:
                        Player.Instance.Flags.LiquorBonds = valuePair.Value;break;
                    case FlagType.KillBonds:
                        Player.Instance.Flags.KillBonds = valuePair.Value;break;
                    case FlagType.BossKillBonds:
                        Player.Instance.Flags.BossKillBonds = valuePair.Value;break;
                    case FlagType.Secret:
                        Player.Instance.Flags.Secret = valuePair.Value;break;
                    case FlagType.LethalCritical:
                        Player.Instance.Flags.LethalCritical = valuePair.Value;break;
                
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
        SmithingBonds,
        EventBonds,
        InnBonds,
        InnSupplies,
        CombatSkills,
        LiquorBonds,
        KillBonds,
        BossKillBonds,
        Secret,
        LethalCritical
    }
}