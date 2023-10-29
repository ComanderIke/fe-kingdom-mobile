using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIAchievementScreen : UIMenu
    {
        private List<LG_Achievement> achievements;
        [SerializeField]private GameObject achievementPrefab;
        [SerializeField] private Transform achievementContainer;
        public override void Show()
        {
            StartCoroutine(ShowCoroutine());
            AddAchievements("All");
        }
      
        private void AddAchievements(string Filter)
        {  
            achievementContainer.DeleteChildren();
            AchievementManager AM = AchievementManager.instance;
            // int AchievedCount = AM.GetAchievedCount();

            // CountText.text = "" + AchievedCount + " / " + AM.States.Count;
            // CompleteText.text = "Complete (" + AM.GetAchievedPercentage() + "%)";

            for (int i = 0; i < AM.AchievementList.Count; i ++)
            {
                if((Filter.Equals("All")) || (Filter.Equals("Achieved") && AM.States[i].Achieved) || (Filter.Equals("Unachieved") && !AM.States[i].Achieved))
                {
                    AddAchievementToUI(AM.AchievementList[i], AM.States[i]);
                }
            }
           // Scrollbar.value = 1;
        }

        public void AddAchievementToUI(AchievementInfromation Achievement, AchievementState State)
        {
            UIAchievement UIAchievement = Instantiate(achievementPrefab, achievementContainer).GetComponent<UIAchievement>();
            UIAchievement.Set(Achievement, State);
        }
        /// <summary>
        /// Filter out a set of locked or unlocked achievements
        /// </summary>
        public void ChangeFilter (string filter)
        {
            AddAchievements(filter);
        }
        IEnumerator ShowCoroutine()
        {
            base.Show();
            yield return null;
        }

        public override void BackClicked()
        {
            Hide();
            
        }
        IEnumerator HideCoroutine()
        {
            // TweenUtility.FadeOut(newGameButtonCanvasGroup);
            base.Hide();
      
            yield return null;
        }



        public override void Hide()
        {
            StartCoroutine(HideCoroutine());
        }
    }
}