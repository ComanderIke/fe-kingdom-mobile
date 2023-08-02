using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.Mechanics;
using Game.Mechanics.Battle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class CombatStatValueTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject statContainerPrefab;
        [SerializeField] private Transform statContainerParent;
     
        [SerializeField] private string labelSum;
      
        [SerializeField] private string labelEffects;
        [SerializeField] private string labelTerrain;
        [SerializeField] private string labelWeapon;
        [SerializeField] private string labelRelic;
        [SerializeField] private LayoutGroup layoutGroup;

        private List<GameObject> instantiatedObjects;
     

        public void Show(Unit unit, BonusStats.CombatStatType statType, Vector3 position)
        {
            instantiatedObjects ??= new List<GameObject>();
            Debug.Log("SHOW TOOLTIP FOR : "+statType);
            for (int i = instantiatedObjects.Count - 1; i >= 0; i--)
            {
                Destroy(instantiatedObjects[i]);
                instantiatedObjects.RemoveAt(i);
            }
            GetComponent<RectTransform>().anchoredPosition= position+ new Vector3(0,100,0);
           
             this.label.text = BonusStats.GetAsText(statType);
        
             var go = Instantiate(statContainerPrefab, statContainerParent);
             var statContainer = go.GetComponent<StatContainerUI>();
            
             bool physical = unit.equippedWeapon.DamageType == DamageType.Physical;
             Vector2Int AttributeBaseAndCombined = unit.Stats.GetAttributeValueOfCombatStat(statType, physical);
             switch (statType)
             {
                 case BonusStats.CombatStatType.Attack:
                     statContainer.SetValue(physical? Attributes.GetAsText((int)AttributeType.STR):Attributes.GetAsText((int)AttributeType.INT), physical?unit.Stats.CombinedAttributes().STR: unit.Stats.CombinedAttributes().INT,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go);
                     break;
                 case BonusStats.CombatStatType.Avoid:  
                   
                     statContainer.SetValue(Attributes.GetAsText((int)AttributeType.AGI)+" * "+BattleStats.AVO_AGI_MULT, unit.Stats.CombinedAttributes().AGI*BattleStats.AVO_AGI_MULT,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go);
                     break;
                 case BonusStats.CombatStatType.Crit: 
                     
                     statContainer.SetValue(Attributes.GetAsText((int)AttributeType.DEX), unit.Stats.CombinedAttributes().DEX,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go);
                     var go2 = Instantiate(statContainerPrefab, statContainerParent);
                     var statContainer2 = go2.GetComponent<StatContainerUI>();
                     
                     statContainer2.SetValue(Attributes.GetAsText((int)AttributeType.LCK), unit.Stats.CombinedAttributes().LCK,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go2);
                     break;
                 case BonusStats.CombatStatType.Critavoid: 
                    
                     statContainer.SetValue(Attributes.GetAsText((int)AttributeType.LCK)+" * "+BattleStats.CRIT_AVO_LCK_MULT, unit.Stats.CombinedAttributes().LCK*BattleStats.CRIT_AVO_LCK_MULT,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go);
                     break;
                 case BonusStats.CombatStatType.Hit: 
                    
                     statContainer.SetValue(Attributes.GetAsText((int)AttributeType.DEX)+" * "+BattleStats.HIT_DEX_MULT, unit.Stats.CombinedAttributes().DEX*BattleStats.HIT_DEX_MULT,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go);
                     break;
                 case BonusStats.CombatStatType.MagicResistance: 
                   
                     statContainer.SetValue(Attributes.GetAsText((int)AttributeType.FTH), unit.Stats.CombinedAttributes().FAITH,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go);
                     break;
                 case BonusStats.CombatStatType.PhysicalResistance: 
                    
                     statContainer.SetValue(Attributes.GetAsText((int)AttributeType.DEF), unit.Stats.CombinedAttributes().DEF,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go);
                     break;
                 case BonusStats.CombatStatType.AttackSpeed:
                    
                     statContainer.SetValue(Attributes.GetAsText((int)AttributeType.AGI), unit.Stats.CombinedAttributes().AGI,false, AttributeBonusState.Same);
                     instantiatedObjects.Add(go);
                     break;
             }
              
             int bonusFromWeapon = unit.Stats.BonusStatsFromWeapon.GetStatFromEnum(statType);
             if (bonusFromWeapon != 0)
             {
                 var weaponpGo = Instantiate(statContainerPrefab, statContainerParent);
                 var statContainerWeapon= weaponpGo.GetComponent<StatContainerUI>();
                 statContainerWeapon.SetValue(labelWeapon, bonusFromWeapon, true, AttributeBonusState.Same);
                 instantiatedObjects.Add(weaponpGo);
             }
             
             int bonusFromEquips = unit.Stats.BonusStatsFromEquips.GetStatFromEnum(statType);
             if (bonusFromEquips != 0)
             {
                 var equipGo = Instantiate(statContainerPrefab, statContainerParent);
                 var statContainerEquip= equipGo.GetComponent<StatContainerUI>();
                 statContainerEquip.SetValue(labelRelic, bonusFromEquips, true, bonusFromEquips>0?AttributeBonusState.Increasing:bonusFromEquips<0?AttributeBonusState.Decreasing:AttributeBonusState.Same);
                 instantiatedObjects.Add(equipGo);
             }
             int bonusFromEffects = unit.Stats.BonusStatsFromEffects.GetStatFromEnum(statType);
             if (bonusFromEffects != 0)
             {
                 var effectGo = Instantiate(statContainerPrefab, statContainerParent);
                 var statContainerEffect = effectGo.GetComponent<StatContainerUI>();
                 statContainerEffect.SetValue(labelEffects, bonusFromEffects, true, bonusFromEffects>0?AttributeBonusState.Increasing:bonusFromEffects<0?AttributeBonusState.Decreasing:AttributeBonusState.Same);
                 instantiatedObjects.Add(effectGo);
             }
             int bonusFromTerrain = unit.Stats.BonusStatsFromTerrain.GetStatFromEnum(statType);
             if (bonusFromTerrain != 0)
             {
                 var terrainGo = Instantiate(statContainerPrefab, statContainerParent);
                 var statContainerTerrain = terrainGo.GetComponent<StatContainerUI>();
                 statContainerTerrain.SetValue(labelTerrain, bonusFromTerrain, true, bonusFromTerrain>0?AttributeBonusState.Increasing:bonusFromTerrain<0?AttributeBonusState.Decreasing:AttributeBonusState.Same);
                 instantiatedObjects.Add(terrainGo);
             }
             var sumGo = Instantiate(statContainerPrefab, statContainerParent);
             var statContainerSum = sumGo.GetComponent<StatContainerUI>();
           
        
          //  int onlyBonuses = unit.BattleComponent.BattleStats.GetStatOnlyBonusesWithoutWeaponFromEnum(statType);
            int sumBonuses = unit.Stats.GetCombatStatBonuses(unit,statType,physical);// (AttributeBaseAndCombined.y-AttributeBaseAndCombined.x) + onlyBonuses;//if positive positive if negative negativ
             int sum = unit.BattleComponent.BattleStats.GetStatFromEnum(statType);
            // int sumWithoutBonuses = sum - onlyBonuses;
           
         
             statContainerSum.SetValue(labelSum,sum , false, sumBonuses > 0?AttributeBonusState.Increasing:sumBonuses < 0?AttributeBonusState.Decreasing:AttributeBonusState.Same);
             instantiatedObjects.Add(sumGo);
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;
        }
    }
}