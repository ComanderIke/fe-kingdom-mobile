using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class AttributeValueTooltipUI : MonoBehaviour
    {
        [SerializeField] private LayoutGroup refreshLayout;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject statContainerPrefab;
        [SerializeField] private LayoutGroup statContainerParent;
        [SerializeField] private string labelBaseStats;
        [SerializeField] private string labelSum;
        [SerializeField] private string labelWeapon;
        [SerializeField] private string labelRelic;
        [SerializeField] private string labelFood;
        [SerializeField] private string labelEffects;
        [SerializeField] private string labelBlessing="Blessing";
        [SerializeField] private string labelBonusGrowths="Bonus";
      
        [SerializeField] private LayoutGroup layoutGroup;
      
        private List<GameObject> instantiatedObjects;
        
      
        public void Show(Unit unit, AttributeType attribute, Vector3 position, bool growth=false)
        {
            instantiatedObjects ??= new List<GameObject>();
            
            for (int i = instantiatedObjects.Count - 1; i >= 0; i--)
            {
               
                Destroy(instantiatedObjects[i]);
                instantiatedObjects.RemoveAt(i);
            }
            GetComponent<RectTransform>().anchoredPosition= position+ new Vector3(0,100,0);
            this.label.text = growth?Attributes.GetAsText((int)attribute)+" Growth": Attributes.GetAsLongText((int)attribute);
            var go = Instantiate(statContainerPrefab, statContainerParent.transform);
            var statContainer = go.GetComponent<StatContainerUI>();
            var value = unit.Stats.BaseAttributes.GetAttributeStat(attribute);
            if(growth)
                value= unit.Stats.BaseGrowths.GetAttributeStat(attribute);
            statContainer.SetValue(labelBaseStats, value, false,
                AttributeBonusState.Same);
            instantiatedObjects.Add(go);

                if (growth)
                {
                    int bonusGrowths = unit.Stats.BonusGrowths.GetAttributeStat(attribute);
                    if (bonusGrowths != 0)
                    {
                        var effectGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                        var statContainerEffect = effectGo.GetComponent<StatContainerUI>();
                        statContainerEffect.SetValue(labelBonusGrowths, bonusGrowths, true,
                            bonusGrowths > 0 ? AttributeBonusState.Increasing :
                            bonusGrowths < 0 ? AttributeBonusState.Decreasing : AttributeBonusState.Same);
                        instantiatedObjects.Add(effectGo);
                    }
                }
                else
                {
                int bonusFromWeapon = unit.Stats.BonusAttributesFromWeapon.GetAttributeStat(attribute);
                if (bonusFromWeapon != 0)
                {

                    var weaponGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                    var statContainerWeapon = weaponGo.GetComponent<StatContainerUI>();
                    statContainerWeapon.SetValue(labelWeapon, bonusFromWeapon, true, AttributeBonusState.Same);
                    instantiatedObjects.Add(weaponGo);
                }

                int bonusFromEquips = unit.Stats.BonusAttributesFromEquips.GetAttributeStat(attribute);

                if (bonusFromEquips != 0)
                {

                    var equipGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                    var statContainerEquip = equipGo.GetComponent<StatContainerUI>();
                    statContainerEquip.SetValue(labelRelic, bonusFromEquips, true,
                        bonusFromEquips > 0 ? AttributeBonusState.Increasing :
                        bonusFromEquips < 0 ? AttributeBonusState.Decreasing : AttributeBonusState.Same);
                    instantiatedObjects.Add(equipGo);
                }

                int bonusFromBlessings = unit.Stats.BonusAttributesFromBlessings.GetAttributeStat(attribute);
                if (bonusFromBlessings != 0)
                {
                    var effectGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                    var statContainerEffect = effectGo.GetComponent<StatContainerUI>();
                    statContainerEffect.SetValue(labelBlessing, bonusFromBlessings, true,
                        bonusFromBlessings > 0 ? AttributeBonusState.Increasing :
                        bonusFromBlessings < 0 ? AttributeBonusState.Decreasing : AttributeBonusState.Same);
                    instantiatedObjects.Add(effectGo);
                }

                int bonusFromFoods = unit.Stats.BonusAttributesFromFood.GetAttributeStat(attribute);

                if (bonusFromFoods != 0)
                {

                    var equipGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                    var statContainerEquip = equipGo.GetComponent<StatContainerUI>();
                    statContainerEquip.SetValue(labelFood, bonusFromFoods, true,
                        bonusFromFoods > 0 ? AttributeBonusState.Increasing :
                        bonusFromFoods < 0 ? AttributeBonusState.Decreasing : AttributeBonusState.Same);
                    instantiatedObjects.Add(equipGo);
                }

                int bonusFromEffects = unit.Stats.BonusAttributesFromEffects.GetAttributeStat(attribute);
                if (bonusFromEffects != 0)
                {
                    var effectGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                    var statContainerEffect = effectGo.GetComponent<StatContainerUI>();
                    statContainerEffect.SetValue(labelEffects, bonusFromEffects, true,
                        bonusFromEffects > 0 ? AttributeBonusState.Increasing :
                        bonusFromEffects < 0 ? AttributeBonusState.Decreasing : AttributeBonusState.Same);
                    instantiatedObjects.Add(effectGo);
                }
            }

            if (instantiatedObjects.Count > 1)
            {
                var sumGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                var statContainerSum = sumGo.GetComponent<StatContainerUI>();
                var bonusState=unit.Stats.GetAttributeBonusState(attribute);
              
                var attributeValue = unit.Stats.CombinedAttributes().GetAttributeStat(attribute);
                if (growth)
                {
                    bonusState = unit.Stats.GetGrowthBonusState(attribute);
                    attributeValue = unit.Stats.CombinedGrowths().GetAttributeStat(attribute);
                }
                    
                
                
                statContainerSum.SetValue(labelSum, attributeValue, false,bonusState);
                instantiatedObjects.Add(sumGo);
            }
            
        }
    }
}
