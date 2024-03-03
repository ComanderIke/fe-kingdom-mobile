using System;
using Game.GameActors.Units.Skills.Active;
using UnityEngine;

namespace Game.GameActors.Units.Skills.TODO
{
    [Serializable]

    [CreateAssetMenu(menuName = "GameData/Skills/Active/DarkClone", fileName = "DarkCloneSkillMixin")]
    public class DarkClone : SelfTargetSkillMixin
    {
      

        //Creates a clone of the unit as a real new temporary unit
        //Clone has a skill to swap position with the real one.
        //Clone lasts for a short time
        //Clone dies with 1 hit but can dodge => Dodge Build nice
        //only 1 use in order to limit cheesing with aggro pulling.
        
        
    }
}