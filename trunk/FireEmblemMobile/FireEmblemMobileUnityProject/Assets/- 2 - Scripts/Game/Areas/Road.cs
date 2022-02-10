using UnityEngine;

namespace __2___Scripts.Game.Areas
{
    public class Road : MonoBehaviour
    {
        public Material standard;
        public Material moved;
        [HideInInspector]
        public LineRenderer line;
        [HideInInspector]
        public EncounterNode start;
        [HideInInspector]
        public EncounterNode end;

        public void Start()
        {
            line = GetComponent<LineRenderer>();
        }

        public void SetMovedVisual()
        {
            line.material = moved;
        }

        public void SetStartNode(EncounterNode child)
        {
            start = child;
            child.roads.Add(this);
        }
    }
}