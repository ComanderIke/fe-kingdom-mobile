
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Debug.Log("TODO AI STATE");
        }

        public override void exit()
        {
			
        }

        public override void update()
        {
            simpleMonsterAI.update();
        }
    }
}
