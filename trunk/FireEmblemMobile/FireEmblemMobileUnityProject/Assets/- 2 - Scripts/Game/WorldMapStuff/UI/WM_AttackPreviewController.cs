using UnityEngine;

namespace Game.WorldMapStuff.UI
{
    public class WM_AttackPreviewController : MonoBehaviour, IWM_AttackPreviewRenderer
    {

        public Canvas canvas;

        public GameObject attackPreview;

        public void Show(Vector3 worldPos)
        {
            canvas.enabled = true;
            attackPreview.transform.position = Camera.main.WorldToScreenPoint(worldPos);

        }

        public void Hide()
        {
            canvas.enabled = false;
        }
    }
}