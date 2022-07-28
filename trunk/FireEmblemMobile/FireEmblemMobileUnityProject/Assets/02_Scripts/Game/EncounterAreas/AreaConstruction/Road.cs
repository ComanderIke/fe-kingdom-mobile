using UnityEngine;

namespace __2___Scripts.Game.Areas
{
    public class Road : MonoBehaviour
    {
        public Material standard;
        public Material moved;
       // [HideInInspector]
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
            if (transform == null)
            {
                Debug.Log("TRANSFORM IS NULL PROB BUG");
                return;
                
            }

            if (transform.parent != null)
            {
                Debug.Log(transform.parent.name);
                line.material = moved;
            }
            else
            {
                Debug.Log("TRANSFORM PARENT IS NULL PROB BUG");
                
            }
        }

        public void SetStartNode(EncounterNode child)
        {
            start = child;
            child.roads.Add(this);
        }
    }
}