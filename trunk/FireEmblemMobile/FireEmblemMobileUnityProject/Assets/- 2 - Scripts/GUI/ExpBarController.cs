
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
    Vector3 oldPosition;
    public TextMeshProUGUI text;
    public Image fill;
    int currentExp;
    int addedExp;
    float textspeed = 3;
    // Start is called before the first frame update
    public void Show(int currentExp, int addedExp)
    {
        this.currentExp = currentExp;
        this.addedExp = addedExp;
        text.text = ""+currentExp;
        LeanTween.scale(this.gameObject, new Vector3(3, 3, 1), 0.6f).setEaseOutQuad().setOnComplete(ShowTextAnimation);
        LeanTween.move(this.gameObject, new Vector3(3, 4, this.transform.position.z), 0.6f).setEaseOutQuad();
        oldPosition = this.transform.position;
        
    }
    void ShowTextAnimation()
    {
        int exp = currentExp;
        float fillAmount = 0;
        LeanTween.value(this.gameObject, 0, addedExp, 1).setOnUpdate((float val) =>
        {
            
            int intVal = (int)val;
            int expVal = exp + intVal;
            if (expVal > 100)
                expVal -= 100;
            text.text = "" + (expVal);
            fillAmount =  (expVal) /100f;
            fill.fillAmount = fillAmount;

        }).setOnComplete(Hide).setDelay(0.15f);
    }
    void Hide()
    {
        LeanTween.scale(this.gameObject, new Vector3(0, 0, 0), 0.4f).setEaseOutQuad().setDelay(0.5f);
        LeanTween.move(this.gameObject, oldPosition, 0.4f).setEaseOutQuad().setDelay(0.5f)
            .setOnComplete(() => AnimationQueue.OnAnimationEnded?.Invoke());
    }
}
