
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
    Vector3 oldPosition;
    public TextMeshProUGUI text;
    public Image fill;
    public GameObject glow;
    int currentExp;
    int addedExp;
    float textspeed = 3;
    public float AnimateInDuration=0.25f;
    public float AnimateStayDuration = 0.45f;
    public float AnimateOutDuration = 0.2f;
    // Start is called before the first frame update
    public void Show(int currentExp, int addedExp)
    {
        this.currentExp = currentExp;
        this.addedExp = addedExp;
        text.text = ""+currentExp;

        //LeanTween.color(this.gameObject, Color.white, 0.1f).setEaseOutQuad();
        LeanTween.alphaCanvas(glow.GetComponent<CanvasGroup>(), 1, 1.0f).setLoopPingPong(1);
        LeanTween.scale(this.gameObject, new Vector3(1.4f, 1.4f, 1), AnimateInDuration).setDelay(0.25f).setEaseSpring().setOnComplete(ShowTextAnimation);
        //LeanTween.moveLocal(this.gameObject, new Vector3(0, -Screen.height/2+transform.parent.GetComponent<RectTransform>().rect.height/2, 0), AnimateInDuration).setEaseOutQuad();
        
        oldPosition = this.transform.localPosition;

    }
  
    void ShowTextAnimation()
    {
        int exp = currentExp;
        float fillAmount = 0;
        LeanTween.value(this.gameObject, 0, addedExp, Math.Max(addedExp / 100f*1.5f, 0.4f)).setEaseOutQuad().setOnUpdate((float val) =>
        {
            
            int intVal = (int)val;
            int expVal = exp + intVal;
            if (expVal > 100)
                expVal -= 100;
            text.text = "" + (expVal);
            fillAmount =  (expVal) /100f;
            fill.fillAmount = fillAmount;

        }).setOnComplete(Hide).setDelay(0.10f);
    }
    void Hide()
    {
        LeanTween.scale(this.gameObject, new Vector3(1, 1, 1), AnimateOutDuration).setEaseInQuad().setDelay(AnimateStayDuration);
        LeanTween.moveLocal(this.gameObject, oldPosition, AnimateOutDuration).setEaseInQuad().setDelay(AnimateStayDuration)
            .setOnComplete(() => { AnimationQueue.OnAnimationEnded?.Invoke(); });
    }

    public void SetText(int exp)
    {
        text.text = "" + (exp);
    }

    public void SetFillAmount(int expVal)
    {
        fill.fillAmount = (expVal) / 100f;
    }
}
