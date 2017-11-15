using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Commands
{
    abstract class Command
    {
        public abstract void Execute();
        public abstract void Undo();
    }
}
