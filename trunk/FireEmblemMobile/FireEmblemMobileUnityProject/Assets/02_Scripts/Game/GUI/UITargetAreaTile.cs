using _02_Scripts.Game.GameActors.Items.Consumables;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UITargetAreaTile : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color green;
        [SerializeField] private Color red;
        [SerializeField] private Color orange;
        [SerializeField] private Color white;
        public void Show(EffectType effectType)
        {
            switch (effectType)
            {
                case EffectType.Bad: image.color = red; break;
                case EffectType.Good: image.color = orange; break;
                case EffectType.Heal: image.color = green; break;
                case EffectType.Neutral: image.color = white; break;
            }
        }

        public void ShowDefault()
        {
            image.color = defaultColor;
        }

        public void Blink()
        {
            LeanTween.color(image.rectTransform, defaultColor, 1f).setLoopPingPong(-1).setEaseInOutQuad();
        }

        public void StopBlink()
        {
            if (LeanTween.isTweening(image.gameObject))
            {
                LeanTween.cancel(image.gameObject);
            }
        }
    }
}