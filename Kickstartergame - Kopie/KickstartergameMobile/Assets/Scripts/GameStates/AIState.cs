using Assets.Scripts.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    class AIState : GameState
    {
        //Dorothy dorothy;
        public AIState(Player p)
        {
            //dorothy = new Dorothy(p);

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
           // dorothy.update();
        }
    }
}
