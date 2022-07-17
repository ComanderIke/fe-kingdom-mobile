using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryButtonController : MonoBehaviour
{
    public Sprite SecretSprite;

    public Image image;

    public Sprite itemSprite;

    public MemoryController MemoryController;

    public Button button;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private bool revealed = false;
    

    void TurnAnimation()
    {
        if (!revealed)
        {
            revealed = true;
            LeanTween.rotateY(gameObject, 90f, .4f).setEaseInQuad().setOnComplete(() =>
            {
                image.sprite = itemSprite;
                LeanTween.rotateY(gameObject, 180f, .4f).setEaseOutBounce();
                if (MemoryController.TurnBack(this))
                {
                    MonoUtility.DelayFunction(TurnBack,1.0f);
                }
            });
           
        }
        else
        {
            TurnBack();
        }
    }

    public void OnCllick()
    {
        TurnAnimation();
    }

    public void TurnBack()
    {
        revealed = false;
        LeanTween.rotateY(gameObject, 90f, .4f).setEaseInQuad().setOnComplete(() =>
        {
            image.sprite = SecretSprite;
            LeanTween.rotateY(gameObject, 0f, .4f).setEaseOutQuad();
        });
    }

    public void SetInActive()
    {
        button.interactable=false;
    }
    public void SetActive()
    {
        button.interactable=true;
    }
}