using UnityEngine;

namespace Game
{
    public class MoveWithObject : MonoBehaviour
    {
        [SerializeField] private GameObject follow;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = follow.transform.position;
        }
    }
}
