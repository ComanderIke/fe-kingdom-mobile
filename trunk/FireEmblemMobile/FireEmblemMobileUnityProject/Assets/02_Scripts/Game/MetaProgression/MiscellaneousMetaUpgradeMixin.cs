using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
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
                case MiscellaneousType.BattleGoldReward: break;
                case MiscellaneousType.CriticalHitIncrease: break;
                case MiscellaneousType.DamageDoneIncrease: break;
                case MiscellaneousType.DamageReceivedIncrease: break;
                case MiscellaneousType.GemstoneMaxSouls: break;
                case MiscellaneousType.MaxStatCaps1: break;
                case MiscellaneousType.MaxStatCaps2: break;
                case MiscellaneousType.GemStoneMergeAmount: break;
                case MiscellaneousType.GoldBonusPerTurnCount: break;
            }
        }
    }

    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        foreach (var entry in intValues[level])
        {
            list.Add(new EffectDescription(""+entry.Key, ""+entry.Value, ""+entry.Value));
        }
       
        return list;
    }
}

public enum MiscellaneousType
{
    GoldBonusPerTurnCount,
    MaxStatCaps1,
    MaxStatCaps2,
    NightMaluses,
    DayBonuses,
    GemstoneMaxSouls,
    GemStoneMergeAmount,
    CriticalHitIncrease,
    DamageDoneIncrease,
    DamageReceivedIncrease,
    BattleGoldReward,
}