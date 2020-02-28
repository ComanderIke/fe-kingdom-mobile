using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [System.Serializable]
    public struct GoalWeightSet 
    {
        public float TENSION_FAKTOR;
        public float VULNERABILITY_FAKTOR;
        public float SOURCE_ATTACK_FAKTOR;
        public float SOURCE_HP_FAKTOR;
        public float SOURCE_DISTANCE_FAKTOR;
        public float TARGET_DISTANCE_FAKTOR;
        public float ATTACK_TARGET_BONUS;

        //public GoalWeightSet()
        //{
        //    TENSION_FAKTOR = 1;
        //    VULNERABILITY_FAKTOR = 1;
        //    SOURCE_ATTACK_FAKTOR = 1;
        //    SOURCE_HP_FAKTOR = 1;
        //    SOURCE_DISTANCE_FAKTOR = 1;
        //    TARGET_DISTANCE_FAKTOR = 10;
        //    ATTACK_TARGET_BONUS = 5;
        //}
    }
    [CreateAssetMenu(menuName = "GameData/AI/WeightSet", fileName = "WeightSet")]
    public class WeightSet : ScriptableObject
    {
        public int STAY;
        public float ALLY_UNIT_MULT;
        public int HEAL_LOW_HP;
        public int HEAL_HALF_HP;
        public int HEAL_ALMOST_FULL_HP;
        public int ATTACK_START_WEIGHT;
        public int ATTACK_KILL_BONUS_WEIGHT;
        public float ATTACK_KILL_HP_MULT;
        public int ATTACK_OWN_DEATH_WEIGHT;
        public float ATTACK_OWN_DEATH_HP_MULT;
        public float RECEIVED_DAMAGE_MULT;
        public float DEALT_DAMAGE_MULT;
        [SerializeField]
        public GoalWeightSet GOAL;

        //public WeightSet()//TODO ADD SWITCH BEHAVIOUR
        //{
        //    STAY = -1;
        //    ALLY_UNIT_MULT = 1;
        //    HEAL_ALMOST_FULL_HP = 1;
        //    HEAL_LOW_HP = 1;
        //    HEAL_ALMOST_FULL_HP = 1;
        //    ATTACK_START_WEIGHT = 10;//start with 10 so enemies will also attack if they get more dmg themselves(Most players like Enemies attacks even if the attack doesn't do much)
        //    ATTACK_KILL_HP_MULT = 1f;
        //    ATTACK_KILL_BONUS_WEIGHT = 10;
        //    ATTACK_OWN_DEATH_HP_MULT = 0.5f;
        //    ATTACK_OWN_DEATH_WEIGHT = 3;
        //    RECEIVED_DAMAGE_MULT = 0.5f;
        //    DEALT_DAMAGE_MULT = 2.0f;
        //    ATTACK_GOAL = new GoalWeightSet();
        //}
    }
}
