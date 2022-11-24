using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.UI;


namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Weapons/WeaponType", fileName = "weaponType1")]
    public class WeaponType: EffectType
    {
        [SerializeField] private String name;
        [SerializeField] private Sprite icon;

        public string Name => name;
        [SerializeField] private Dictionary<EffectType, float> effectiveAgainst;
        public Sprite Icon => icon;
        public bool IsEffective(EffectType effectType)
        {
            return effectiveAgainst.ContainsKey(effectType);
        }
        public float GetEffectiveCoefficient(EffectType effectType)
        {
            return effectiveAgainst[effectType];
        }
        
    }
}