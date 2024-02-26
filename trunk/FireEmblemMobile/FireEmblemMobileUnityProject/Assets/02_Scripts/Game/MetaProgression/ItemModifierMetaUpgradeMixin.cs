using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/ItemModifier", fileName = "MetaUpgrade1")]
public class ItemModifierMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<ResourceType, int> []startResource;
    public SerializableDictionary<ItemBP, int> []startItems;
    public SerializableDictionary<ItemModifierType, int>[] itemModifiers;
    public override void Activate(int level)
    {
        foreach (KeyValuePair<ResourceType, int> valuePair in startResource[level])
        {
            switch (valuePair.Key)
            {
                case ResourceType.Gold:
                    Party.StartGold = valuePair.Value; break;
            }
        }
        foreach (KeyValuePair<ItemBP, int> valuePair in startItems[level])
        {
            Convoy.StartItems.Add(valuePair.Key, valuePair.Value);
        }

        foreach (KeyValuePair<ItemModifierType, int> valuePair in itemModifiers[level])
        {
            switch (valuePair.Key)
            {
                case ItemModifierType.PotionsHeal:
                    HealthPotion.ExtraHealAmount = valuePair.Value;break;
                case ItemModifierType.HolyWaterCurse:
                    HolyWater.RemoveAll = true; break;
            }
        }
    }
    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        if(level <startResource.Length)
            foreach (var entry in startResource[level])
            {
                list.Add(new EffectDescription(entry.Key.EnumToString()+":", "+"+entry.Value, "+"+entry.Value));
              
            }
        if(level <startItems.Length)
            foreach (var entry in startItems[level])
            {
                list.Add(new EffectDescription(entry.Key.name+"s:", "+"+entry.Value, "+"+entry.Value));
              
            }
        if(level <itemModifiers.Length)
            foreach (var entry in itemModifiers[level])
            {
                list.Add(new EffectDescription(entry.Key.EnumToString()+":", "+"+entry.Value, "+"+entry.Value));
              
            }
       
        return list;
    
    }
}
public enum ItemModifierType
{
    PotionsHeal,
    HolyWaterCurse
}