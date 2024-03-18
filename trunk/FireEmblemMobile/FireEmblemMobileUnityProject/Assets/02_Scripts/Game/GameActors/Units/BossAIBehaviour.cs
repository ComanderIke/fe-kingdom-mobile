using System.Linq;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Base;
using Game.Manager;
using Game.Systems;

namespace Game.GameActors.Units
{
    public class BossAIBehaviour:AIBehaviour
    {
        public int everyXTurnUseSkill = 3;
        public int turnSkillIndex = 0;
        public override Skill GetSkillToUse()
        {
            return agent.SkillManager.ActiveSkills.First();//TODO USE TURNSKILLINDEX?
        }

        public override void UpdateState(IAIAgent agent, bool hasAttackableTargets, bool usedSkill = false)
        {
            int turn = GridGameManager.Instance.GetSystem<TurnSystem>().TurnCount;
            if (turn % everyXTurnUseSkill == 0)
            {
                SetState( State.UseSkill);
            }
            else
                SetState(State.Aggressive);
        }
    }
}