using UnityEngine;

namespace Game.GUI.EncounterUI.Inn
{
    [CreateAssetMenu(menuName = "GameData/Inn/Recipe")]
    public class Recipe:ScriptableObject
    {
        public int price;
        public string name;
        public int heal;
        public string bonuses;
        public Sprite icon;
    }
}