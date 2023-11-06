using System.Collections.Generic;
using System.Linq;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using Game.GameResources;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class CharacterStateUI : MonoBehaviour
    {
        [SerializeField] private Transform statModifierParent;
        [SerializeField] private Transform negativeStateParent;
        [SerializeField] private Transform positiveStateParent;
        [SerializeField] private GameObject statePrefab;
        [SerializeField] private Image statBuffsImage;
        [SerializeField] private Color positiveStateColor;
        [SerializeField] private Color negativeStateColor;
        [SerializeField] private Sprite positiveStatModifierIcon;
        [SerializeField] private Sprite negativeStatModifierIcon;
        [SerializeField] private Sprite mixedStatModifierIcon;
        private StatusEffectManager statusEffectManager;
        private Dictionary<BuffDebuffBase, GameObject> instantiatedStatusEffects;


        public void Initialize(Unit unit, StatusEffectManager statusEffectManager)
        {
            instantiatedStatusEffects = new Dictionary<BuffDebuffBase, GameObject>();
            negativeStateParent.DeleteAllChildren();
            positiveStateParent.DeleteAllChildren();
            statusEffectManager.OnStatusEffectAdded += AddStatusEffect;
            statusEffectManager.OnStatusEffectRemoved+= RemoveStatusEffect;
            foreach (var state in statusEffectManager.Buffs)
            {
                AddStatusEffect(unit, state);
            }
            foreach (var state in statusEffectManager.Debuffs)
            {
                AddStatusEffect(unit, state);
            }
            foreach (var state in statusEffectManager.StatModifiers)
            {
                AddStatusEffect(unit, state);
            }
         
        }
        void AddStatusEffect(Unit unit,StatModifier state)
        {
            if (instantiatedStatusEffects.ContainsKey(state))
                return;
            var parent = statModifierParent;
            var go = Instantiate(statePrefab, parent);
            var image = go.GetComponent<Image>();
            bool positive = state.HasPositives();
            bool negative = state.HasNegatives();
            if(positive&&!negative)
            {
                image.sprite = positiveStatModifierIcon;
            }
            else if (!positive && negative)
            {
                image.sprite = negativeStatModifierIcon;
            }
            else
            {
                image.sprite = mixedStatModifierIcon;
            }
        
          
            go.SetActive(false);
            instantiatedStatusEffects.Add(state, go);
        }
        void AddStatusEffect(Unit unit,BuffDebuffBase state)
        {
            if (instantiatedStatusEffects.ContainsKey(state))
                return;
            bool good = state is Buff;
            var parent = good ? positiveStateParent : negativeStateParent;
            var go = Instantiate(statePrefab, parent);
            var image = go.GetComponent<Image>();
            image.color = good ? positiveStateColor : negativeStateColor;
            image.sprite = state.Icon;
            go.SetActive(false);
            instantiatedStatusEffects.Add(state, go);
        }
        void RemoveStatusEffect(Unit unit, BuffDebuffBase state)
        {
            if (!instantiatedStatusEffects.ContainsKey(state))
                return;
            Destroy(instantiatedStatusEffects[state]);
            instantiatedStatusEffects.Remove(state);
        }
      

    }
}