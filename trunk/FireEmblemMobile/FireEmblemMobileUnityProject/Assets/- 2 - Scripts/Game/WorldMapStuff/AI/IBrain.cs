namespace Game.AI
{
    public interface IBrain
    {
        public bool IsFinished();
        public void Think();
    }
}