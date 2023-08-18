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
        [field:SerializeField] public Attributes BaseGrowths{ get; set; }
        public Attributes BonusAttributesFromEquips { get; set; }
        public Attributes BonusAttributesFromWeapon { get; set; }
        public Attributes BonusAttributesFromEffects { get; set; }
        public CombatStats BonusStatsFromTerrain { get; set; }
        public CombatStats BonusStatsFromEffects { get; set; }
        public CombatStats BonusStatsFromEquips { get; set; }

        public CombatStats BonusStatsFromWeapon { get; set; }

        // [SerializeField]
        // public int MaxSp;
        [SerializeField] public int Mov;
        public AttributeType Boon { get; private set; }
        public AttributeType Bane{ get; private set; }
        public Stats()
        {
            BaseAttributes = new Attributes();
            BaseGrowths = new Attributes();
            BonusGrowths = new Attributes();
            
            BonusAttributesFromEffects = new Attributes();
            BonusAttributesFromEquips = new Attributes();
            BonusAttributesFromWeapon = new Attributes();
            BonusStatsFromEffects = new CombatStats();
            BonusStatsFromEquips = new CombatStats();
            BonusStatsFromTerrain = new CombatStats();
            BonusStatsFromWeapon = new CombatStats();
            AttackRanges = new List<int>();
            Bane = AttributeType.NONE;
            Boon = AttributeType.NONE;
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
            stats.BonusGrowths = new Attributes(BonusGrowths);
            stats.BonusAttributesFromEffects = new Attributes(BonusAttributesFromEffects);
            stats.BonusAttributesFromWeapon = new Attributes(BonusAttributesFromWeapon);
            stats.BonusAttributesFromEquips = new Attributes(BonusAttributesFromEquips);
            stats.Mov = Mov;
            stats.BonusStatsFromEffects = new CombatStats(BonusStatsFromEffects);
            stats.BonusStatsFromTerrain = new CombatStats(BonusStatsFromTerrain);
            stats.BonusStatsFromEquips = new CombatStats(BonusStatsFromEquips);
            stats.BonusStatsFromWeapon = new CombatStats(BonusStatsFromWeapon);

            return stats;
        }

        public void LoadData(StatsData statsData)
        {

            //MaxSp = statsData.MaxSp;
            Debug.Log("TODO Load Data which bonus stats to serialize?");
            Mov = statsData.Mov;
            AttackRanges = statsData.AttackRanges;
            BaseAttributes = new Attributes(statsData.Attributes);
        }
        public Attributes CombinedGrowths()
        {
            return BaseGrowths + BonusGrowths;
        }
        public Attributes CombinedAttributes()
        {
            return BaseAttributes + BonusAttributesFromWeapon + BonusAttributesFromEffects + BonusAttributesFromEquips;
        }

        public CombatStats CombinedBonusStats()
        {
            return BonusStatsFromEquips + BonusStatsFromWeapon + BonusStatsFromEffects + BonusStatsFromTerrain;
        }

        public Attributes BaseAttributesAndWeapons()
        {
            return BaseAttributes + BonusAttributesFromWeapon;
        }

        public CombatStats GetBonusStatsWithoutWeapon()
        {
            return BonusStatsFromEquips + BonusStatsFromTerrain + BonusStatsFromEffects;
        }


        public Vector2Int GetAttributeValueOfCombatStat(CombatStats.CombatStatType statType, bool physical = true)
        {
            int baseAttributeValue = 0;
            int combinedAttributeValue = 0;
            switch (statType)
            {
                case CombatStats.CombatStatType.Attack:
                    baseAttributeValue =
                        physical ? BaseAttributesAndWeapons().STR : BaseAttributesAndWeapons().INT;
                    combinedAttributeValue = physical ? CombinedAttributes().STR : CombinedAttributes().INT;
                    break;
                case CombatStats.CombatStatType.Avoid:
                    baseAttributeValue = BaseAttributesAndWeapons().AGI * BattleStats.AVO_AGI_MULT;
                    combinedAttributeValue = CombinedAttributes().AGI * BattleStats.AVO_AGI_MULT;
                    break;
                case CombatStats.CombatStatType.Crit:
                    baseAttributeValue = BaseAttributesAndWeapons().DEX + BaseAttributesAndWeapons().LCK;
                    combinedAttributeValue =
                        CombinedAttributes().DEX + CombinedAttributes().LCK;
                    break;
                case CombatStats.CombatStatType.Critavoid:
                    baseAttributeValue = BaseAttributesAndWeapons().LCK * BattleStats.CRIT_AVO_LCK_MULT;
                    combinedAttributeValue = CombinedAttributes().LCK * BattleStats.CRIT_AVO_LCK_MULT;
                    break;
                case CombatStats.CombatStatType.Hit:
                    baseAttributeValue = BaseAttributesAndWeapons().DEX * BattleStats.HIT_DEX_MULT;
                    combinedAttributeValue = CombinedAttributes().DEX * BattleStats.HIT_DEX_MULT;
                    break;
                case CombatStats.CombatStatType.Resistance:
                    baseAttributeValue = BaseAttributesAndWeapons().FAITH;
                    combinedAttributeValue = CombinedAttributes().FAITH;
                    break;
                case CombatStats.CombatStatType.Protection:
                    baseAttributeValue = BaseAttributesAndWeapons().DEF;
                    combinedAttributeValue = CombinedAttributes().DEF;
                    break;
                case CombatStats.CombatStatType.AttackSpeed:
                    baseAttributeValue = BaseAttributesAndWeapons().AGI;
                    combinedAttributeValue = CombinedAttributes().AGI;
                    break;
            }

            return new Vector2Int(baseAttributeValue, combinedAttributeValue);

        }

        public int GetCombatStatBonuses(Unit unit, CombatStats.CombatStatType statType, bool physical = true)
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

     
        public void SetBane(AttributeType bane)
        {
            if (this.Bane != AttributeType.NONE)
            {
                BaseAttributes.IncreaseAttribute(1, this.Bane);
                BaseGrowths.IncreaseAttribute(10, this.Bane);
            }

            this.Bane = bane;
            BaseAttributes.IncreaseAttribute(-1, this.Bane);
            BaseGrowths.IncreaseAttribute(-10, this.Bane);
        }

        public void SetBoon(AttributeType boon)
        {
            if (this.Boon != AttributeType.NONE)
            {
                BaseAttributes.IncreaseAttribute(-1, this.Boon);
                BaseGrowths.IncreaseAttribute(-10, this.Boon);
            }

            this.Boon = boon;
            BaseAttributes.IncreaseAttribute(1, this.Boon);
            BaseGrowths.IncreaseAttribute(10, this.Boon);
        }
    }
}