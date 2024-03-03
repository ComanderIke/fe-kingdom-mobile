using UnityEngine;

namespace Game.GameMechanics
{
    [CreateAssetMenu(menuName = "GameData/TimeOfDayBonuses")]
    public class TimeOfDayBonuses :ScriptableObject
    {
        public int curseResistance;
        //public int enemylevelsPerArea;
        public int critical;
        public int allyCritical;
        public int enemyCritical;
        // public string other;
      
        public int accuracy;
        public float graceModifier;
        public float goldModifier;
        public int healPerNode;
    }
}