using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Miscellaneous", fileName = "MetaUpgrade1")]
public class MiscellaneousMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<MiscellaneousType, int>[] intValues;

    public override void Activate(int level)
    {
        foreach (KeyValuePair<MiscellaneousType, int> valuePair in intValues[level])
        {
            switch (valuePair.Key)
            {
                case MiscellaneousType.DayBonuses: break;
                case MiscellaneousType.NightMaluses: break;
                case MiscellaneousType.BattleGoldReward:
                    Player.Instance.Modifiers.BattleGoldReward = valuePair.Value;break;
                case MiscellaneousType.CriticalHitIncrease: break;
                case MiscellaneousType.DamageDoneIncrease: break;
                case MiscellaneousType.DamageReceivedIncrease: break;
                case MiscellaneousType.GemstoneMaxSouls: break;
                case MiscellaneousType.MaxStats: break;
                case MiscellaneousType.GemStoneMergeAmount:
                    Smithy.GemStoneMergeAmount = valuePair.Value; break;
                case MiscellaneousType.GoldBonusPerTurn:
                    Player.Instance.Modifiers.GoldBonusPerTurn = valuePair.Value;break;
            }
        }
    }

    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        foreach (var entry in intValues[level])
        {
            list.Add(new EffectDescription(""+TextUtility.EnumToString(entry.Key)+":", (entry.Value>0?"+":"")+entry.Value, (entry.Value>0?"+":"")+entry.Value));
        }
       
        return list;
    }
}

public enum MiscellaneousType
{
    GoldBonusPerTurn,
    MaxStats,
    NightMaluses,
    DayBonuses,
    GemstoneMaxSouls,
    GemStoneMergeAmount,
    CriticalHitIncrease,
    DamageDoneIncrease,
    DamageReceivedIncrease,
    BattleGoldReward,
}