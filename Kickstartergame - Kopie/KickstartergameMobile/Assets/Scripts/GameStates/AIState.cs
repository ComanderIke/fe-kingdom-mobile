using Assets.Scripts;
using Assets.Scripts.Players;

namespace Assets.Scripts.GameStates
{
    class AIState : GameState
    {
        SimpleMonsterAI simpleMonsterAI;
        public AIState(Player p)
        {
            simpleMonsterAI = new SimpleMonsterAI(p);

        }
        public override void enter()
        {
        }

        public override void exit()
        {
			
        }

        public override void update()
        {
            simpleMonsterAI.Update();
        }
    }
}
