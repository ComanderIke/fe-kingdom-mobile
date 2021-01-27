using Game.GameActors.Units;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GUI
{
    public abstract class IAttackPreviewUI : MonoBehaviour
    {
        public abstract void Show(BattlePreview battlePreview, UnitVisual attackerVisual, UnitVisual defenderVisual);
        public abstract void Hide();
    }
}