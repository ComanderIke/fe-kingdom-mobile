using System.Collections.Generic;
using Game.Dialog.DialogSystem;
using Game.EncounterAreas.Encounters.Merchant;
using Game.EncounterAreas.Encounters.Smithy;
using Game.GameActors.Units.Skills.Base;
using Game.GUI.EncounterUI.Inn;
using Game.GUI.Utility;
using UnityEngine;

namespace Game.MetaProgression
{
    [CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/EncounterNodeModifier", fileName = "MetaUpgrade1")]
    public class EncounterNodeModifierMetaUpgradeMixin : MetaUpgradeMixin
    {
        public SerializableDictionary<EncounterNodeModifierType, int>[] modifiers;
        public override void Activate(int level)
        {
            foreach (KeyValuePair<EncounterNodeModifierType, int> keyValuePair in modifiers[level])
            {
                switch (keyValuePair.Key)
                {
                    case EncounterNodeModifierType.MerchantSlots:
                        Merchant.SlotCount = keyValuePair.Value; break;
                    case EncounterNodeModifierType.FoodSlotsInn:
                        UIInnController.FoodSlots = keyValuePair.Value;break;
                    case EncounterNodeModifierType.SmithingMaxUpgrade:
                        Smithy.MaxUpgradeLevel = keyValuePair.Value;break;
                
                }
            }
        }

        public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
        {
            var list = new List<EffectDescription>();
            foreach (var entry in modifiers[level])
            {
                list.Add(new EffectDescription(entry.Key.EnumToString()+":", "+"+entry.Value, "+"+entry.Value));
            }
       
            return list;
        }
    }

    public enum EncounterNodeModifierType
    {
        MerchantSlots,
        SmithingMaxUpgrade,
        FoodSlotsInn
    }
}