using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Stuff
{
    class GoalResource
    {
        public Vector2 SourceLocation;
        public float Suitability;
    }
    public class Goal
    {
        public enum GoalType
        {
            NONE=0,
            ATTACK = 1
       
        }

        public GoalType GoalId;
        public Vector2 GoalTarget;
        public float Priority;
        public bool IsActive;
        List<GoalResource> PotentialResources;
        public int NeededResources;

        public Goal()
        {
            PotentialResources = new List<GoalResource>();
            IsActive = false;
            GoalTarget = new Vector2(0, 0);
        }

        public void CreateGoal(GoalType gt, int tx, int ty, WeightSet w)
        {
            // set basic goal properties
            GoalId = gt;
            GoalTarget.x = tx;
            GoalTarget.y = ty;
            IsActive = false;
            PotentialResources.Clear();

            // Determine Goal Priority based on goal type
            switch (GoalId)
            {
                case GoalType.ATTACK:
                    NeededResources = 100;
                    break;
            }
        }

        public bool HasSufficientResources
        {
            get
            {
                return (PotentialResources.Count >= NeededResources);
            }
        }

        public void AssignGoalResources()
        {
            // order our potential resources by suitability
            var resourcesBySuitability = from pr in PotentialResources
                                         orderby pr.Suitability descending
                                         select pr;

            // assign a number of resources equal to what we need
            int assignCount = 0;

            foreach (GoalResource gr in resourcesBySuitability)
            {
               
                // get unit representing this resource
                Character u = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().GetCharacterAtLocation(new Vector3(gr.SourceLocation.x,0,gr.SourceLocation.y));
                // double check that we got a valid unit
                if (u != null)
                {
                    // set goal for unit and bump count
                    u.SetAiGoal(GoalId, GoalTarget);
                    assignCount++;
                    // if we have assigned enough units, break out of loop
                    if (assignCount >= NeededResources)
                    {
                        break;
                    }
                }
            }
        }

        public void AssignUnitResourceSuitability(Character u, WeightSet w)
        {
            switch (GoalId)
            {
                case GoalType.ATTACK:
                    GetBaseUnitSuitabilityForAttackGoal(u, w);
                    break;
            }
        }

     

        private void AddPotentialUnitResource(Character u, float suitability)
        {
            GoalResource gr = new GoalResource();
            gr.SourceLocation.x = u.x;
            gr.SourceLocation.y = u.z;
            gr.Suitability = suitability;

            PotentialResources.Add(gr);
        }

        private float GetBaseDistanceFactor(int sx, int sy, int tx, int ty, float range)
        {
            float ret = (range > 0) ? (float)GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().GetDistance(sx, sy, tx, ty) / range : 5f;
            return 4f - ret;
        }

        private void GetBaseUnitSuitabilityForAttackGoal(Character u, WeightSet w)
        {
            float suitability=0;
            if (u.behaviour == AIBehaviour.aggressiv)
            {

                // get base distance rating
                suitability = GetBaseDistanceFactor(u.x, u.z, (int)GoalTarget.x, (int)GoalTarget.y, u.charclass.movRange + u.GetMaxAttackRange()) * w.ATTACK_GOAL.SOURCE_DISTANCE_FAKTOR;
                // add current hp rating
                suitability += (float)u.HP * w.ATTACK_GOAL.SOURCE_HP_FAKTOR;
                // add attack rating
                suitability += (float)u.GetDamage(true) * w.ATTACK_GOAL.SOURCE_ATTACK_FAKTOR;

            }
            AddPotentialUnitResource(u, suitability);
        }

    }
}
