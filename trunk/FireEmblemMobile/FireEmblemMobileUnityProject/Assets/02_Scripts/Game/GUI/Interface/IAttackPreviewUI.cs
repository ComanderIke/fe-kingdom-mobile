using Game.GameActors.Units;
using Game.States.Mechanics.Battle;
using UnityEngine;

namespace Game.GUI.Interface
{
    public abstract class IAttackPreviewUI : MonoBehaviour
    {
     
        public abstract void Show(BattlePreview battlePreview, Unit attacker, Unit defender, string attackLabel);
        public abstract void Hide();
        public abstract void Show(BattlePreview battlePreview, Unit attacker, string attackLabel, Sprite attackableObjectSprite);
    }
}