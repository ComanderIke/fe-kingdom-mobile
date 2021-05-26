namespace Game.Grid.GridPathFinding
{
    [System.Serializable]
    public class Step
    {
        private readonly int x;
        private readonly int y;

        public Step(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int GetX()
        {
            return x;
        }
        public int GetY()
        {
            return y;
        }
        public override string ToString()
        {
            return "Step: " + x + " " + y;
        }
    }
}
