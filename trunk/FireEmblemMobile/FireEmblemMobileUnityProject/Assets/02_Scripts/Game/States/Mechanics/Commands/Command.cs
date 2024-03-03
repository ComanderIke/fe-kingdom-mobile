namespace Game.States.Mechanics.Commands
{
    public abstract class Command
    {
        public bool IsFinished;
        public abstract void Execute();
        public abstract void Undo();

        public abstract void Update();
    }
}