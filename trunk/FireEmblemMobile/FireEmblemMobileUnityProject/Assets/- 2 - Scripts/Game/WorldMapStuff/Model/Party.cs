using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [CreateAssetMenu(fileName = "Party", menuName = "GameData/Party")]
    public class Party:WM_Actor
    {

   

        public GameTransformManager GameTransformManager { get; set; }
  
        [SerializeField]
        private List<Unit> members;
        public static Action<Party> PartyDied;
        public WM_ActorRenderer WmActorRenderer { get; set; }

        public Party()
        {
            members = new List<Unit>();
            TurnStateManager = new TurnStateManager(this);
            
        }
    

        public bool IsAlive()
        {
            return members.Count(a => a.IsAlive()) != 0;
        }

        public override void SetAttackTarget(bool b)
        {
            Debug.Log("TODO AttackTarget Visuals");
        }

   

        public override void ResetPosition()
        {
            Debug.Log("ResetLocation");
        }
    }
}