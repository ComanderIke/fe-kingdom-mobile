using System;
using UnityEngine;

namespace Utility
{
    [CreateAssetMenu(fileName ="ColorManager", menuName = "GameData/Config/Colors")]
    public class ColorManager : ScriptableObject {

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
        public static ColorManager Instance;

        private void OnEnable()
        {
            Instance = this;
        }

        public Color GetFactionColor(int factionId)
        {
            return FactionColors[factionId];
        }
    }
}
