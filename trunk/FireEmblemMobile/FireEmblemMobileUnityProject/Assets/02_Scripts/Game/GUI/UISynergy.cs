using Game.GameActors.Units.Skills.Base;
using Game.GameMechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UISynergy : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image glowBorder;

        public void Show(BlessingBP blessing, SynergieEffects synergieEffects, bool blessed)
        {
            glowBorder.enabled = blessed;
            icon.sprite = blessing.Icon;
        }
    }
}
