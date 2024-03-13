using Game.GameActors.Units;
using GameEngine;
using UnityEngine.UI;

namespace Game.Dialog
{
    public class DialogEngineSystem : IEngineSystem
    {
        public DialogueManager Renderer;
        public DialogEngineSystem()
        {
            
        }
        public void Init()
        {
            
        }

        public void UnitLevelupDialog(Unit unit)
        {
            Renderer.ShowDialog(unit.DialogComponent.GetNormalLevelUpConversation());
        }
        public void Deactivate()
        {
           
        }

        public void Activate()
        {
            
        }
    }
}