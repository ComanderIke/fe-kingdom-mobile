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
        [field:SerializeField] public Attributes FixedGrowthOffsets{ get; set; }
        public Attributes BonusAttributesFromEquips { get; set; }
        public Attributes BonusAttributesFromWeapon { get; set; }
        public Attributes BonusAttributesFromFood { get; set; }
        public Attributes BonusAttributesFromEffects { get; set; }
        public Attributes BonusAttributesFromBlessings { get; set; }
        public CombatStats BonusStatsFromTerrain { get; set; }
        public CombatStats BonusStatsFromEffects { get; set; }
        public CombatStats BonusStatsFromBlessings { get; set; }
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
            FixedGrowthOffsets = new Attributes();
            BonusAttributesFromEffects = new Attributes();
            BonusAttributesFromEquips = new Attributes();
            BonusAttributesFromWeapon = new Attributes();
            BonusAttributesFromFood = new Attributes();
            BonusAttributesFromBlessings = new Attributes();
            BonusStatsFromEffects = new CombatStats();
            BonusStatsFromEquips = new CombatStats();
            BonusStatsFromTerrain = new CombatStats();
            BonusStatsFromWeapon = new CombatStats();
            BonusStatsFromBlessings = new CombatStats();
            AttackRanges = new List<int>();
            Bane = AttributeType.NONE;
            Boon = AttributeType.NONE;
            BaseAttributes.OnAttributesUpdated += AttributesUpdate;
            BonusAttributesFromEffects.OnAttributesUpdated += AttributesUpdate;
            BonusAttributesFromEquips.OnAttributesUpdated += AttributesUpdate;
            BonusAttributesFromFood.OnAttributesUpdated += AttributesUpdate;
            BonusAttributesFromWeapon.OnAttributesUpdated += AttributesUpdate;
            BonusAttributesFromBlessings.OnAttributesUpdated += AttributesUpdate;
            
        }

        void AttributesUpdate()
        {
            onStatsUpdated?.Invoke();
        }


        public Attributes BonusGrowths { get; set; }
  


        public StatsData GetSaveData()
        {
            return new StatsData(Mov, BaseAttributes, BaseGrowths, AttackRanges);
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
            stats.BaseGrowths = new Attributes(BaseGrowths);
            stats.BaseAttributes = new Attributes(BaseAttributes);
            stats.BonusGrowths = new Attributes(BonusGrowths);
            stats.FixedGrowthOffsets = new Attributes(FixedGrowthOffsets);
            stats.BonusAttributesFromEffects = new Attributes(BonusAttributesFromEffects);
            stats.BonusAttributesFromWeapon = new Attributes(BonusAttributesFromWeapon);
            stats.BonusAttributesFromEquips = new Attributes(BonusAttributesFromEquips);
            stats.BonusAttributesFromFood = new Attributes(BonusAttributesFromFood);
            stats.BonusAttributesFromBlessings = new Attributes(BonusAttributesFromBlessings);
            stats.Mov = Mov;
            stats.BonusStatsFromEffects = new CombatStats(BonusStatsFromEffects);
            stats.BonusStatsFromTerrain = new CombatStats(BonusStatsFromTerrain);
            stats.BonusStatsFromEquips = new CombatStats(BonusStatsFromEquips);
            stats.BonusStatsFromWeapon = new CombatStats(BonusStatsFromWeapon);
            stats.BonusStatsFromBlessings = new CombatStats(BonusStatsFromBlessings);
            

            return stats;
        }

        public void LoadData(StatsData statsData)
        {

            //MaxSp = statsData.MaxSp;
            MyDebug.LogTODO("TODO Load Data which bonus stats to serialize?");
            Mov = statsData.Mov;
            AttackRanges = statsData.AttackRanges;
            BaseAttributes = new Attributes(statsData.Attributes);
            BaseGrowths = new Attributes(statsData.BaseGrowths);
        }
        public Attributes CombinedGrowths()
        {
            return BaseGrowths + BonusGrowths+ ((int)Player.Instance.Modifiers.GrowthIncrease);
        }
        public Attributes CombinedAttributes()
        {
            return BaseAttributes + BonusAttributesFromWeapon + BonusAttributesFromEffects +BonusAttributesFromBlessings+ BonusAttributesFromEquips+ BonusAttributesFromFood;
        }

        public CombatStats CombinedBonusStats()
        {
            return BonusStatsFromEquips + BonusStatsFromWeapon + BonusStatsFromEffects +BonusStatsFromBlessings+ BonusStatsFromTerrain;
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
                    baseAttributeValue = BaseAttributesAndWeapons().LCK;
                    combinedAttributeValue =
                        CombinedAttributes().LCK;
                    break;
                // case CombatStats.CombatStatType.Critavoid:
                //     baseAttributeValue = BaseAttributesAndWeapons().LCK * BattleStats.CRIT_AVO_LCK_MULT;
                //     combinedAttributeValue = CombinedAttributes().LCK * BattleStats.CRIT_AVO_LCK_MULT;
                //     break;
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
                case CombatStats.CombatStatType.CurseResistance:
                    baseAttributeValue = BaseAttributesAndWeapons().FAITH * BattleStats.CURSE_RES_FTH_MULT;
                    combinedAttributeValue = CombinedAttributes().FAITH * BattleStats.CURSE_RES_FTH_MULT;
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
        public AttributeBonusState GetGrowthBonusState(AttributeType attribute)
        {
            int baseAttributesAndWeapon = BaseGrowths.GetAttributeStat(attribute);
            return baseAttributesAndWeapon <
                   CombinedGrowths().GetAttributeStat(attribute)
                ? AttributeBonusState.Increasing
                : baseAttributesAndWeapon >
                  CombinedGrowths().GetAttributeStat(attribute)
                    ? AttributeBonusState.Decreasing
                    : AttributeBonusState.Same;
        }


        // public void IncreaseWeakest()
        // {
        //     List<AttributeType> weakest = new List<AttributeType>();
        //     int weakestValue = 199;
        //    
        //     weakestValue=CheckWeakestValue(weakestValue, weakest, AttributeType.STR);
        //     weakestValue=CheckWeakestValue(weakestValue, weakest, AttributeType.INT);
        //     weakestValue=CheckWeakestValue(weakestValue, weakest, AttributeType.DEX);
        //     weakestValue=CheckWeakestValue(weakestValue, weakest, AttributeType.DEF);
        //     weakestValue=CheckWeakestValue(weakestValue, weakest, AttributeType.FTH);
        //     weakestValue=CheckWeakestValue(weakestValue, weakest, AttributeType.LCK);
        //     weakestValue=CheckWeakestValue(weakestValue, weakest, AttributeType.CON);
        //     weakestValue=CheckWeakestValue(weakestValue, weakest, AttributeType.AGI);
        //         
        //     foreach(var weakestEntry in weakest)
        //         BaseAttributes.IncreaseAttribute(1, weakestEntry);
        // }
        //
        // int CheckWeakestValue(int weakestValue, List<AttributeType> weakest, AttributeType attribute)
        // {
        //     int attributeValue = BaseAttributes.GetFromIndex((int)attribute);
        //     if (attributeValue < weakestValue)
        //     {
        //         weakestValue = attributeValue;
        //         weakest.Clear();
        //         weakest.Add(attribute);
        //     }else if (attributeValue== weakestValue)
        //     {
        //         weakest.Add(attribute);
        //     }
        //
        //     return weakestValue;
        // }
        //
        // int CheckStrongestValue(int strongestValue, List<AttributeType> strongest, AttributeType attribute)
        // {
        //     int attributeValue = BaseAttributes.GetFromIndex((int)attribute);
        //     if (attributeValue > strongestValue)
        //     {
        //         strongestValue =attributeValue;
        //         strongest.Clear();
        //         strongest.Add(attribute);
        //     }else if (attributeValue== strongestValue)
        //     {
        //         strongest.Add(attribute);
        //     }
        //
        //     return strongestValue;
        // }
        //
        // public void IncreaseStrongest()
        // {
        //     List<AttributeType> strongest = new List<AttributeType>();
        //     int strongestValue = 199;
        //     strongestValue=CheckStrongestValue(strongestValue, strongest, AttributeType.STR);
        //     strongestValue=CheckStrongestValue(strongestValue, strongest, AttributeType.INT);
        //     strongestValue=CheckStrongestValue(strongestValue, strongest, AttributeType.DEX);
        //     strongestValue=CheckStrongestValue(strongestValue, strongest, AttributeType.DEF);
        //     strongestValue=CheckStrongestValue(strongestValue, strongest, AttributeType.FTH);
        //     strongestValue=CheckStrongestValue(strongestValue, strongest, AttributeType.LCK);
        //     strongestValue=CheckStrongestValue(strongestValue, strongest, AttributeType.CON);
        //     strongestValue=CheckStrongestValue(strongestValue, strongest, AttributeType.AGI);
        //     foreach(var strongestEntry in strongest)
        //         BaseAttributes.IncreaseAttribute(1, strongestEntry);
        // }
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

        public void OnValidate()
        {
            BaseAttributes.sum = BaseAttributes.GetSum(true);
            BaseGrowths.sum = BaseGrowths.GetSum();
        }

        public event Action onStatsUpdated;

      
    }
}