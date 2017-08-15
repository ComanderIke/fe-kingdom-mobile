using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AI.Stuff
{
    public class GoalWeightSet
    {
        public readonly float TENSION_FAKTOR;
        public readonly float VULNERABILITY_FAKTOR;
        public readonly float SOURCE_ATTACK_FAKTOR;
        public readonly float SOURCE_HP_FAKTOR;
        public readonly float SOURCE_DISTANCE_FAKTOR;
        public readonly float TARGET_DISTANCE_FAKTOR;
        public readonly float ATTACK_TARGET_BONUS;
        public GoalWeightSet()
        {
            TENSION_FAKTOR = 1;
            VULNERABILITY_FAKTOR = 1;
            SOURCE_ATTACK_FAKTOR = 1;
            SOURCE_HP_FAKTOR = 1;
            SOURCE_DISTANCE_FAKTOR = 1;
            TARGET_DISTANCE_FAKTOR = 10;
            ATTACK_TARGET_BONUS = 5;
        }
    }
    public class WeightSet
    {
        public readonly int STAY;
        public readonly float ALLY_UNIT_MULT;
        public readonly int HEAL_LOW_HP;
        public readonly int HEAL_HALF_HP;
        public readonly int HEAL_ALMOST_FULL_HP;
        public readonly int ATTACK_START_WEIGHT;
        public readonly int ATTACK_KILL_BONUS_WEIGHT;
        public readonly float ATTACK_KILL_HP_MULT;
        public readonly int ATTACK_OWN_DEATH_WEIGHT;
        public readonly float ATTACK_OWN_DEATH_HP_MULT;
        public readonly float RECEIVED_DAMAGE_MULT;
        public readonly float DEALT_DAMAGE_MULT;
        public readonly GoalWeightSet ATTACK_GOAL;

        public WeightSet(AIBehaviour b)
        {
            switch (b)
            {
                case AIBehaviour.aggressiv:
                    STAY = -1;
                    break;
                case AIBehaviour.guard:
                    STAY = 5;
                    break;
                case AIBehaviour.normal:
                    STAY = -1;
                    break;
                case AIBehaviour.passiv:
                    STAY = 1000;
                    break;
            }
            ALLY_UNIT_MULT = 1;
            HEAL_ALMOST_FULL_HP = 1;
            HEAL_LOW_HP = 1;
            HEAL_ALMOST_FULL_HP = 1;
            ATTACK_START_WEIGHT = 10;//start with 10 so enemies will also attack if they get more dmg themselves(Most players like Enemies attacks even if the attack doesn't do much)
            ATTACK_KILL_HP_MULT = 1f;
            ATTACK_KILL_BONUS_WEIGHT = 10;
            ATTACK_OWN_DEATH_HP_MULT = 0.5f;
            ATTACK_OWN_DEATH_WEIGHT = 3;
            RECEIVED_DAMAGE_MULT = 0.5f;
            DEALT_DAMAGE_MULT = 2.0f;
            ATTACK_GOAL = new GoalWeightSet();
        }
    }
}
