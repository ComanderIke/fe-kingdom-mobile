using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Items;
using Game.GUI.EncounterUI.Inn;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/EncounterNodeModifier", fileName = "MetaUpgrade1")]
public class EncounterNodeModifierMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<EncounterNodeModifierType, int> modifiers;
    public override void Activate(int level)
    {
        foreach (KeyValuePair<EncounterNodeModifierType, int> keyValuePair in modifiers)
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
        foreach (var entry in modifiers)
        {
            list.Add(new EffectDescription(""+entry.Key, ""+entry.Value, ""+entry.Value));
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