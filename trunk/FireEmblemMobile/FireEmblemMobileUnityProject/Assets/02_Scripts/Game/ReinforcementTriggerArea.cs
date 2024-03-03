using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class ReinforcementTriggerArea : MonoBehaviour
    {
        [SerializeField]private List<Vector3> positions;

        private void OnDrawGizmos()
        {
            foreach (var pos in positions)
            {
                Gizmos.color = new Color(1,0,0,.25f);
                
                Gizmos.DrawCube(pos+new Vector3(.5f,.5f,0), new Vector3(1,1,1f));
            }
        }

        public bool Contains(Vector2 asVector)
        {
            foreach (var pos in positions)
            {
                if (Math.Abs(pos.x - asVector.x) < 0.01f && Math.Abs(pos.y - asVector.y) < 0.01f)
                    return true;
            }

            return false;
        }
    }
}