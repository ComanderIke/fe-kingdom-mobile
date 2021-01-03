using Game.GameActors.Players;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameResources
{
    public class DemoData : MonoBehaviour
    {
        public Unit[] demoUnits;
        // Start is called before the first frame update
        void Start()
        {
            foreach (var unit in demoUnits)
            {
                var unitInst = Instantiate(unit);
                Player.Instance.Units.Add(unitInst);
            }
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
