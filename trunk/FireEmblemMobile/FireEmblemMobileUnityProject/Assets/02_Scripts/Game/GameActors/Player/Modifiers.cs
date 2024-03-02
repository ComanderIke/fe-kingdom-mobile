using Game.GameResources;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Players
{
    public class Modifiers
    {
        public Modifiers()
        {
            GrowthIncrease = 0;
            ExperienceGain = 1.0f;
            GoldGain = 1.0f;
            GraceGain = 1.0f;
            HealingRate = 1.0f;
            AssistExpRate = 1.0f;
            BondExpGain = 1.0f;
            RelicDropRate = 1.0f;
            EliteBattleRate = 1.0f;
            FoodHealRate = 1.0f;
            GemstoneDropRate = 1.0f;
            KillExpRate = 1.0f;
            RestHealRate = 1.0f;
            RareEncounterRate = 1.0f;
            RareSkillRarity = 1.0f;
            EpicSkillRarity = 1.0f;
            LegendarySkillRarity = 1.0f;
            RareMerchants = 1.0f;
            GoldBonusPerTurn = 0;
            FlameLevelRate = 1.0f;
            SkillActivation = 1.0f;
            BonusHeal = 0;
            GemStoneEffect = 0;
            SupportBonus = 0;
            ThanatosAppearRate = 1.0f;
        }

        private TimeOfDayBonuses dayBonuses;
        public TimeOfDayBonuses DayBonuses
        {
            get
            {
                if (dayBonuses == null)
                    dayBonuses = ScriptableObject.Instantiate(GameBPData.Instance.DayBonuses);
                return dayBonuses;
            }
        }

        private TimeOfDayBonuses nightBonuses;
        public TimeOfDayBonuses NightBonuses
        {
            get
            {
                if (nightBonuses == null)
                    nightBonuses = ScriptableObject.Instantiate(GameBPData.Instance.NightBonuses);
                return nightBonuses;
            }
        }
        public float ExperienceGain { get; set; }
        public float GoldGain { get; set; }
        public float GraceGain { get; set; }
        public float HealingRate { get; set; }
        public float AssistExpRate { get; set; }
        public float BondExpGain { get; set; }
        public float CurseResistance { get; set; }
        public float RelicDropRate { get; set; }
        public float EliteBattleRate { get; set; }
        public float FlameLevelRate { get; set; }
        public float FoodHealRate { get; set; }
        public float GemstoneDropRate { get; set; }
        public float KillExpRate { get; set; }
        public float RestHealRate { get; set; }
        public float RareEncounterRate { get; set; }
        public float GrowthIncrease { get; set; }
        public float RareSkillRarity { get; set; }
        public float EpicSkillRarity { get; set; }
        public float LegendarySkillRarity { get; set; }
        public float RareMerchants { get; set; }
        public int GoldBonusPerTurn { get; set; }
        public int BattleGoldReward { get; set; }
        public float SkillActivation { get; set; }
        public int BonusHeal { get; set; }
        public float GemStoneEffect { get; set; }
        public int SupportBonus { get; set; }
        public int FireDamage { get; set; }
        public int CombatSkillCost { get; set; }
        public float ThanatosAppearRate { get; set; }
    }
}