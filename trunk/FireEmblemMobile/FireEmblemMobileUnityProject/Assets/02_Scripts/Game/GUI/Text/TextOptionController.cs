using System;
using System.Collections;
using System.Collections.Generic;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GUI.Text;
using TMPro;
using UnityEngine;

public enum TextOptionState
{
    Normal,
    High,
    Secret,
    Impossible,
    Lowish,
    Low
}
public class TextOptionController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI StatPreview;
    [SerializeField] private TextSizer textSizer;
    private UIEventController controller;
    public Color textNormalColor;
    public Color textSecretColor;
    public Color textNotPossibleColor;
    public Color textLowColor;
    public Color textLowishColor;
    public Color textHighColor;
    public LGDialogChoiceData Option;
    [SerializeField] private float delay = 0.25f;
    [SerializeField] private Animator animator;
    
    public void SetIndex(int index)
    {
        gameObject.SetActive(true);
        animator.enabled = false;
        MonoUtility.DelayFunction(() =>
        {
            if (animator != null)
                animator.enabled = true;
        },delay*index);
    }

    public void Setup(LGDialogChoiceData option,string text,string statText,TextOptionState textState, UIEventController controller)
    {
        Option = option;
        this.controller = controller;
        this.text.SetText(text);
        this.text.color = textNormalColor;
        this.StatPreview.SetText(String.IsNullOrEmpty(statText)?"": "("+statText+")");
        switch (textState)
        {
            case TextOptionState.Impossible:  this.StatPreview.color = textNotPossibleColor;break;
            case TextOptionState.Secret:  this.StatPreview.color = textSecretColor;break;
            case TextOptionState.Normal:  this.StatPreview.color = textNormalColor;break;
            case TextOptionState.High:  this.StatPreview.color = textHighColor;break;
            case TextOptionState.Low:  this.StatPreview.color = textLowColor;break;
            case TextOptionState.Lowish:  this.StatPreview.color = textLowishColor;break;
        }

        this.text.enabled = false;
        textSizer.Refresh();
        MonoUtility.InvokeNextFrame(() =>//Because Textsize updates on the next update loop.
        {
            if(this.text!=null)
                this.text.enabled = true;
        });

    }
   

    public void Clicked()
    {
        controller.OptionClicked(this);
    }
}
