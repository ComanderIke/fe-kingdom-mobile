using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class ActivatedSkillBp : SkillBP
    {
        [SerializeField] public int hpCost = 1;
        [SerializeField] public int Uses = 1;
    }
}