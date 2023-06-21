using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.UI;


namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Weapons/WeaponType", fileName = "weaponType1")]
    public class WeaponType: EffectiveAgainstType
    {
        [SerializeField] private String weaponName;
        [SerializeField] private Sprite icon;

        public string WeaponName => weaponName;
        [SerializeField] private List<EffectiveAgainstType> effectiveAgainst;
  
        [SerializeField] private List<float> effectiveAgainstCoefficients;
        [SerializeField] private List<EffectiveAgainstType> advantageAgainst;
        private Dictionary<EffectiveAgainstType, float> effectiveAgainstDict;
        public Sprite Icon => icon;
        private Dictionary<EffectiveAgainstType, float> EffectiveAgainst{
            get
            {
                if (effectiveAgainstDict == null)
                { 
                    effectiveAgainstDict = new Dictionary<EffectiveAgainstType, float>();
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

        public bool HasAdvantage(EffectiveAgainstType effectiveAgainstType)
        {
            return advantageAgainst.Contains(effectiveAgainstType);
        }
        public bool IsEffective(EffectiveAgainstType effectiveAgainstType)
        {
            return EffectiveAgainst.ContainsKey(effectiveAgainstType);
        }
        public float GetEffectiveCoefficient(EffectiveAgainstType effectiveAgainstType)
        {
            return EffectiveAgainst[effectiveAgainstType];
        }
        
    }
}