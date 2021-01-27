using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GUI
{
    public abstract class IAttackPreviewUI : MonoBehaviour
    {
        public abstract void Show(BattlePreview battlePreview);
        public abstract void Hide();
    }
}