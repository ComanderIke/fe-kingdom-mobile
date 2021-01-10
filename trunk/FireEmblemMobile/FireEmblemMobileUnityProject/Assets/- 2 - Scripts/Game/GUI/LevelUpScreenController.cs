using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utility;

namespace Game.GUI
{
    public class LevelUpScreenController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelUpText;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI strText;
        [SerializeField] private TextMeshProUGUI spdText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI spText;
        [SerializeField] private TextMeshProUGUI defText;
        [SerializeField] private TextMeshProUGUI resText;
        [SerializeField] private TextMeshProUGUI magText;
        [SerializeField] private TextMeshProUGUI sklText;
        [SerializeField] private TextMeshProUGUI strAddedText;
        [SerializeField] private TextMeshProUGUI spdAddedText;
        [SerializeField] private TextMeshProUGUI hpAddedText;
        [SerializeField] private TextMeshProUGUI spAddedText;
        [SerializeField] private TextMeshProUGUI defAddedText;
        [SerializeField] private TextMeshProUGUI resAddedText;
        [SerializeField] private TextMeshProUGUI magAddedText;
        [SerializeField] private TextMeshProUGUI sklAddedText;
        private float delaybetweenPopups = 0.10f;
        List<Action> actions = new List<Action>();
        int actionIndex = 0;
        // Start is called before the first frame update
        public void Show(string name, int levelBefore, int levelAfter, int[] stats, int[] statsIncreases)
        {
            gameObject.SetActive(true);
        
            nameText.text = "" + name;
            levelText.text = "" + levelBefore;
            strText.text = "" + stats[2];
            spdText.text = "" + stats[5];
            defText.text = "" + stats[6];
            sklText.text = "" + stats[4]; 
            magText.text = "" + stats[3];
            hpText.text = "" + stats[0];
            spText.text = "" +stats[1];
            resText.text = "" + stats[7];

            strAddedText.text = "";
            spdAddedText.text = "";
            defAddedText.text = "";
            sklAddedText.text = "";
            magAddedText.text = "";
            hpAddedText.text = "";
            spAddedText.text = "";
            resAddedText.text = "";
            //for(int i=0; i < statsIncreases.Length; i++)
            //{
            //    Debug.Log(statsIncreases[i] + ", ");
            //}
            LeanTween.alphaCanvas(levelUpText.GetComponent<CanvasGroup>(), 1, 0.15f).setEaseOutQuad();
            LeanTween.scale(levelUpText.gameObject, Vector3.one, 0.15f).setEaseOutQuad();
            LeanTween.moveLocalY(levelUpText.gameObject, levelUpText.transform.localPosition.y + 100, 0.15f).setEaseOutQuad().setOnComplete(
                () =>
                {
                    LeanTween.alphaCanvas(levelUpText.GetComponent<CanvasGroup>(), 0, 0.15f).setEaseInQuad().setDelay(1.8f);
                    LeanTween.scale(levelUpText.gameObject, new Vector3(1, 0, 1), 0.15f).setEaseInQuad().setDelay(1.8f);
                    LeanTween.moveLocalY(levelUpText.gameObject, levelUpText.transform.localPosition.y - 100, 0.15f).setDelay(1.8f).setEaseInQuad().setOnComplete(() => {
                        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1, 0.55f).setEaseOutQuad();
                        LeanTween.scale(levelText.gameObject, levelText.transform.localScale * 1.2f, 0.8f).setEaseOutQuad().setDelay(0.65f).setOnStart(() => levelText.text = "" + levelAfter)
                            .setOnComplete(() =>
                            {
                                LeanTween.scale(levelText.gameObject, levelText.transform.localScale, 0.0f).setEaseInQuad().setDelay(delaybetweenPopups).setOnComplete(() =>
                                {
                                    if (actionIndex < actions.Count)
                                        actions[actionIndex].Invoke();
                                    actionIndex++;
                                });
                            });
                    });
                });

            if (statsIncreases[0] > 0) {
                actions.Add(CreateStatPopUpActionAnimationIn(hpText, "" + (stats[0] + statsIncreases[0]), hpAddedText, statsIncreases[0]));
                actions.Add(CreateStatPopUpActionAnimationOut(hpText, hpAddedText));
            }
            if (statsIncreases[2] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(strText, "" + (stats[2] +statsIncreases[2]), strAddedText, statsIncreases[2]));
                actions.Add(CreateStatPopUpActionAnimationOut(strText, strAddedText));
            }
            if (statsIncreases[3] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(magText, "" + (stats[3] + statsIncreases[3]), magAddedText, statsIncreases[3]));
                actions.Add(CreateStatPopUpActionAnimationOut(magText, magAddedText));
            }
            if (statsIncreases[5] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(sklText, "" + (stats[5] +statsIncreases[5]), sklAddedText, statsIncreases[5]));
                actions.Add(CreateStatPopUpActionAnimationOut(sklText, sklAddedText));
            }
            if (statsIncreases[1] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(spText, "" + (stats[1] +statsIncreases[1]), spAddedText, statsIncreases[1]));
                actions.Add(CreateStatPopUpActionAnimationOut(spText, spAddedText));
            }
            if (statsIncreases[4] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(spdText, "" + (stats[4] + statsIncreases[4]), spdAddedText, statsIncreases[4]));
                actions.Add(CreateStatPopUpActionAnimationOut(spdText, spdAddedText));
            }
            if (statsIncreases[6] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(defText, "" + (stats[6] + statsIncreases[6]), defAddedText, statsIncreases[6]));
                actions.Add(CreateStatPopUpActionAnimationOut(defText, defAddedText));
            }
            if (statsIncreases[7] > 0)
            {
                actions.Add(CreateStatPopUpActionAnimationIn(resText, "" + (stats[7] + statsIncreases[7]), resAddedText, statsIncreases[7]));
                actions.Add(CreateStatPopUpActionAnimationOut(resText, resAddedText));
            }
            actions.Add(()=> LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, 0.65f).setEaseInQuad().setDelay(delaybetweenPopups)
                .setOnComplete(()=>AnimationQueue.OnAnimationEnded?.Invoke()));

        }
        private Action CreateStatPopUpActionAnimationIn(TextMeshProUGUI textobject, string text, TextMeshProUGUI textAddedobject, int statIncrease)
        {
            return () => {
                LeanTween.scale(textobject.gameObject, textobject.transform.localScale * 1.0f, 0.20f).setEaseOutQuad().setDelay(delaybetweenPopups)
                    .setOnComplete(() =>
                    {
                        textobject.text = text;
                        if (actionIndex < actions.Count)
                            actions[actionIndex].Invoke();
                        actionIndex++;
                    });
                textAddedobject.text = "+"+statIncrease;
                LeanTween.alphaCanvas(textAddedobject.GetComponent<CanvasGroup>(), 1, 0.20f).setEaseOutQuad().setDelay(delaybetweenPopups);
                LeanTween.scale(textAddedobject.gameObject, Vector3.one, 0.20f).setEaseOutQuad().setDelay(delaybetweenPopups);
            };
        }
        private Action CreateStatPopUpActionAnimationOut(TextMeshProUGUI textobject, TextMeshProUGUI textAddedobject)
        {
            return () => {
                LeanTween.scale(textobject.gameObject, textobject.transform.localScale, 0.0f).setEaseInQuad().setDelay(delaybetweenPopups)
                    .setOnComplete(() =>
                    {
                        if (actionIndex < actions.Count)
                            actions[actionIndex].Invoke();
                        actionIndex++;
                    });
                //LeanTween.alphaCanvas(textAddedobject.GetComponent<CanvasGroup>(), 0, 0.3f).setEaseInQuad().setDelay(delaybetweenPopups);
            };
        }
    }
}
