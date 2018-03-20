using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters.SpecialAttacks
{
    public class SpecialAttackVisual
    {
        public Sprite Icon { get; set; }
        public GameObject Animation { get; set; }

        public SpecialAttackVisual(int iconId, int AnimationId)
        {
        }
    }
}
