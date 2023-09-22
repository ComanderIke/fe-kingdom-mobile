using System;
using System.Collections;
using System.Collections.Generic;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Items;
using Game.GUI.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TextOptionState
{
    Normal,
    High,
    Secret,
    Impossible,
    Lowish,
    Low,
    Locked
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
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Animator animator;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI healtext;
    [SerializeField] private TextMeshProUGUI charText;
    [SerializeField] private Image charImage;
    [SerializeField] private TextMeshProUGUI blessingText;
    [SerializeField] private Image blessingImage;
    [SerializeField] private TextMeshProUGUI useItemText;
    [SerializeField] private Image useItemImage;
    [SerializeField] private TextMeshProUGUI receiveItemText;
    [SerializeField] private Image receiveItemImage;
    [SerializeField] private TextMeshProUGUI useResourceText;
    [SerializeField] private Image useResImage;
    [SerializeField] private TextMeshProUGUI receiveResourceText;
    [SerializeField] private Image receiveResImage;
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite graceSprite;
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
        canvasGroup.alpha = 1;
        button.interactable = true;
        animator.enabled = true;
        switch (textState)
        {
            case TextOptionState.Impossible:  this.StatPreview.color = textNotPossibleColor;break;
            case TextOptionState.Secret:  this.StatPreview.color = textSecretColor;break;
            case TextOptionState.Normal:  this.StatPreview.color = textNormalColor;break;
            case TextOptionState.High:  this.StatPreview.color = textHighColor;break;
            case TextOptionState.Low:  this.StatPreview.color = textLowColor;break;
            case TextOptionState.Lowish:  this.StatPreview.color = textLowishColor;break;
            case TextOptionState.Locked:
                canvasGroup.alpha = 0.45f;
                button.interactable = false; 
                animator.enabled = false;break;
        }

        if (option.CharacterRequirement != null)
        {
            charText.gameObject.SetActive(true);
            charText.text = option.CharacterRequirement.name;
            charImage.sprite = option.CharacterRequirement.visuals.CharacterSpriteSet.FaceSprite;
        }
        // if (option.BlessingRequirement != null)
        // {
        //     charText.gameObject.SetActive(true);
        //     charText.text = option.CharacterRequirement.name;
        //     charImage.sprite = option.CharacterRequirement.visuals.CharacterSpriteSet.FaceSprite;
        // }

        LGEventDialogSO test = (LGEventDialogSO)option.NextDialogue;
        
        if (option.ItemRequirements != null&& option.ItemRequirements.Count>0)
        {
            var item = option.ItemRequirements[0];
            if (item != null)
            {
                useItemText.gameObject.SetActive(true);
                useItemText.text = item.name;
                useItemImage.sprite = item.sprite;
            }
        }
        if (test.RewardItems != null&& test.RewardItems.Count>0)
        {
            var item = test.RewardItems[0];
            if (item != null)
            {
                receiveItemText.gameObject.SetActive(true);
                receiveItemText.text = item.name;
                receiveItemImage.sprite = item.sprite;
            }
        }
        if (option.ResourceRequirements != null&& option.ResourceRequirements.Count>0)
        {
            var item = option.ResourceRequirements[0];
            if (item != null)
            {
                useResourceText.gameObject.SetActive(true);
                useResourceText.text = "use " + item.Amount;
                useResImage.sprite = GetSpriteFromResourceType(item.ResourceType);
            }
        }
        if (test.RewardResources != null&& test.RewardResources.Count>0)
        {
            var item = test.RewardResources[0];
            if (item != null&& item.ResourceType!=ResourceType.Morality)
            {
                receiveResourceText.gameObject.SetActive(true);
                receiveResourceText.text = "receive " + item.Amount;
                receiveResImage.sprite = GetSpriteFromResourceType(item.ResourceType);
            }
        }
        this.text.enabled = false;
        textSizer.Refresh();
        MonoUtility.InvokeNextFrame(() =>//Because Textsize updates on the next update loop.
        {
            if(this.text!=null)
                this.text.enabled = true;
        });

    }
   Sprite GetSpriteFromResourceType(ResourceType type)
   {
       switch (type)
       {
           case ResourceType.Gold:
               return goldSprite; break;
           case ResourceType.Grace:
               return graceSprite; break;
           default: return goldSprite;
               break;
       }
   }

    public void Clicked()
    {
        controller.OptionClicked(this);
    }
}
