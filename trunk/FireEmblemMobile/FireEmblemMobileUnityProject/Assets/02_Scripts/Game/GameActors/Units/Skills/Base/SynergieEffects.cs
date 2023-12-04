using System.Collections.Generic;
using Game.GameActors.Units.Skills.Passive;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    public class SynergieEffects
    {
        [SerializeField] public List<SkillEffectMixin> skillEffectMixins;
        [SerializeField] public bool replacesOtherEffects;
        [SerializeField] public ConditionCompareType compareTypeWithExistingConditions;
        [SerializeField] public ConditionBigPackage conditionManager;
        [SerializeField] public bool replacesOtherCOnditions;
        
    }
}