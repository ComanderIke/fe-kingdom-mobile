using System.Collections.Generic;
using Game.GameActors.Units.Interfaces;

namespace Game.AI.DecisionMaking
{
    public class SkillUserComparer : IComparer<IAIAgent>
    {
        private AttackTargetComparer targetComparer;

        public SkillUserComparer()
        {
            targetComparer = new AttackTargetComparer();
        }
        public int Compare(IAIAgent x, IAIAgent y)
        {
            int ret = 0;
            ret = targetComparer.Compare(x.AIComponent.BestSkillTarget, y.AIComponent.BestSkillTarget);
            if (ret == 0)
            {
                if (x.MovementRange > y.MovementRange)
                {
                    ret = 1;
                }
                else if (x.MovementRange < y.MovementRange)
                    ret = -1;
            }

            if (ret == 0)
            {
                //Slot Order?
            }
            return ret;
        }
    }
}