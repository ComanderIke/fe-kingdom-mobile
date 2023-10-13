using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class InteractionBlocker : MonoBehaviour
    {
        public static InteractionBlocker Instance;
        void Start()
        {
            callers = new Dictionary<GameObject, bool>();
            Instance = this;
            gameObject.SetActive(false);
        }

        void Update()
        {
        
        }

        private Dictionary<GameObject, bool> callers;
        public void SetActive(GameObject caller, bool value)
        {
            if (callers.ContainsKey(caller))
            {
                callers[caller] = value;
            }
            else
            {
                callers.Add(caller, value);
            }

            bool isAnyTrue = false;
            foreach (bool var in callers.Values)
            {
                if (var)
                    isAnyTrue = true;
            }
            // Debug.Log("Caller: "+caller+ " "+value);
            // Debug.Log("IsAnyTrue: "+isAnyTrue);
            if(isAnyTrue)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }
    }
}
