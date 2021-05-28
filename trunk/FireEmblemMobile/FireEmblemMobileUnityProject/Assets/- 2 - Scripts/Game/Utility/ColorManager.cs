using UnityEngine;

namespace Utility
{
    [ExecuteInEditMode]
    public class ColorManager : MonoBehaviour {

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
      

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public Color GetFactionColor(int factionId)
        {
            return FactionColors[factionId];
        }
    }
}
