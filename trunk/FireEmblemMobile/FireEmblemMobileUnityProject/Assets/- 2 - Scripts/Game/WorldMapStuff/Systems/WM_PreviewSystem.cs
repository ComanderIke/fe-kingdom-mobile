using System.Linq;
using Game.WorldMapStuff.Input;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Model.Battle;
using GameEngine;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public class WM_PreviewSystem:IEngineSystem
    {
        private WM_AttackPreview attackPreview;

        public WM_PreviewSystem(IWM_AttackPreviewRenderer attackPreviewRenderer)
        {
            attackPreview = new WM_AttackPreview(attackPreviewRenderer);
            WM_PartySelectionSystem.OnDeselectParty += HideAttackPreview;
        }
        public void Init()
        {
            
        }

        public void ShowAttackPreview(WM_Actor party)
        {
            attackPreview.Show(party);
        }

        void HideAttackPreview()
        {
            attackPreview.Hide();
        }
    }
}