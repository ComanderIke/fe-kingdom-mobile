using System.Collections.Generic;
using Game.Dialog.DialogSystem;
using Game.EncounterAreas.Encounters.Church;
using Game.EncounterAreas.Encounters.Merchant;
using Game.EncounterAreas.Encounters.Smithy;
using Game.GameActors.Units.Skills.Base;
using Game.GUI.EncounterUI.Inn;
using UnityEngine;

namespace Game.MetaProgression
{
    [CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/GoldModifier", fileName = "MetaUpgrade1")]
    public class GoldModifierMetaUpgradeMixin: MetaUpgradeMixin
    {
        public float[] percentage;
        public GoldModifierType ModifierType;
        public override void Activate(int level)
        {
            switch (ModifierType)
            {
                case GoldModifierType.PriceBuying:
                    Merchant.PriceRateBuying = percentage[level]; break;
                case GoldModifierType.PriceSelling:
                    Merchant.PriceRateSelling = percentage[level]; break;
                case GoldModifierType.PriceShrine:
                    Church.PriceRate = percentage[level];
                    break;
                case GoldModifierType.PriceInn:
                    UIInnController.PriceRate = percentage[level];
                    break;
                case GoldModifierType.PriceSmithy:
                    Smithy.PriceRate = percentage[level];
                    break;
            }
        }
        public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
        {
            var list = new List<EffectDescription>();
            int upgLevel = level;
            if (upgLevel < percentage.Length-1)
                upgLevel++;
            list.Add(new EffectDescription(""+ModifierType.EnumToString()+":", TextUtility.FormatPercentage(percentage[level]), TextUtility.FormatPercentage(percentage[upgLevel])));
       
            return list;
    
        }
    }
    public enum GoldModifierType
    {
        PriceShrine,
        PriceSmithy,
        PriceSelling,
        PriceBuying,
        PriceInn,
    }
}