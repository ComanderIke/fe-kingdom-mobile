using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
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
