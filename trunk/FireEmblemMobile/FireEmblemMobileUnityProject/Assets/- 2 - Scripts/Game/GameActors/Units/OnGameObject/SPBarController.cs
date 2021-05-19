using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public class SPBarController : ISPBarRenderer
    {
        public GameObject SPBarPrefab;
        public GameObject SPBarEmptyPrefab;
        public Transform Parent;

        private static readonly int Empty = Animator.StringToHash("Empty");
        // Start is called before the first frame update
   
        public override void SetValue(int value, int maxValue)
        {
            foreach (Transform child in Parent) {
                GameObject.Destroy(child.gameObject);
            }

            for (int i = 0; i < value; i++)
            {
                Instantiate(SPBarPrefab, Parent, false);
            }
            for (int i = value; i < maxValue; i++)
            {
                
                var go= Instantiate(SPBarEmptyPrefab, Parent, false);
                if (value == 0)
                {
                    go.GetComponent<Animator>().SetTrigger(Empty);
                }
            }
        }
    }
}