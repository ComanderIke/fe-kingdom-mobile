using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GUI.EncounterUI.Inn
{
    [CreateAssetMenu(menuName = "GameData/Inn/Recipe")]
    public class Recipe:ScriptableObject
    {
        public int price;
        public string name;
        public int heal;
        public int bonuses;
        public Sprite icon;
        public InnBonusType bonusType;
        public List<AttributeType> AttributeType;

        public enum InnBonusType
        {
            Exp,
            Temporary,
            Permanent,
            RefreshSkills,
        }

        public bool IsLiquor;
    }
}