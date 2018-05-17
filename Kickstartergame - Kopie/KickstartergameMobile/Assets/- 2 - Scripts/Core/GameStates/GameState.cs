
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    [System.Serializable]
    public abstract class GameState
    {
        
        public abstract void Enter();
       
        public abstract void Exit();

        public abstract void Update();
       

    }
}
