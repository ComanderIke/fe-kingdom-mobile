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
        [SerializeField] private LayoutGroup refreshLayout;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject statContainerPrefab;
        [SerializeField] private LayoutGroup statContainerParent;
        [SerializeField] private string labelBaseStats;
        [SerializeField] private string labelSum;
        [SerializeField] private string labelWeapon;
        [SerializeField] private string labelRelic;
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
            this.label.text = Attributes.GetAsLongText((int)attribute);
            var go = Instantiate(statContainerPrefab, statContainerParent.transform);
            var statContainer = go.GetComponent<StatContainerUI>();
            statContainer.SetValue(labelBaseStats, unit.Stats.BaseAttributes.GetAttributeStat(attribute), false, AttributeBonusState.Same);
            instantiatedObjects.Add(go);
            int bonusFromWeapon = unit.Stats.BonusAttributesFromWeapon.GetAttributeStat(attribute);
          
            if (bonusFromWeapon != 0)
            {
                
                var weaponGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                var statContainerWeapon= weaponGo.GetComponent<StatContainerUI>();
                statContainerWeapon.SetValue(labelWeapon, bonusFromWeapon, true, AttributeBonusState.Same);
                instantiatedObjects.Add(weaponGo);
            }
            int bonusFromEquips = unit.Stats.BonusAttributesFromEquips.GetAttributeStat(attribute);
          
            if (bonusFromEquips != 0)
            {
                
                var equipGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                var statContainerEquip= equipGo.GetComponent<StatContainerUI>();
                statContainerEquip.SetValue(labelRelic, bonusFromEquips, true, bonusFromEquips>0?AttributeBonusState.Increasing:bonusFromEquips<0?AttributeBonusState.Decreasing:AttributeBonusState.Same);
                instantiatedObjects.Add(equipGo);
            }
            int bonusFromEffects = unit.Stats.BonusAttributesFromEffects.GetAttributeStat(attribute);
            if (bonusFromEffects != 0)
            {
                var effectGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                var statContainerEffect = effectGo.GetComponent<StatContainerUI>();
                statContainerEffect.SetValue(labelEffects, bonusFromEffects, true, bonusFromEffects>0?AttributeBonusState.Increasing:bonusFromEffects<0?AttributeBonusState.Decreasing:AttributeBonusState.Same);
                instantiatedObjects.Add(effectGo);
            }

            if (instantiatedObjects.Count > 1)
            {
                var sumGo = Instantiate(statContainerPrefab, statContainerParent.transform);
                var statContainerSum = sumGo.GetComponent<StatContainerUI>();
                var bonusState=unit.Stats.GetAttributeBonusState(attribute);
                
                statContainerSum.SetValue(labelSum, unit.Stats.CombinedAttributes().GetAttributeStat(attribute), false,bonusState);
                instantiatedObjects.Add(sumGo);
            }

            // LayoutRebuilder.ForceRebuildLayoutImmediate(refreshLayout.transform as RectTransform);
            // refreshLayout.enabled = false;
            // refreshLayout.enabled = true;
            // LayoutRebuilder.ForceRebuildLayoutImmediate(statContainerParent.transform as RectTransform);
            // statContainerParent.enabled = false;
            // statContainerParent.enabled = true;
            // LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            // layoutGroup.enabled = false;
            // layoutGroup.enabled = true;
            // LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            // gameObject.SetActive(!gameObject.activeSelf);
            // gameObject.SetActive(!gameObject.activeSelf);
            // layoutGroup.CalculateLayoutInputHorizontal();
            // layoutGroup.CalculateLayoutInputVertical();
            // layoutGroup.SetLayoutHorizontal();
            // layoutGroup.SetLayoutVertical();
        }
    }
}
