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
        public void Show(EffectType effectType)
        {
            switch (effectType)
            {
                case EffectType.Bad: image.color = red; break;
                case EffectType.Good: image.color = orange; break;
                case EffectType.Heal: image.color = Color.green; break;
            }
        }
    }
}