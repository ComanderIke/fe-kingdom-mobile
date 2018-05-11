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
        public abstract void enter();
       
        public abstract void exit();

        public abstract void update();

    }
}
