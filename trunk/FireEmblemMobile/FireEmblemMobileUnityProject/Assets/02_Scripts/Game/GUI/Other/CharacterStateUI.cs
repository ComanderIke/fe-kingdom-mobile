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
        [SerializeField] private Transform negativeStateParent;
        [SerializeField] private Transform positiveStateParent;
        [SerializeField] private GameObject statePrefab;
        [SerializeField] private Image statBuffsImage;
        [SerializeField] private Color positiveStateColor;
        [SerializeField] private Color negativeStateColor;
        private StatusEffectManager statusEffectManager;
        private Dictionary<StatusEffect, GameObject> instantiatedStatusEffects;


        public void Initialize(StatusEffectManager statusEffectManager)
        {
            instantiatedStatusEffects = new Dictionary<StatusEffect, GameObject>();
            negativeStateParent.DeleteAllChildren();
            positiveStateParent.DeleteAllChildren();
            statusEffectManager.OnStatusEffectAdded += AddStatusEffect;
            statusEffectManager.OnStatusEffectRemoved-= RemoveStatusEffect;
            foreach (var state in statusEffectManager.StatusEffects)
            {
                AddStatusEffect(state);
            }
         
        }

        void AddStatusEffect(StatusEffect state)
        {
            if (instantiatedStatusEffects.ContainsKey(state))
                return;
            var parent = state.Positive ? positiveStateParent : negativeStateParent;
            var go = Instantiate(statePrefab, parent);
            var image = go.GetComponent<Image>();
            image.color = state.Positive ? positiveStateColor : negativeStateColor;
            image.sprite = state.Icon;
            go.SetActive(false);
            instantiatedStatusEffects.Add(state, go);
        }
        void RemoveStatusEffect(StatusEffect state)
        {
            if (!instantiatedStatusEffects.ContainsKey(state))
                return;
            Destroy(instantiatedStatusEffects[state]);
            instantiatedStatusEffects.Remove(state);
        }
      

    }
}