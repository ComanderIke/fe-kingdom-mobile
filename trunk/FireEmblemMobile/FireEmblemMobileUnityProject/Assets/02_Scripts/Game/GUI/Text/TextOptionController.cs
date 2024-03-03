using System;
using Febucci.UI;
using Game.Dialog.DialogSystem;
using Game.GameActors.Items;
using Game.GameActors.Player;
using Game.GUI.EncounterUI.Event;
using Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Text
{
    public enum TextOptionState
    {
        Normal,
        High,
        Secret,
        SecretHidden,
        Impossible,
        Lowish,
        Low,
        Locked,
        Evil,
        Good
    }
    public class TextOptionController : MonoBehaviour
    {
        public event Action onTextAppeared;
        public TextMeshProUGUI text;
        public TextMeshProUGUI StatPreview;
        [SerializeField] private TextSizer textSizer;
        private UIEventController controller;
        public Color textNormalColor;
        public Color textSecretColor;
        public Color textGoodColor;
        public Color textEvilColor;
        public Color textNotPossibleColor;
        public Color textLowColor;
        public Color textLowishColor;
        public Color textHighColor;
        [SerializeField] private TMP_ColorGradient useColor;
        [SerializeField] private TMP_ColorGradient notEnoughColor;
        public LGDialogChoiceData Option;
        [SerializeField] private float delay = 0.25f;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Animator animator;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI healText;
        [SerializeField] private Image rollImage;
        [SerializeField] private Image healImage;
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private Image damageImage;
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
        [SerializeField] private TextMeshProUGUI failResourceText;
        [SerializeField] private Image failResImage;
        [SerializeField] private TextMeshProUGUI receiveResourceText;
        [SerializeField] private Image receiveResImage;
        [SerializeField] private Sprite goldSprite;
        [SerializeField] private Sprite supplySprite;
        [SerializeField] private Sprite graceSprite;
        [SerializeField] private Sprite hpSprite;
        [SerializeField] private Sprite damageSprite;
        [SerializeField] private Sprite expSprite;
        private bool locked = false;
        public void SetIndex(int index)
        {
            gameObject.SetActive(true);
            animator.enabled = false;
            if (!locked)
            {
            
                MonoUtility.DelayFunction(() =>
                {
                    if (animator != null)
                        animator.enabled = true;
                }, delay * index);
            }
        }

        public void Setup(LGDialogChoiceData option,string text,string statText,TextOptionState textState, UIEventController controller)
        {
            Option = option;
            this.controller = controller;
            this.text.SetText(text);
            this.text.color = textNormalColor;
            this.StatPreview.SetText(String.IsNullOrEmpty(statText)?"": "("+statText+")");
            canvasGroup.alpha = 1;
            locked = false;
            button.interactable = true;
            animator.enabled = true;
            switch (textState)
            {
                case TextOptionState.Impossible:  this.StatPreview.color = textNotPossibleColor;break;
                case TextOptionState.Secret:  this.text.color = textSecretColor;break;
                case TextOptionState.Evil:  this.text.color = textEvilColor;break;
                case TextOptionState.Good:  this.text.color = textGoodColor;break;
                case TextOptionState.Normal:  this.StatPreview.color = textNormalColor;break;
                case TextOptionState.High:  this.StatPreview.color = textHighColor;break;
                case TextOptionState.Low:  this.StatPreview.color = textLowColor;break;
                case TextOptionState.Lowish:  this.StatPreview.color = textLowishColor;break;
                case TextOptionState.Locked:
                    canvasGroup.alpha = 0.45f;
                    button.interactable = false; 
                    animator.enabled = false;
                    locked = true;
                    break;
            }
            //
            if (option.NextDialogueFail != null && Player.Instance.DialogOptionsExperienced.Contains(option))
            {
                var failOption = (LGEventDialogSO)option.NextDialogueFail;
                failResourceText.gameObject.SetActive(true);
                var item = failOption.RewardResources[0];
                failResImage.sprite =GetSpriteFromResourceType(item.ResourceType, item.Amount);
                //rollImage.gameObject.SetActive(true);
            
            }
            // if (option.CharacterRequirements != null&& option.CharacterRequirements.Count>0)
            // {
            //     this.text.color = Color.blue;
            //     // charText.gameObject.SetActive(true);
            //     // charText.text = option.CharacterRequirement.Name== Player.Instance.Party.ActiveUnit.Name?option.CharacterRequirement.Name:"switch to "+option.CharacterRequirement.Name;
            //     //charImage.sprite = option.CharacterRequirement.visuals.CharacterSpriteSet.FaceSprite;
            // }
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
                    bool canAfford = Player.Instance.Party.CanAfford(item.Create());
                    useItemText.colorGradientPreset = canAfford ? useColor : notEnoughColor;
                }
            }
            if (test !=null&&test.RewardItems != null&& test.RewardItems.Count>0)
            {
                var item = test.RewardItems[0];
                if (item != null)
                {
                    receiveItemText.gameObject.SetActive(true);
                    //receiveItemText.text = item.name;
                    receiveItemImage.sprite = item.sprite;
                
                }
            }
            if (option.ResourceRequirements != null&& option.ResourceRequirements.Count>0)
            {
                var item = option.ResourceRequirements[0];
                if (item != null&& item.ResourceType!=ResourceType.Morality)
                {
                    useResourceText.gameObject.SetActive(true);
                    useResourceText.text = "use " + item.Amount;
                    useResImage.sprite = GetSpriteFromResourceType(item.ResourceType, item.Amount);
                    bool canAfford = Player.Instance.Party.CanAfford(item.ResourceType, item.Amount);
                    useResourceText.colorGradientPreset = canAfford ? useColor : notEnoughColor;
                }
            }
            if (test !=null&&test.RewardResources != null&& test.RewardResources.Count>0)
            {
                foreach (var reward in test.RewardResources)
                {
                    var item = reward;
                    if (Player.Instance.DialogOptionsExperienced.Contains(option)&&item != null && item.ResourceType != ResourceType.Morality &&
                        item.ResourceType !=
                        ResourceType
                            .HP_Percent) //if (Player.Instance.DialogOptionsExperienced.Contains(option)&&item != null&& item.ResourceType!=ResourceType.Morality)
                    {
                        receiveResourceText.gameObject.SetActive(true);
                  
                        receiveResourceText.text = "receive"; //+ item.Amount;
                        if(option.NextDialogueFail!=null)
                            receiveResourceText.text = "Success"; 
                        receiveResImage.sprite = GetSpriteFromResourceType(item.ResourceType, item.Amount);
                    }
                    else if (Player.Instance.DialogOptionsExperienced.Contains(option)&&item != null && item.ResourceType == ResourceType.HP_Percent)
                    {
                        if (item.Amount > 0)
                        {
                            healText.gameObject.SetActive(true);
                            healText.text = "";
                        }
                        else
                        {
                            damageText.gameObject.SetActive(true);
                            damageText.text = "";
                        }

                    }
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
        Sprite GetSpriteFromResourceType(ResourceType type, int amount)
        {
            switch (type)
            {
                case ResourceType.Gold:
                    return goldSprite; break;
                case ResourceType.Supplies:
                    return supplySprite;
                    break;
                case ResourceType.Grace:
                    return graceSprite; break;
                case ResourceType.Exp:
                    return expSprite; break;
                case ResourceType.HP_Percent:
                    if(amount>0)
                        return hpSprite;
                    return damageSprite; break;
    
                default: return goldSprite;
                    break;
            }
        }

        public void Clicked()
        {
            if( !Player.Instance.DialogOptionsExperienced.Contains(Option))
                Player.Instance.DialogOptionsExperienced.Add(Option);
            controller.OptionClicked(this);
        }

        [SerializeField] private TypewriterByCharacter typeWriter;
        public void OnTextAppeared()
        {
            onTextAppeared?.Invoke();
        }
   
    }
}