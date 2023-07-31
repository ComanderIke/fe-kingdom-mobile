using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class AttributeValueTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject statContainerPrefab;
        [SerializeField] private Transform statContainerParent;
        [SerializeField] private string labelBaseStats;
        [SerializeField] private string labelSum;
        [SerializeField] private string labelEquipment;
        [SerializeField] private string labelEffects;
      
        [SerializeField] private LayoutGroup layoutGroup;
      
        private List<GameObject> instantiatedObjects;
        
      
        public void Show(Unit unit, AttributeType attribute, Vector3 position)
        {
            instantiatedObjects ??= new List<GameObject>();
            Debug.Log("CONTAINER OBJECTS BEFORE: "+ instantiatedObjects.Count);
            for (int i = instantiatedObjects.Count - 1; i >= 0; i--)
            {
                Debug.Log("DESTROY ONE OBJECT" +i);
                Destroy(instantiatedObjects[i]);
                instantiatedObjects.RemoveAt(i);
            }
            Debug.Log("CONTAINER OBJECTS AFTER DESTROY: "+ instantiatedObjects.Count);
            GetComponent<RectTransform>().anchoredPosition= position+ new Vector3(0,100,0);
            this.label.text = Attributes.GetAsText((int)attribute);
            var go = Instantiate(statContainerPrefab, statContainerParent);
            var statContainer = go.GetComponent<StatContainerUI>();
            statContainer.SetValue(labelBaseStats, unit.Stats.BaseAttributes.GetAttributeStat(attribute), false, StatContainerUI.ColorState.Same);
            instantiatedObjects.Add(go);
            int bonusFromEquips = unit.Stats.BonusAttributesFromEquips.GetAttributeStat(attribute);
          
            if (bonusFromEquips != 0)
            {
                
                var equipGo = Instantiate(statContainerPrefab, statContainerParent);
                var statContainerEquip= equipGo.GetComponent<StatContainerUI>();
                statContainerEquip.SetValue(labelEquipment, bonusFromEquips, true, bonusFromEquips>0?StatContainerUI.ColorState.Increasing:bonusFromEquips<0?StatContainerUI.ColorState.Decreasing:StatContainerUI.ColorState.Same);
                instantiatedObjects.Add(equipGo);
            }
            int bonusFromEffects = unit.Stats.BonusAttributesFromEffects.GetAttributeStat(attribute);
            if (bonusFromEffects != 0)
            {
                var effectGo = Instantiate(statContainerPrefab, statContainerParent);
                var statContainerEffect = effectGo.GetComponent<StatContainerUI>();
                statContainerEffect.SetValue(labelEffects, bonusFromEffects, true, bonusFromEffects>0?StatContainerUI.ColorState.Increasing:bonusFromEffects<0?StatContainerUI.ColorState.Decreasing:StatContainerUI.ColorState.Same);
                instantiatedObjects.Add(effectGo);
            }

            if (instantiatedObjects.Count > 1)
            {
                var sumGo = Instantiate(statContainerPrefab, statContainerParent);
                var statContainerSum = sumGo.GetComponent<StatContainerUI>();
                statContainerSum.SetValue(labelSum, unit.Stats.CombinedAttributes().GetAttributeStat(attribute), false,
                    unit.Stats.BaseAttributes.GetAttributeStat(attribute) >
                    unit.Stats.CombinedAttributes().GetAttributeStat(attribute)
                        ?
                        StatContainerUI.ColorState.Increasing
                        : unit.Stats.BaseAttributes.GetAttributeStat(attribute) <
                          unit.Stats.CombinedAttributes().GetAttributeStat(attribute)
                            ? StatContainerUI.ColorState.Decreasing
                            : StatContainerUI.ColorState.Same);
                instantiatedObjects.Add(sumGo);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;
        }
    }
}
