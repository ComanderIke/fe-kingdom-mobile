using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class LevelUpScreenController : MonoBehaviour, ILevelUpRenderer
    {

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image faceImage;
        [SerializeField] private TMP_ColorGradient textBaseColor;
        [SerializeField] private TMP_ColorGradient textIncreasedColor;
        [SerializeField] private TextMeshProUGUI conLabelText;
        [SerializeField] private TextMeshProUGUI agiLabelText;
        [SerializeField] private TextMeshProUGUI strLabelText;
        [SerializeField] private TextMeshProUGUI dexLabelText;
        [SerializeField] private TextMeshProUGUI intLabelText;
        [SerializeField] private TextMeshProUGUI fthLabelText;
        [SerializeField] private TextMeshProUGUI defLabelText;
        [SerializeField] private TextMeshProUGUI lckLabelText;
        [SerializeField] private TextMeshProUGUI conText;
        [SerializeField] private TextMeshProUGUI agiText;
        [SerializeField] private TextMeshProUGUI strText;
        [SerializeField] private TextMeshProUGUI dexText;
        [SerializeField] private TextMeshProUGUI intText;
        [SerializeField] private TextMeshProUGUI fthText;
        [SerializeField] private TextMeshProUGUI defText;
        [SerializeField] private TextMeshProUGUI lckText;

        [SerializeField] private TextMeshProUGUI strAddedText;
        [SerializeField] private TextMeshProUGUI intAddedText;
        [SerializeField] private TextMeshProUGUI defAddedText;
        [SerializeField] private TextMeshProUGUI lckAddedText;
        [SerializeField] private TextMeshProUGUI conAddedText;
        [SerializeField] private TextMeshProUGUI dexAddedText;
        [SerializeField] private TextMeshProUGUI agiAddedText;
        [SerializeField] private TextMeshProUGUI fthAddedText;
        [SerializeField] private CanvasGroup alphaCanvas;
        private float delaybetweenPopups = 0.13f;
        private float endDelay = 1.35f;
        List<Action> actions = new List<Action>();
        int actionIndex = 0;
        private int[] stats;
        private int[] statsIncreases;
        private int levelAfter;
        private Canvas canvas;
        [SerializeField] private Button RerollButton;
        [SerializeField] private TextMeshProUGUI rerollAmountText;

        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        public void OnContinueClicked()
        {
            LeanTween.alphaCanvas(alphaCanvas, 0, 0.65f).setEaseInQuad()
                .setOnComplete(()=>
                {
                    levelText.transform.localScale = Vector3.one;
                    MonoUtility.Instance.EnableOtherGraphicRaycasters(GetComponent<GraphicRaycaster>());
                    OnFinished?.Invoke();
                    canvas.enabled = false;
                });
        }

        void Reset()
        {
            actions.Clear();
            actionIndex = 0;
            LeanTween.cancel(gameObject);
            LeanTween.cancel(levelText.gameObject);
            LeanTween.cancel(alphaCanvas.gameObject);
            LeanTween.cancel(strText.gameObject);
            LeanTween.cancel(strAddedText.gameObject);
            LeanTween.cancel(strLabelText.gameObject);
            LeanTween.cancel(dexText.gameObject);
            LeanTween.cancel(dexAddedText.gameObject);
            LeanTween.cancel(dexLabelText.gameObject);
            LeanTween.cancel(intText.gameObject);
            LeanTween.cancel(intAddedText.gameObject);
            LeanTween.cancel(intLabelText.gameObject);
            LeanTween.cancel(agiText.gameObject);
            LeanTween.cancel(agiAddedText.gameObject);
            LeanTween.cancel(agiLabelText.gameObject);
            LeanTween.cancel(conText.gameObject);
            LeanTween.cancel(conAddedText.gameObject);
            LeanTween.cancel(conLabelText.gameObject);
            LeanTween.cancel(lckText.gameObject);
            LeanTween.cancel(lckAddedText.gameObject);
            LeanTween.cancel(lckLabelText.gameObject);
            LeanTween.cancel(defText.gameObject);
            LeanTween.cancel(defAddedText.gameObject);
            LeanTween.cancel(defLabelText.gameObject);
            LeanTween.cancel(fthText.gameObject);
            LeanTween.cancel(fthAddedText.gameObject);
            LeanTween.cancel(fthLabelText.gameObject);
            levelText.transform.localScale = new Vector3(1,1,1);
            ResetTextObject(strAddedText);
            ResetTextObject(intAddedText);
            ResetTextObject(dexAddedText);
            ResetTextObject(agiAddedText);
            ResetTextObject(defAddedText);
            ResetTextObject(lckAddedText);
            ResetTextObject(conAddedText);
            ResetTextObject(fthAddedText);
            strLabelText.colorGradientPreset = textBaseColor;
            intLabelText.colorGradientPreset = textBaseColor;
            dexLabelText.colorGradientPreset = textBaseColor;
            agiLabelText.colorGradientPreset = textBaseColor; 
            conLabelText.colorGradientPreset = textBaseColor;
            fthLabelText.colorGradientPreset = textBaseColor;
            defLabelText.colorGradientPreset = textBaseColor;
            lckLabelText.colorGradientPreset = textBaseColor;
            strText.colorGradientPreset = textBaseColor;
            intText.colorGradientPreset = textBaseColor;
            dexText.colorGradientPreset = textBaseColor;
            agiText.colorGradientPreset = textBaseColor; 
            conText.colorGradientPreset = textBaseColor;
            fthText.colorGradientPreset = textBaseColor;
            defText.colorGradientPreset = textBaseColor;
            lckText.colorGradientPreset = textBaseColor;
            strLabelText.text = "";
            intLabelText.text = "";
            dexLabelText.text = "";
            agiLabelText.text = "";
            conLabelText.text = "";
            fthLabelText.text = "";
            defLabelText.text = "";
            lckLabelText.text = "";
            strText.text = "";
            intText.text = "";
            dexText.text = "";
            agiText.text = "";
            conText.text = "";
            fthText.text = "";
            defText.text = "";
            lckText.text = "";
            strAddedText.text = "";
            intAddedText.text = "";
            dexAddedText.text = "";
            agiAddedText.text = "";
            conAddedText.text = "";
            fthAddedText.text = "";
            defAddedText.text = "";
            lckAddedText.text = "";
        }
        public void OnRerollClicked()
        {
            
            OnReroll.Invoke();
           
        }
        // Start is called before the first frame update
        public void Play()
        {

            MonoUtility.DelayFunction(() =>
            {
                 MonoUtility.Instance.DisableOtherGraphicRaycasters(GetComponent<GraphicRaycaster>());
            canvas.enabled = true;
           // LeanTween.alphaCanvas(levelUpText.GetComponent<CanvasGroup>(), 1, 0.15f).setEaseOutQuad();
           // LeanTween.scale(levelUpText.gameObject, Vector3.one, 0.15f).setEaseOutQuad();



           conLabelText.text = "</>"+Attributes.GetAsLongText(4);
           strLabelText.text =  "</>"+Attributes.GetAsLongText(0);
           dexLabelText.text =  "</>"+Attributes.GetAsLongText(1);
           intLabelText.text =  "</>"+Attributes.GetAsLongText(2);
           agiLabelText.text =  "</>"+Attributes.GetAsLongText(3);
           lckLabelText.text= "</>"+Attributes.GetAsLongText(5);
           fthLabelText.text =  "</>"+Attributes.GetAsLongText(7);
           defLabelText.text =  "</>"+Attributes.GetAsLongText(6);
           
           if (statsIncreases[4] > 0)
           {
               actions.Add(CreateStatPopUpActionAnimationIn(conLabelText, conText, "" + (stats[4] +statsIncreases[4]), conAddedText, statsIncreases[4]));
               actions.Add(CreateStatPopUpActionAnimationOut(conText));
           }
           if (statsIncreases[0] > 0) {
                actions.Add(CreateStatPopUpActionAnimationIn(strLabelText, strText, "" + (stats[0] + statsIncreases[0]), strAddedText, statsIncreases[0]));
                actions.Add(CreateStatPopUpActionAnimationOut(strText));
            }
            if (statsIncreases[1] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(dexLabelText,dexText, "" + (stats[1] +statsIncreases[1]), dexAddedText, statsIncreases[1]));
                actions.Add(CreateStatPopUpActionAnimationOut(dexText));
            }
            if (statsIncreases[2] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(intLabelText,intText, "" + (stats[2] + statsIncreases[2]), intAddedText, statsIncreases[2]));
                actions.Add(CreateStatPopUpActionAnimationOut(intText));
            }
            if (statsIncreases[3] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(agiLabelText,agiText, "" + (stats[3] +statsIncreases[3]), agiAddedText, statsIncreases[3]));
                actions.Add(CreateStatPopUpActionAnimationOut(agiText));
            }
           
            if (statsIncreases[5] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(lckLabelText, lckText, "" + (stats[5] + statsIncreases[5]), lckAddedText, statsIncreases[5]));
                actions.Add(CreateStatPopUpActionAnimationOut(lckText));
            }
            if (statsIncreases[6] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(defLabelText, defText, "" + (stats[6] + statsIncreases[6]), defAddedText, statsIncreases[6]));
                actions.Add(CreateStatPopUpActionAnimationOut(defText));
            }
            if (statsIncreases[7] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(fthLabelText, fthText, "" + (stats[7] + statsIncreases[7]), fthAddedText, statsIncreases[7]));
                actions.Add(CreateStatPopUpActionAnimationOut(fthText));
            }
            LeanTween.alphaCanvas(alphaCanvas, 1, 0.55f).setEaseOutQuad();
            LeanTween.scale(levelText.gameObject, levelText.transform.localScale * 1.2f, 0.5f).setEaseOutQuad().setDelay(0.65f).setOnStart(() => levelText.text = "Lv " + levelAfter)
                .setOnComplete(() =>
                {
                    LeanTween.scale(levelText.gameObject, levelText.transform.localScale, 0.0f).setEaseInQuad().setDelay(delaybetweenPopups).setOnComplete(() =>
                    {
                        if (actionIndex < actions.Count)
                            actions[actionIndex].Invoke();
                        actionIndex++;
                    });
                });
            },.11f);
           
        }
        public void UpdateValues(string name, Sprite sprite,int levelBefore, int levelAfter, int[] stats, int[] statsIncreases, int rerollAmount)
        {

            Reset();
            MonoUtility.DelayFunction(() =>
            {
                rerollAmountText.text = "" + rerollAmount+"(-1)";
                RerollButton.interactable = rerollAmount > 0;
                faceImage.sprite = sprite;
                this.statsIncreases = statsIncreases;
                this.stats = stats;//str, int ,dex,agi,con,fth
                this.levelAfter = levelAfter;
                nameText.text = "" + name;
                levelText.text = "Lv " + levelBefore;
                strText.text = "" + stats[0];
                dexText.text = "" + stats[1];
                intText.text = "" + stats[2];
                agiText.text = "" + stats[3]; 
                conText.text = "" + stats[4];
                lckText.text = "" + stats[5];
                defText.text = "" + stats[6];
                fthText.text = "" + stats[7];
          
                strAddedText.text = "";
                intAddedText.text = "";
                dexAddedText.text = "";
                agiAddedText.text = "";
                conAddedText.text = "";
                fthAddedText.text = "";
                defAddedText.text = "";
                lckAddedText.text = "";
            },.1f);
            
           
        }

        public event Action OnFinished;
        public event Action OnReroll;
        public void ResetForReroll()
        {
          // Reset();
        }

        private void ResetTextObject(TextMeshProUGUI text)
        {
            text.GetComponent<CanvasGroup>().alpha = 0;
        }

        private Action CreateStatPopUpActionAnimationIn(TMP_Text label, TMP_Text textObject, string text, TMP_Text textAddedObject, int statIncrease)
        {
            return () => {
                LeanTween.scale(textObject.gameObject, textObject.transform.localScale * 1.0f, 0.20f).setEaseOutQuad().setDelay(delaybetweenPopups)
                    .setOnComplete(() =>
                    {
                        textObject.text = "<bounce>"+text;
                        textObject.colorGradientPreset = textIncreasedColor;
                        label.text = "<bounce>"+label.text;
                        label.colorGradientPreset = textIncreasedColor;
                        if (actionIndex < actions.Count)
                            actions[actionIndex].Invoke();
                        actionIndex++;
                    });
                textAddedObject.text = "<bounce>+"+statIncrease;
                LeanTween.alphaCanvas(textAddedObject.GetComponent<CanvasGroup>(), 1, 0.20f).setEaseOutQuad().setDelay(delaybetweenPopups);
                LeanTween.scale(textAddedObject.gameObject, Vector3.one, 0.20f).setEaseOutQuad().setDelay(delaybetweenPopups);
            };
        }
        private Action CreateStatPopUpActionAnimationOut(TMP_Text textObject)
        {
            return () => {
                LeanTween.scale(textObject.gameObject, textObject.transform.localScale, 0.0f).setEaseInQuad().setDelay(delaybetweenPopups)
                    .setOnComplete(() =>
                    {
                        if (actionIndex < actions.Count)
                            actions[actionIndex].Invoke();
                        actionIndex++;
                    });
            };
        }

        
    }
}
