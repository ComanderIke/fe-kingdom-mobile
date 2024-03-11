using System.Collections.Generic;
using Game.GameActors.Units.Interfaces;

namespace Game.GameActors.Units
{
    public class AIGroup
    {
        public AIBehaviour.State state = AIBehaviour.State.Patrol;
        private List<IAIAgent> agents;

        public AIGroup(List<IAIAgent> agents, AIBehaviour.State state)
        {
            this.agents = agents;
            this.state = state;
            foreach (var agent in agents)
            {
                agent.AIComponent.AIBehaviour.aiGroup = this;
            }
        }

        public void AddAgent(IAIAgent agent)
        {
            agents.Add(agent);
            agent.AIComponent.AIBehaviour.aiGroup = this;
        }

        public void SetState(AIBehaviour.State state1)
        {
            this.state = state1;
        }
    }
}