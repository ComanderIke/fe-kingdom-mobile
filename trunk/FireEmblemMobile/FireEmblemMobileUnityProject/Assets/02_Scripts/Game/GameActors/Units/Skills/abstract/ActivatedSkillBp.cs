using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class ActivatedSkillBp : SkillBP
    {
        [SerializeField] private int hpCost = 1;
        [SerializeField] private int CastAmount = 1;
    }
}