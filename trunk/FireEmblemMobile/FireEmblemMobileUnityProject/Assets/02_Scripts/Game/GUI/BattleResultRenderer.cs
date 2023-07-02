using System;
using System.Collections;
using System.Collections.Generic;
using Game.AI;
using Game.GameActors.Players;
using Game.GUI;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace LostGrace
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
        // [SerializeField] private Canvas canvas;
        // [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField]private BattleResult result;
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
        private void OnEnable()
        {
            Show(result);
        }

        public void Show(BattleResult result)
        {

            this.result = result;
            // canvas.enabled = true;
           // TweenUtility.FadeIn(canvasGroup);
           goldBonusesList = new List<Bonuses>();
           goldBonusesList.Add(new Bonuses(result.GetVictoryGold(),"Victory", 1));
           goldBonusesList.Add(new Bonuses(result.GetGoldFromTurnCount(),"Turn Bonus", 1));
           graceBonusesList = new List<Bonuses>();
           graceBonusesList.Add(new Bonuses(result.GetVictoryGrace(),"Victory", 1));
           graceBonusesList.Add(new Bonuses(result.GetGraceFromTurnCount(),"Turn Bonus", 1));
           itemBonusesList = new List<StockedItem>();
           itemBonusesList.AddRange(result.GetItemBonuses());
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
            foreach (var stockedItem in itemBonusesList)
            {
                var go = Instantiate(itemRewardGO, itemRewardParent);
                go.GetComponent<UIConvoyItemController>().SetValues(stockedItem);
                yield return new WaitForSeconds(0.5f);
            }
        }
      
        
        public void Hide()
        {
            Debug.Log("HIDE RESULT PANNEL");
            // TweenUtility.FadeOut(canvasGroup).setOnComplete(() =>
            // {
            //     Debug.Log("FADEOUT FINISHED");
            //     canvas.enabled = false;
            //     OnFinished?.Invoke();
            // });
        }
    
    }
}
