using UnityEngine;

namespace Assets.GameActors.Units.Skills
{
    public class SkillSpriteSet
    {
        public SkillSpriteSet(Sprite sprite, Sprite spriteDisabled, Sprite spriteHovered, Sprite spriteLevelup, Sprite spriteLevelupPressed)
        {
            Sprite = sprite;
            SpriteDisabled = spriteDisabled;
            SpriteHovered = spriteHovered;
            SpriteLevelup = spriteLevelup;
            SpriteLevelupPressed = spriteLevelupPressed;
        }

        public Sprite Sprite { get; private set; }
        public Sprite SpriteDisabled { get; private set; }
        public Sprite SpriteHovered { get; private set; }
        public Sprite SpriteLevelup { get; private set; }
        public Sprite SpriteLevelupPressed { get; private set; }
    }
}