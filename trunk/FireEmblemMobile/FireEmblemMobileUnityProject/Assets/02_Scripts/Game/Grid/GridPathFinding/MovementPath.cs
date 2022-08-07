using System.Collections;

namespace Game.Grid.GridPathFinding
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

        public void PrependStep(int x, int y)
        {
            steps.Add(new Step(x, y));
        }

        public void Reverse()
        {
            steps.Reverse();
        }

        public int GetIndex(Step step)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                Step s = (Step)steps[i];
                if (s.GetX() == step.GetX() && s.GetY() == step.GetY())
                    return i;
            }

            return -1;
        }
    }
}