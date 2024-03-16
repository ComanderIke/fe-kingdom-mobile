using Game.GameActors.Units;
using LostGrace;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Controller
{
    public class BattleCanvasController : MonoBehaviour
    {
        public RawImage image;
        public RawImage charImage;
        public CritCutInnController critCutinnController;

        public GameObject battleCanvas;
        private CanvasGroup battleCanvasGroup;
        public float fadeInTime = 1.2f;
        public float fadeOutTime = 0.25f;
        public RenderTexture RenderTexture;

        public Canvas canvas;
        public float fixedheight = 440;
        private float width;
        private float height;
        private void Awake()
        {
        
        }

        private void Start()
        {
            battleCanvasGroup = battleCanvas.GetComponent<CanvasGroup>();
            RectTransform rawImageRect = image.GetComponent<RectTransform>();
            RectTransform rawCharImageRect = charImage.GetComponent<RectTransform>();
       
            width =canvas.pixelRect.width/canvas.scaleFactor;
            height = fixedheight; //canvas.pixelRect.height/canvas.scaleFactor;
      
            rawImageRect.sizeDelta = new Vector2(width, height);
            rawCharImageRect.sizeDelta = new Vector2(width, height);
            //RenderTexture.width =(int) width;
            //RenderTexture.height =(int)height;
        
        }
  

        public void Show()
        {
      
            GetComponent<Canvas>().enabled = true;
            RectTransform rectT = image.rectTransform;
            rectT.sizeDelta = new Vector2(rectT.sizeDelta.x, 0);
       
            LeanTween.size(rectT, new Vector2(width, height), fadeInTime).setEaseOutQuad();
            RectTransform rectTC = charImage.rectTransform;
            rectTC.sizeDelta = new Vector2(rectTC.sizeDelta.x, 0);
       
            LeanTween.size(rectTC, new Vector2(width, height), fadeInTime).setEaseOutQuad();
            battleCanvasGroup.alpha = 1;
            //LeanTween.scaleY(battleCanvas,1,1.2f).setEaseOutQuad();
            //LeanTween.alphaCanvas(battleCanvasGroup,.95f, fadeInTime).setEaseOutQuad();
        }

        public void Hide()
        {
            RectTransform rectTC = charImage.rectTransform;
            LeanTween.size(rectTC, new Vector2(rectTC.sizeDelta.x, 0), fadeOutTime).setEaseInQuad().setOnComplete(()=>GetComponent<Canvas>().enabled = false);

            RectTransform rectT = image.rectTransform;
            LeanTween.size(rectT, new Vector2(rectT.sizeDelta.x, 0), fadeOutTime).setEaseInQuad().setOnComplete(()=>GetComponent<Canvas>().enabled = false);
            // LeanTween.scaleY(battleCanvas,0,0.4f).setEaseInQuad();
            // LeanTween.alphaCanvas(battleCanvasGroup,0, fadeOutTime).setEaseInQuad();
       
        }

        public void ShowCritical(Unit unit)
        {
            critCutinnController.Show(unit);
        }
    }
}
