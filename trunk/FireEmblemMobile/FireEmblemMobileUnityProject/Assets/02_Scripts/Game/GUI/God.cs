using System.Collections.Generic;
using Game.GameActors.Units.Visuals;
using Game.GameMechanics;
using Game.MetaProgression;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GUI
{
    [CreateAssetMenu(menuName = "GameData/God", fileName = "God")]
    public class God :ScriptableObject
    {
        public string Name;
        public DialogSpriteSet DialogSpriteSet;
        public DialogSpriteSet AlternateSpriteSet;
        [ColorUsage(true,true)]
        public Color bodyOutlineColor;
        public Color Color;
        public Color TooltipColor;
        public Color TooltipFrameColor;
        public Color upgradeBGColor;
        public BlessingBP BlessingBp;
        public Material GlowMaterial;
        [FormerlySerializedAs("statueSprite")] public Sprite StatueSprite;
        public List<MetaUpgradeBP> MetaUpgrades;
        [field:SerializeField]public ChronikEntry ChronikComponent { get; set; }
        [field:SerializeField]public float BondExpRate { get; set; }
        [field:SerializeField]public int MaxBondLevel { get; set; }


        public Blessing GetBlessing()
        {
            return (Blessing)BlessingBp.Create();
        }
    }
}