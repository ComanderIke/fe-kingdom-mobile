using System;
using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    public class SelectableCharacterUI : MonoBehaviour
    {
        private Unit unit;
        [SerializeField]private UIUnitIdleAnimation unitUIIdleAnimation;
        public event Action<Unit> onClicked;

        public void SetCharacter(Unit unit)
        {
            this.unit = unit;
            unitUIIdleAnimation.Show(unit);
        }

        public void Clicked()
        {
            onClicked?.Invoke(unit);
        }
    }
}