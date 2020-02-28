using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Grid.PathFinding
{
    [System.Serializable]
    public class Step
    {
        private float x;
        private float y;

        public Step(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public float getX()
        {
            return x;
        }
        public float getY()
        {
            return y;
        }
        public override string ToString()
        {
            return "Step: " + x + " " + y;
        }
    }
}
