using System;
using System.Collections;
using System.Collections.Generic;
using Game.GUI.Controller;
using Game.GUI.Convoy;
using Game.GUI.EncounterUI.Merchant;
using Game.Utility;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace Game.GUI
{
    [ExecuteInEditMode]
    public class BattleResultRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject goldAddedLine;
        [SerializeField] private UIAnimatedCountingText goldAddedText;
        [SerializeField] private TextMeshProUGUI goldAddedLabel;
        [SerializeField] private GameObject graceAddedLine;
        [SerializeField] private UIAnimatedCountingText graceAddedText;
        [SerializeField] private TextMeshProUGUI graceAddedLabel;
        [SerializeField] private UIAnimatedCountingText totalGrace;
        [SerializeField] private UIAnimatedCountingText totalGold;
        [SerializeField] private UIAnimatedCountingText turnCount;
        [SerializeField] private MMF_Player showFeedback;
        [SerializeField] private MMF_Player goldFeedback;
        [SerializeField] private MMF_Player graceFeedback;
        [SerializeField] private MMF_Player itemFeedback;

        [SerializeField] private GameObject itemRewardGO;

        [SerializeField] private Transform itemRewardParent;
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField]private BattleResult result;
        [SerializeField] private TMP_ColorGradient bonusBlue;
        [SerializeField] private TMP_ColorGradient bonusRed;
        [SerializeField] private TMP_ColorGradient normal;
        struct Bonuses
        {
            public string description;
            public int goldAmount;
            public int count;

            public Bonuses(int goldAmount, string description, int count)
            {
                this.goldAmount = goldAmount;
                this.description = description;
                this.count = count;
            }
        }
        public event Action OnFinished;
        private List<Bonuses> goldBonusesList;
        private List<Bonuses> graceBonusesList;
        private List<StockedItem> itemBonusesList;
        // private void OnEnable()
        // {
        //     Show(result);
        // }

        enum Efficiency
        {
            Fast,
            Normal,
            Slow
        }
        public void Show(BattleResult result)
        {

            BottomUI.Hide();
            itemRewardParent.DeleteChildren();
            finished = false;
            this.result = result;
            canvas.enabled = true;
            TweenUtility.FadeIn(canvasGroup);
            goldBonusesList = new List<Bonuses>();
            Efficiency efficiency = Efficiency.Normal;
            turnCount.SetTextCounting(0,result.GetTurnCount(), false);
            efficiency= result.GetTurnCount() < result.GetMaxTurnCount()? Efficiency.Fast:result.GetTurnCount()> result.GetMaxTurnCount()?Efficiency.Slow:Efficiency.Normal;
            turnCount.GetComponent<TextMeshProUGUI>().colorGradientPreset = efficiency == Efficiency.Fast ? bonusBlue: efficiency==Efficiency.Slow? bonusRed:normal;
            goldBonusesList.Add(new Bonuses(result.GetVictoryGold(),"Victory", 1));
            goldBonusesList.Add(new Bonuses(result.GetGoldFromTurnCount(),"Turn Bonus", 1));
            graceBonusesList = new List<Bonuses>();
            graceBonusesList.Add(new Bonuses(result.GetVictoryGrace(),"Victory", 1));
            graceBonusesList.Add(new Bonuses(result.GetGraceFromTurnCount(),"Turn Bonus", 1));
            itemBonusesList = new List<StockedItem>();
            itemBonusesList.AddRange(result.GenerateItemBonuses());
            showFeedback.PlayFeedbacks();
          
            // if (result.GetTurnCount() != 0)
            // {
            //     var lineGO=Instantiate(rewardLinePrefab, rewardLineParent);
            //     var rewardLine = lineGO.GetComponent<RewardLineUI>();
            //     rewardLine.SetValues("Turn Count", result.GetGoldFromTurnCount(), result.GetExpFromTurnCount(), result.GetTurnCount(), null);
            // }
            //
            //
            // totalGrace.text = ""+result.GetTotalBexp();
            // totalGold.text = ""+result.GetTotalGold();


        }

        public void ShowGoldBonuses()
        {
            Debug.Log("Show Gold Bonuses: "+goldBonusesList.Count);
            if (goldBonusesList.Count != 0)
            {
                goldAddedLabel.SetText(goldBonusesList[0].description);
                goldAddedText.SetText(""+goldBonusesList[0].goldAmount);
                goldAddedText.SetTextCounting(goldBonusesList[0].goldAmount, 0,false);
                totalGold.SetTextCounting(totalGold.GetCurrentAmount(),totalGold.GetCurrentAmount()+goldBonusesList[0].goldAmount,false);
                goldFeedback.StopFeedbacks();
                goldFeedback.PlayFeedbacks();
                goldBonusesList.RemoveAt(0);
            }
            else
            {
                
                ShowGraceBonuses();
            }
           
        }
        public void ShowGraceBonuses()
        {
            Debug.Log("Show Gold Bonuses: "+goldBonusesList.Count);
            if (graceBonusesList.Count != 0)
            {
                graceAddedLabel.SetText(graceBonusesList[0].description);
                graceAddedText.SetText(""+graceBonusesList[0].goldAmount);
                graceAddedText.SetTextCounting(graceBonusesList[0].goldAmount, 0,false);
                totalGrace.SetTextCounting(totalGrace.GetCurrentAmount(),totalGrace.GetCurrentAmount()+graceBonusesList[0].goldAmount,false);
                graceFeedback.StopFeedbacks();
                graceFeedback.PlayFeedbacks();
                graceBonusesList.RemoveAt(0);
            }
            else
            {
                itemFeedback.PlayFeedbacks();
                //ShowItems();
            }
        }
        public void ShowItems()
        {
            StartCoroutine(ItemCoroutine());
        }

        IEnumerator ItemCoroutine()
        {
            int index = 0;
            foreach (var stockedItem in itemBonusesList)
            {
                var go = Instantiate(itemRewardGO, itemRewardParent);
                go.GetComponent<UIConvoyItemController>().SetValues(stockedItem,index);
                index++;
                yield return new WaitForSeconds(0.5f);
            }

            finished = true;
        }

        private bool finished = false;

        public void Hide()
        {
            Debug.Log("HIDE RESULT PANNEL");
             TweenUtility.FadeOut(canvasGroup).setOnComplete(() =>
            {
                Debug.Log("FADEOUT FINISHED");
                canvas.enabled = false;
                OnFinished?.Invoke();
            });
        }
    
    }
}
