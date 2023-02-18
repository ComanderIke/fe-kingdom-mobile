using System.Collections;
using System.Collections.Generic;
using _02_Scripts.Game.Dialog.DialogSystem;
using TMPro;
using UnityEngine;

public enum TextOptionState
{
    Normal,
    High,
    Secret,
    Impossible
}
public class TextOptionController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI StatPreview;
    private UIEventController controller;
    public Color textNormalColor;
    public Color textSecretColor;
    public Color textNotPossibleColor;
    public Color textHighColor;
    public LGDialogChoiceData Option;
    [SerializeField] private float delay = 0.25f;
    [SerializeField] private Animator animator;
    
    public void SetIndex(int index)
    {
        gameObject.SetActive(true);
        animator.enabled = false;
        MonoUtility.DelayFunction(()=>animator.enabled=true,delay*index);
    }

    public void Setup(LGDialogChoiceData option,string text,string statText,TextOptionState textState, UIEventController controller)
    {
        Option = option;
        this.controller = controller;
        this.text.text = text;
        this.text.color = textNormalColor;
        this.StatPreview.text = "("+statText+")";
        switch (textState)
        {
            case TextOptionState.Impossible:  this.StatPreview.color = textNotPossibleColor;break;
            case TextOptionState.Secret:  this.StatPreview.color = textSecretColor;break;
            case TextOptionState.Normal:  this.StatPreview.color = textNormalColor;break;
            case TextOptionState.High:  this.StatPreview.color = textHighColor;break;
        }
       

    }
    public void Setup(LGDialogChoiceData option,string text, UIEventController controller)
    {
        Option = option;
        this.controller = controller;
        this.text.text = text;
        this.text.color = textNormalColor;
        this.StatPreview.text = "";
    }

    public void Clicked()
    {
        controller.OptionClicked(this);
    }
}
