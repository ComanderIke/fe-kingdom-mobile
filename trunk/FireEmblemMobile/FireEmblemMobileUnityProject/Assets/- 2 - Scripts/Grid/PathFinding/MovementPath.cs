using System.Collections;

namespace Assets.Grid.PathFinding
{
    [System.Serializable]
    public class MovementPath
    {
        private readonly ArrayList steps = new ArrayList();

        public MovementPath()
        {
        }

        public void Remove(int index)
        {
            steps.RemoveAt(index);
        }
        public int GetLength()
        {
            return steps.Count;
        }

        public Step GetStep(int index)
        {
            return (Step) steps[index];
        }

        public void PrependStep(float x, float y)
        {
            steps.Add(new Step(x, y));
        }

        public void Reverse()
        {
            steps.Reverse();
        }
    }
}