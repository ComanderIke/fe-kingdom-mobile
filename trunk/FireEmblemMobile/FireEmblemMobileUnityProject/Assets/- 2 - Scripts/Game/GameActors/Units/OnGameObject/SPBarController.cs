using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public class SPBarController : ISPBarRenderer
    {
        public GameObject SPBarPrefab;

        public Transform Parent;
// Start is called before the first frame update
   
        public override void SetValue(int value)
        {
            foreach (Transform child in Parent) {
                GameObject.Destroy(child.gameObject);
            }

            for (int i = 0; i < value; i++)
            {
                Instantiate(SPBarPrefab, Parent, false);
            }
        }
    }
}