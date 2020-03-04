namespace Assets.Grid.PathFinding
{
    [System.Serializable]
    public class Step
    {
        private readonly float x;
        private readonly float y;

        public Step(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public float GetX()
        {
            return x;
        }
        public float GetY()
        {
            return y;
        }
        public override string ToString()
        {
            return "Step: " + x + " " + y;
        }
    }
}
