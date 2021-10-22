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

        private static readonly int Vanish = Animator.StringToHash("Vanish");
        // Start is called before the first frame update
   
        public override void SetValue(int value, int maxValue)
        {
            int childs = Parent.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                GameObject.Destroy(Parent.GetChild(i).gameObject);
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
           
      
            // foreach (Transform child in Parent) {
            //     Debug.Log("destroy: "+child.name);
            //     DestroyImmediate(child.gameObject);
            // }
        }
        public override void SetPreviewValue(int value, int valueAfter,int maxValue)
        {
            int childs = Parent.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                GameObject.Destroy(Parent.GetChild(i).gameObject);
            }

            for (int i = 0; i < valueAfter; i++)
            {

                Instantiate(SPBarPrefab, Parent, false);
            }
            for (int i = valueAfter; i < value; i++)
            {
                
                var go= Instantiate(SPBarEmptyPrefab, Parent, false);

                go.GetComponent<Animator>().SetTrigger(Vanish);
                
                
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