using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Grid.PathFinding
{
    [System.Serializable]
    public class MovementPath
    {
        private ArrayList steps = new ArrayList();

        public MovementPath()
        {

        }
        public int getLength()
        {
            return steps.Count;
        }
        public Step getStep(int index)
        {
            return (Step)steps[index];
        }
        public void prependStep(float x, float y)
        {
            steps.Add(new Step(x, y));
        }
        public void Reverse()
        {
            steps.Reverse();
        }
    }
}
