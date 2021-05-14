using UnityEngine;

namespace Game.WorldMapStuff.Model.Battle
{
    public class WM_AttackPreview
    {
        public IWM_AttackPreviewRenderer renderer;

        public WM_AttackPreview(IWM_AttackPreviewRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void Show(WM_Actor actor)
        {
            Debug.Log("SHOW ATTACK PREVIEW :)");
            renderer.Show(actor.GameTransformManager.Transform.position);
        }

        public void Hide()
        {
            renderer.Hide();
        }
    }
}