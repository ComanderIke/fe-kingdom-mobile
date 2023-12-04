using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class GlowSpot : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        public int X => (int)transform.localPosition.x;

        public int Y => (int)transform.localPosition.y;
    }
}
