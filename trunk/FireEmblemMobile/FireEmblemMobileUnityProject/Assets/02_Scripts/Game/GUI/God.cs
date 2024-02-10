using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/God", fileName = "God")]
    public class God :ScriptableObject
    {
        public string Name;
        public DialogSpriteSet DialogSpriteSet;
        public DialogSpriteSet AlternateSpriteSet;
        public Color Color;
        public Color TooltipColor;
        public Color TooltipFrameColor;
        public Color upgradeBGColor;
        public BlessingBP BlessingBp;
        public Material GlowMaterial;
        public Sprite statueSprite;
        [field:SerializeField]public ChronikEntry ChronikComponent { get; set; }
        [field:SerializeField]public float BondExpRate { get; set; }
        [field:SerializeField]public int MaxBondLevel { get; set; }


        public Blessing GetBlessing()
        {
            return (Blessing)BlessingBp.Create();
        }
    }
}