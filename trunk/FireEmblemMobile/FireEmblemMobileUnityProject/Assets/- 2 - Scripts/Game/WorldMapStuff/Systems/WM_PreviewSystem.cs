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
        private WM_Actor currentEnemy;
        public WM_PreviewSystem(IWM_AttackPreviewRenderer attackPreviewRenderer)
        {
            attackPreview = new WM_AttackPreview(attackPreviewRenderer);
            
        }
        public void Init()
        {
            
        }

        public void Activate()
        {
            WM_PartySelectionSystem.OnDeselectParty += HideAttackPreview;
        }

        public void Deactivate()
        {
            WM_PartySelectionSystem.OnDeselectParty -= HideAttackPreview;
        }

        public void ShowAttackPreview(WM_Actor party)
        {
            attackPreview.Show(party);
            party.location.renderer.ShowAttackable();
            currentEnemy = party;
        }

        void HideAttackPreview()
        {
            attackPreview.Hide();
            if(currentEnemy!=null && currentEnemy.location!=null)
                currentEnemy.location.renderer.ShowAttackable();
        }
    }
}