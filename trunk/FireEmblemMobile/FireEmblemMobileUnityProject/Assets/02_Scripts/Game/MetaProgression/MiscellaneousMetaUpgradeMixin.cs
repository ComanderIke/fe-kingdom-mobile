using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Units.Numbers;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Miscellaneous", fileName = "MetaUpgrade1")]
public class MiscellaneousMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<MiscellaneousType, int> intValues;
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