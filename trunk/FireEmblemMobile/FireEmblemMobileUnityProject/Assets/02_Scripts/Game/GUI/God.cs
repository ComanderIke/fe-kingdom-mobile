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
        public BlessingBP BlessingBp;
        public Material GlowMaterial;
        [field:SerializeField]public ChronikEntry ChronikComponent { get; set; }


        public Blessing GetBlessing()
        {
            return (Blessing)BlessingBp.Create();
        }
    }
}