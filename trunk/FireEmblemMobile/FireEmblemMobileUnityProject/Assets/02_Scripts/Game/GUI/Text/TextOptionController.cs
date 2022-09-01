using System.Collections;
using System.Collections.Generic;
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
    public ResponseOption Option;

    public void Setup(ResponseOption option,string text,string statText,TextOptionState textState, UIEventController controller)
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

    public void Clicked()
    {
        controller.OptionClicked(this);
    }
}
