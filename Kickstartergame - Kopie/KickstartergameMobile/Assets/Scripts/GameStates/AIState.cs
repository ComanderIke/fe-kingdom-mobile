using Assets.Scripts;
using Assets.Scripts.Players;
using UnityEngine;

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
            Debug.Log("Exit AI State");
        }

        public override void update()
        {
            simpleMonsterAI.Update();
        }
    }
}
