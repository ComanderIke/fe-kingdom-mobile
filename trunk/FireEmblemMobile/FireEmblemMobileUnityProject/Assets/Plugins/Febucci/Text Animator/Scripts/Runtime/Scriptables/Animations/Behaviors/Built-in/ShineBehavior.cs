using System.Security.Cryptography.X509Certificates;
using Febucci.UI.Core;
using UnityEngine;

namespace Febucci.UI.Effects
{
    [UnityEngine.Scripting.Preserve]
    [CreateAssetMenu(menuName = "Text Animator/Animations/Behaviors/Shine", fileName = "Shine Behavior")]
    [EffectInfo("shine", EffectCategory.Behaviors)]
    public sealed class ShineBehavior : BehaviorScriptableBase
    {
        public float baseFrequency = 0.5f;
        public float baseWaveSize = 0.08f;
        public float minValue = .5f;

        float frequency;
        float waveSize;
        
        public override void SetModifier(ModifierInfo modifier)
        {
            switch (modifier.name)
            {
                //frequency
                case "f": frequency = baseFrequency * modifier.value; break;
                //wave size
                case "s": waveSize = baseWaveSize * modifier.value; break;
                case "a": minValue =  modifier.value; break;
            }
        }

        public override void ResetContext(TAnimCore animator)
        {
            frequency = baseFrequency;
            waveSize = baseWaveSize;
            //minValue = .5f;
        }

        Color32 temp;
        public override void ApplyEffectTo(ref Core.CharacterData character, TAnimCore animator)
        {
            for (byte i = 0; i < TextUtilities.verticesPerChar; i++)
            {
                float h;
                float s;
                float v;
                Color.RGBToHSV(character.current.colors[i], out h, out s, out v);
                //shifts hue
                temp = Color.HSVToRGB(h, s, minValue+Mathf.PingPong(animator.time.timeSinceStart * frequency + character.index * waveSize, 1)*(1-minValue));
                temp.a = character.current.colors[i].a; //preserves original alpha
                character.current.colors[i] = temp;
            }
        }
    }
}