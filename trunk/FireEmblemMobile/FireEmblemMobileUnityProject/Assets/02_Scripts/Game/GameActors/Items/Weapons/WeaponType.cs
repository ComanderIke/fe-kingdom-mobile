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
        [SerializeField] private String weaponName;
        [SerializeField] private Sprite icon;

        public string WeaponName => weaponName;
        [SerializeField] private List<EffectType> effectiveAgainst;
  
        [SerializeField] private List<float> effectiveAgainstCoefficients;
        [SerializeField] private List<EffectType> advantageAgainst;
        private Dictionary<EffectType, float> effectiveAgainstDict;
        public Sprite Icon => icon;
        private Dictionary<EffectType, float> EffectiveAgainst{
            get
            {
                if (effectiveAgainstDict == null)
                { 
                    effectiveAgainstDict = new Dictionary<EffectType, float>();
                    if (effectiveAgainst != null)
                    {
                        for (int i = 0; i < effectiveAgainst.Count; i++)
                        {
                            effectiveAgainstDict.Add(effectiveAgainst[i], i<effectiveAgainstCoefficients.Count ? effectiveAgainstCoefficients[i]:1);
                        }
                    }
                }

                return effectiveAgainstDict;
            }
        }

        public bool HasAdvantage(EffectType effectType)
        {
            return advantageAgainst.Contains(effectType);
        }
        public bool IsEffective(EffectType effectType)
        {
            return EffectiveAgainst.ContainsKey(effectType);
        }
        public float GetEffectiveCoefficient(EffectType effectType)
        {
            return EffectiveAgainst[effectType];
        }
        
    }
}