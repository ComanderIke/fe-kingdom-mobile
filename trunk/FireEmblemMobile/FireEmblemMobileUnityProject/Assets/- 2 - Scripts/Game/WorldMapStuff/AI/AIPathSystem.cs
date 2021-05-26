using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Game.WorldMapStuff.AI
{
    public class AIPathSystem : MonoBehaviour
    {
        // public List<AIPath> agents;
    
        // Start is called before the first frame update
        void Start()
        {
            //SpriteOutLinePressed.OnClicked += LocationClicked;
        }

        // Update is called once per frame
        void Update()
        {
            if(UnityEngine.Input.GetMouseButtonDown(0))
            {
                var pos=Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                LocationClicked(pos);
            }
        }

        // public void LocationClicked(GameObject destination)
        // {
        //     StartCoroutine(SetDestinationForAgents(destination.transform.position));
        //
        // }
        public void LocationClicked(Vector3 destination)
        {
            // StartCoroutine(SetDestinationForAgents(destination));
   
        }

        // IEnumerator SetDestinationForAgents(Vector3 destination)
        // {
        //     foreach (var agent in agents)
        //     {
        //         agent.destination = destination;//+new Vector3(Random.Range(-0.25f,0.25f),Random.Range(-0.25f,0.25f));
        //         yield return new WaitForSeconds(Random.Range(0.1f,0.4f));
        //     }
        //     yield return null;
        // }
    
    }
}
