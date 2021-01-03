namespace Game.Mechanics.Commands
{
    public abstract class Command
    {
        public bool Finished = false;
        public abstract void Execute();
        public abstract void Undo();
    }
}