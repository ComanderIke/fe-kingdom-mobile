using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterCursorController : MonoBehaviour
{
    public float scaleTime = 0.8f;
    public float scaleFactor = 1.08f;
    [SerializeField] private Image image;
    [SerializeField] private Image fillImage;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image uiFillBar;
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
       
        uiFillBar.fillAmount -= Time.deltaTime*2;
        if (fillImage.fillAmount >= 1)
            fillImage.fillAmount = 1;
        if (uiFillBar.fillAmount <= 0)
            uiFillBar.fillAmount = 0;
    }
    public void SetFill(float fill)
    {
        if (!visible)
            return;
        fillImage.fillAmount = fill;
        var specialFillAmount =0.5f-( (1 + (fill - 1)) / 2);
     
        uiFillBar.fillAmount = specialFillAmount;
    }
    public void SetScale(float scale)
    {
        if (!visible)
            return;
        image.transform.localScale = new Vector3(scale, scale, scale);
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
