using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    [ExecuteInEditMode]
    public class Reinforcement : MonoBehaviour
    { 
       
        
        public int X => (int) transform.localPosition.x;
        public int Y => (int)transform.localPosition.y;
        public List<ReinforcementUnit> ReinforcementUnits;

        void Update()
        {
            transform.localPosition = new Vector3((int) transform.localPosition.x, (int) transform.localPosition.y,
                (int) transform.localPosition.z);
            if (ReinforcementUnits.Count == 0)
                return;
            gameObject.name = "Reinforcement_" + ReinforcementUnits[0].unitBp.name+"_"+ReinforcementUnits[0].Trigger;
            if (ReinforcementUnits[0].Trigger == ReinforcementTrigger.Turn)
                gameObject.name += "_Turn" + ReinforcementUnits[0].turn;
        }
    }
}
