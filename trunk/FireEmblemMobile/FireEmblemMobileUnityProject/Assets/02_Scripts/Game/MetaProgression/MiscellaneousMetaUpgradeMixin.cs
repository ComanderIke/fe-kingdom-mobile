using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
using Game.GameResources;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Miscellaneous", fileName = "MetaUpgrade1")]
public class MiscellaneousMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<MiscellaneousType, int>[] intValues;

    private float levelToAccuracyModifier = 1.5f;
    public override void Activate(int level)
    {
        foreach (KeyValuePair<MiscellaneousType, int> valuePair in intValues[level])
        {
            switch (valuePair.Key)
            {
                case MiscellaneousType.DayBonuses:
                    ActivateDayBonuses(valuePair.Value);
                   
                    break;
                case MiscellaneousType.NightMaluses: 
                    ActivateNightBonuses(valuePair.Value);
                    break;
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
                case MiscellaneousType.ExtraHealing:
                    Player.Instance.Modifiers.BonusHeal = valuePair.Value;break;
                case MiscellaneousType.SupportBonus:
                    Player.Instance.Modifiers.SupportBonus = valuePair.Value;break;
                case MiscellaneousType.FireDamage:
                    Player.Instance.Modifiers.FireDamage = valuePair.Value;break;
                case MiscellaneousType.CombatSkillCost:
                    Player.Instance.Modifiers.CombatSkillCost = valuePair.Value;break;
            }
        }
    }

    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        foreach (var entry in intValues[level])
        {
            if (entry.Key == MiscellaneousType.DayBonuses)
            {
                list.Add(new EffectDescription("Heal per Node: ", entry.Value+" Hp", entry.Value+"Hp"));
            }
            else if (entry.Key == MiscellaneousType.NightMaluses)
            {

                list.Add(new EffectDescription("Hit Rate: ", (GameBPData.Instance.NightBonuses.accuracy+(int)(entry.Value*levelToAccuracyModifier))+"%", (GameBPData.Instance.NightBonuses.accuracy+(int)(entry.Value*levelToAccuracyModifier))+"%"));
                list.Add(new EffectDescription("Critical Rate: ", "+"+entry.Value+"%", "+"+entry.Value+"%"));
            }
            else
            {
                list.Add(new EffectDescription("" + TextUtility.EnumToString(entry.Key) + ":",
                    (entry.Value > 0 ? "+" : "") + entry.Value, (entry.Value > 0 ? "+" : "") + entry.Value));
            }
        }

        return list;
    }

    void ActivateDayBonuses(int level)
    {
        Player.Instance.Modifiers.DayBonuses.healPerNode = level;
    }
    void ActivateNightBonuses(int level)
    {
        Player.Instance.Modifiers.NightBonuses.accuracy = (int)(level*levelToAccuracyModifier);
        Player.Instance.Modifiers.NightBonuses.allyCritical = level;
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
    ExtraHealing,
    SupportBonus,
    FireDamage,
    CombatSkillCost,
}