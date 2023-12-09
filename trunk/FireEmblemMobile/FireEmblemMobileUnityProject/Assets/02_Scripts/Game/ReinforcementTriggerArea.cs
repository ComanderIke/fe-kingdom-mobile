using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
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
    }
}