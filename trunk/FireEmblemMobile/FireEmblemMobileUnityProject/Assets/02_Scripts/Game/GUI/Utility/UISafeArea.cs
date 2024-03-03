using UnityEngine;

namespace Game.GUI.Utility
{
    public class UISafeArea : MonoBehaviour
    {

        public Canvas canvas;
        private RectTransform panelSafeArea;
        private ScreenOrientation currentOrientation = ScreenOrientation.LandscapeLeft;

        private Rect currentSaveArea = new Rect();

        void Start()
        {
            panelSafeArea = GetComponent<RectTransform>();
            currentOrientation = Screen.orientation;
            currentSaveArea = Screen.safeArea;
        
            ApplySafeArea();
        }
    

        void ApplySafeArea()
        {
            if (panelSafeArea == null)
                return;


            Rect safeArea = Screen.safeArea;

            Vector2 minAnchor = safeArea.position;
            Vector2 maxAnchor = safeArea.position+safeArea.size;

            minAnchor.x /= canvas.pixelRect.width;
            minAnchor.y /= canvas.pixelRect.height;
        
            maxAnchor.x /= canvas.pixelRect.width;
            maxAnchor.y /= canvas.pixelRect.height;
            // Debug.Log("Screen Size: "+Screen.width+" "+Screen.height);
            // Debug.Log("Canvas pixelRect: "+canvas.pixelRect.width+" "+canvas.pixelRect.height);
            // Debug.Log("Save Area Position: "+safeArea.position.x+" "+safeArea.position.y);
            // Debug.Log("Save Area Size: "+safeArea.size.x+" "+safeArea.size.y);
            panelSafeArea.anchorMin = minAnchor;
            panelSafeArea.anchorMax = maxAnchor;

            currentOrientation = Screen.orientation;
            currentSaveArea = Screen.safeArea;
        }

        private void Update()
        {
            if (currentOrientation != Screen.orientation || currentSaveArea != Screen.safeArea)
            {
                ApplySafeArea();
            }
        }
    }
}
