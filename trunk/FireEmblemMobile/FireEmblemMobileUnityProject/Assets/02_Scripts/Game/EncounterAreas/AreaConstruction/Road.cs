using __2___Scripts.Game.Utility;
using LostGrace;
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
        [SerializeField] GameObject ConnectionArrowPrefab;
        private ArrowAnimation instantiatedArrow;

        public void Start()
        {
            line = GetComponent<LineRenderer>();
        }

        public void NodeSelected()
        {
            if(instantiatedArrow!=null)
                instantiatedArrow.SetActive(true, end is BattleEncounterNode);
        }
        public void NodeDeselected()
        {
            if(instantiatedArrow!=null)
                instantiatedArrow.SetActive(false);
        }
        public void SetMoveable(bool moveable)
        {
            if (transform == null)
            {
                Debug.Log("TRANSFORM IS NULL PROB BUG");
                return;
                
            }
            if (transform.parent != null)
            {
                line.gameObject.transform.DeleteChildren();
                if (moveable)
                {
                    var go = Instantiate(ConnectionArrowPrefab, line.gameObject.transform);
                    var startPos = start.gameObject.transform.position;
                    var endPos = end.gameObject.transform.position;
                    Vector3 normalized = (endPos - startPos).normalized;
                    go.transform.position = startPos + normalized * .7f;

                    go.transform.right = endPos - go.transform.position;
                    go.GetComponent<ArrowAnimation>().SetTargetPosition(startPos + normalized * 1f);
                    instantiatedArrow = go.GetComponent<ArrowAnimation>();
                }
            }
            else
            {
                Debug.Log("TRANSFORM PARENT IS NULL PROB BUG");
                
            }
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