using Game.GUI;
using Game.GUI.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.EncounterAreas.Controller
{
    public class EncounterCursorController : MonoBehaviour
    {
        public float scaleTime = 0.8f;
        public float scaleFactor = 1.08f;
        [SerializeField] private Image image;
        [SerializeField] private Image fillImage;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private UIWhiteRadialFillBar uiFillBar;
        private LTDescr fadeTween = null;

        private bool visible = true;
        // Start is called before the first frame update
        void Start()
        {
            LeanTween.scale(gameObject, new Vector3(scaleFactor, scaleFactor,scaleFactor), scaleTime).setEaseInOutQuad().setLoopType(LeanTweenType.pingPong);
        }

        // Update is called once per frame
        void Update()
        {
            //transform.RotateAround(Vector3.forward, Time.deltaTime*rotationSpeed);
       
        }

        public void DecreaseFill()
        {
            if (!visible)
                return;
            fillImage.fillAmount += 2*Time.deltaTime;
       
            uiFillBar.AddFill(-Time.deltaTime*2);
            if (fillImage.fillAmount >= 1)
                fillImage.fillAmount = 1;
        
        }
        public void SetFill(float fill)
        {
            if (!visible)
                return;
            fillImage.fillAmount = fill;
            var specialFillAmount =0.5f-( (1 + (fill - 1)) / 2);

            uiFillBar.SetFill(specialFillAmount);
   
        }
        public void SetScale(float scale)
        {
            if (!visible)
                return;
            image.transform.localScale = new Vector3(scale, scale, scale);
            fillImage.transform.localScale = new Vector3(scale, scale, scale);
        }
        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
            fillImage.sprite = sprite;
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
        public void SetPosition(Vector3 transformPosition)
        {
            transform.position = transformPosition;
        }

   
        public void Show()
        {

            visible = true;
      
            if(fadeTween!=null)
                LeanTween.cancel(fadeTween.uniqueId);
            fadeTween=TweenUtility.FadeIn(canvasGroup);
        }
        public void Hide()
        { 
        
            SetFill(1);
            visible = false;
            if(fadeTween!=null)
                LeanTween.cancel(fadeTween.uniqueId);
            fadeTween=TweenUtility.FadeOut(canvasGroup);
        }
    }
}
