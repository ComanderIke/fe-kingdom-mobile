using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Ressources
{
    [CreateAssetMenu(menuName= "GameData/TextData",fileName = "TextData")]
    public class TextData: UnityEngine.ScriptableObject
    {
        public string[] textData;
        
    }
}
