using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class ArrowAnimation : MonoBehaviour
    {
        // Start is called before the first frame update
         Vector3 targetPosition;
        void OnEnable()
        {
          
             LeanTween.move(gameObject,targetPosition, 1).setLoopPingPong(-1).setEaseInOutQuad();
        }

        public void SetTargetPosition(Vector3 targ)
        {
            this.targetPosition = targ;
            LeanTween.cancel(gameObject);
            OnEnable();
        }

        
    }
}
