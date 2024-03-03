using Game.GameActors.Items.Consumables;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UITargetAreaTile : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite defaultColor;
        [SerializeField] private Sprite green;
        [SerializeField] private Sprite red;
        [SerializeField] private Sprite orange;
        [SerializeField] private Sprite white;
        public void Show(EffectType effectType)
        {
            switch (effectType)
            {
                case EffectType.Bad: image.sprite = red; break;
                case EffectType.Good: image.sprite = orange; break;
                case EffectType.Heal: image.sprite = green; break;
                case EffectType.Neutral: image.sprite = white; break;
            }
        }

        public void ShowDefault()
        {
            image.sprite = defaultColor;
        }

        public void Blink()
        {
            LeanTween.color(image.rectTransform, new Color(1,1,1,.5f),1).setLoopPingPong(-1).setEaseInOutQuad();
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