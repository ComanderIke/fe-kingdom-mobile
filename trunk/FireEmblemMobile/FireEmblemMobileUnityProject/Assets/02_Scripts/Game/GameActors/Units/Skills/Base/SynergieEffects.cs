using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Passive;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.Skills.Base
{
    [System.Serializable]
    public class SynergieEffects
    {
        [SerializeField] public List<SkillEffectMixin> skillEffectMixins;
        [SerializeField] public bool replacesOtherEffects;
        [SerializeField] public ConditionCompareType compareTypeWithExistingConditions;
        [SerializeField] public ConditionBigPackage conditionManager;
        [FormerlySerializedAs("replacesOtherCOnditions")] [SerializeField] public bool replacesOtherConditions;
        [SerializeField] public int extraRange;
        [SerializeField] public int costReduction;

    }
}