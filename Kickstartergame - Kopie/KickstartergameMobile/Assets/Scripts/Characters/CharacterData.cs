using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    [Serializable]
    public class CharacterData
    {
        public string name;
        public Stats stats;
        public int level;
        public int exp;
        public CharacterClassType type;
        public List<int> itemIndexes;

        public CharacterData(string name, int level, int exp, CharacterClassType type, Stats stats, List<int>itemIndexes)
        {
            this.name = name;
            this.level = level;
            this.exp = exp;
            this.type = type;
            this.stats = stats;
            this.itemIndexes = itemIndexes;

        }
    }
}
