using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Numbers
{
    [Serializable]
    //[CreateAssetMenu(menuName = "GameData/Unit/Stats", fileName = "UnitStats")]
    public class Stats : ICloneable
    {
        [HideInInspector] [SerializeField] public List<int> AttackRanges;
        [SerializeField] public Attributes BaseAttributes;
        public Attributes BonusAttributesFromEquips { get; set; }
        public Attributes BonusAttributesFromWeapon { get; set; }
        public Attributes BonusAttributesFromEffects { get; set; }
        public BonusStats BonusStatsFromTerrain { get; set; }
        public BonusStats BonusStatsFromEffects { get; set; }
        public BonusStats BonusStatsFromEquips { get; set; }

        public BonusStats BonusStatsFromWeapon { get; set; }

        // [SerializeField]
        // public int MaxSp;
        [SerializeField] public int Mov;

        public Stats()
        {
            BaseAttributes = new Attributes();
            BonusAttributesFromEffects = new Attributes();
            BonusAttributesFromEquips = new Attributes();
            BonusAttributesFromWeapon = new Attributes();
            BonusStatsFromEffects = new BonusStats();
            BonusStatsFromEquips = new BonusStats();
            BonusStatsFromTerrain = new BonusStats();
            BonusStatsFromWeapon = new BonusStats();
            AttackRanges = new List<int>();
        }


        public Attributes BonusGrowths { get; set; }


        public StatsData GetSaveData()
        {
            return new StatsData(Mov, BaseAttributes, AttackRanges);
        }

        public int GetMaxAttackRange()
        {
            return AttackRanges.Max();
        }

        public object Clone()
        {
            Stats stats = new Stats();
            stats.AttackRanges = new List<int>();
            foreach (int i in AttackRanges)
            {
                stats.AttackRanges.Add(i);
            }

            // stats.MaxSp = MaxSp;
            //  Debug.Log("Clone2Attriubtes");
            stats.BaseAttributes = new Attributes(BaseAttributes);
            stats.BonusAttributesFromEffects = new Attributes(BonusAttributesFromEffects);
            stats.BonusAttributesFromWeapon = new Attributes(BonusAttributesFromWeapon);
            stats.BonusAttributesFromEquips = new Attributes(BonusAttributesFromEquips);
            stats.Mov = Mov;
            stats.BonusStatsFromEffects = new BonusStats(BonusStatsFromEffects);
            stats.BonusStatsFromTerrain = new BonusStats(BonusStatsFromTerrain);
            stats.BonusStatsFromEquips = new BonusStats(BonusStatsFromEquips);
            stats.BonusStatsFromWeapon = new BonusStats(BonusStatsFromWeapon);

            return stats;
        }

        public void LoadData(StatsData statsData)
        {

            //MaxSp = statsData.MaxSp;
            Mov = statsData.Mov;
            AttackRanges = statsData.AttackRanges;
            BaseAttributes = new Attributes(statsData.Attributes);
        }

        public Attributes CombinedAttributes()
        {
            return BaseAttributes + BonusAttributesFromWeapon + BonusAttributesFromEffects + BonusAttributesFromEquips;
        }

        public BonusStats CombinedBonusStats()
        {
            return BonusStatsFromEquips + BonusStatsFromWeapon + BonusStatsFromEffects + BonusStatsFromTerrain;
        }

        public Attributes BaseAttributesAndWeapons()
        {
            return BaseAttributes + BonusAttributesFromWeapon;
        }

        public BonusStats GetBonusStatsWithoutWeapon()
        {
            return BonusStatsFromEquips + BonusStatsFromTerrain + BonusStatsFromEffects;
        }


        public Vector2Int GetAttributeValueOfCombatStat(BonusStats.CombatStatType statType, bool physical = true)
        {
            int baseAttributeValue = 0;
            int combinedAttributeValue = 0;
            switch (statType)
            {
                case BonusStats.CombatStatType.Attack:
                    baseAttributeValue =
                        physical ? BaseAttributesAndWeapons().STR : BaseAttributesAndWeapons().INT;
                    combinedAttributeValue = physical ? CombinedAttributes().STR : CombinedAttributes().INT;
                    break;
                case BonusStats.CombatStatType.Avoid:
                    baseAttributeValue = BaseAttributesAndWeapons().AGI * BattleStats.AVO_AGI_MULT;
                    combinedAttributeValue = CombinedAttributes().AGI * BattleStats.AVO_AGI_MULT;
                    break;
                case BonusStats.CombatStatType.Crit:
                    baseAttributeValue = BaseAttributesAndWeapons().DEX + BaseAttributesAndWeapons().LCK;
                    combinedAttributeValue =
                        CombinedAttributes().DEX + CombinedAttributes().LCK;
                    break;
                case BonusStats.CombatStatType.Critavoid:
                    baseAttributeValue = BaseAttributesAndWeapons().LCK * BattleStats.CRIT_AVO_LCK_MULT;
                    combinedAttributeValue = CombinedAttributes().LCK * BattleStats.CRIT_AVO_LCK_MULT;
                    break;
                case BonusStats.CombatStatType.Hit:
                    baseAttributeValue = BaseAttributesAndWeapons().DEX * BattleStats.HIT_DEX_MULT;
                    combinedAttributeValue = CombinedAttributes().DEX * BattleStats.HIT_DEX_MULT;
                    break;
                case BonusStats.CombatStatType.MagicResistance:
                    baseAttributeValue = BaseAttributesAndWeapons().FAITH;
                    combinedAttributeValue = CombinedAttributes().FAITH;
                    break;
                case BonusStats.CombatStatType.PhysicalResistance:
                    baseAttributeValue = BaseAttributesAndWeapons().DEF;
                    combinedAttributeValue = CombinedAttributes().DEF;
                    break;
                case BonusStats.CombatStatType.AttackSpeed:
                    baseAttributeValue = BaseAttributesAndWeapons().AGI;
                    combinedAttributeValue = CombinedAttributes().AGI;
                    break;
            }

            return new Vector2Int(baseAttributeValue, combinedAttributeValue);

        }

        public int GetCombatStatBonuses(Unit unit, BonusStats.CombatStatType statType, bool physical = true)
        {
            var baseAndCombinedAttributeStat = GetAttributeValueOfCombatStat(statType, physical);
            int onlyBonuses = unit.BattleComponent.BattleStats.GetStatOnlyBonusesWithoutWeaponFromEnum(statType);
            int sumBonuses = (baseAndCombinedAttributeStat.y - baseAndCombinedAttributeStat.x) + onlyBonuses;
            return sumBonuses;
        }

        public AttributeBonusState GetAttributeBonusState(AttributeType attribute)
        {
            int baseAttributesAndWeapon = BaseAttributes.GetAttributeStat(attribute) +
                                          BonusAttributesFromWeapon.GetAttributeStat(attribute);
            return baseAttributesAndWeapon <
                   CombinedAttributes().GetAttributeStat(attribute)
                ? AttributeBonusState.Increasing
                : baseAttributesAndWeapon >
                  CombinedAttributes().GetAttributeStat(attribute)
                    ? AttributeBonusState.Decreasing
                    : AttributeBonusState.Same;
        }
    }
}