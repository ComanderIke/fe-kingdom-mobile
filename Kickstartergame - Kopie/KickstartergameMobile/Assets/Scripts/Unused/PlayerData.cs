using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerData
    {
        public int playernumber;
        public string name;
        public List<CharacterData> characters;
        public float[] color;

        public PlayerData(string name, int number, List<CharacterData>characters, Color color)
        {
            this.name = name;
            this.color = new float[4];
            this.color[0] = color[0];
            this.color[1] = color[1];
            this.color[2] = color[2];
            this.color[3] = color[3];
            this.playernumber = number;
            this.characters = characters;
        }
    }
}
