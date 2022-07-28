using System;
using GameEngine;
using UnityEngine;

namespace Utility
{
    [CreateAssetMenu(fileName ="ColorManager", menuName = "GameData/Config/Colors")]
    public class ColorManager : SingletonScriptableObject<ColorManager> {

        public Color MainGreenColor;
        public Color MainRedColor;
      
        public Color MainWhiteColor;
        public Color MainGreyColor;
        public Color PhysicalDmgColor;
        public Color MagicDmgColor;
        public Color CriticalHitColor;
        public Color EffectiveDmgColor;
        public Color[] SixGradeColors;
        public Color[] FactionColors; 
        public Color[] FactionUIBackgroundColors;



        public Color GetFactionColor(FactionId factionId)
        {
            return FactionColors[(int)factionId];
        }
    }
}
