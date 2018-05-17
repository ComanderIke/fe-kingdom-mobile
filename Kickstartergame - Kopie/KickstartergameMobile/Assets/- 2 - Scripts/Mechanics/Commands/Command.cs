using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Commands
{
    public abstract class Command
    {
        public bool finished = false;
        public abstract void Execute();
        public abstract void Undo();
    }
}
