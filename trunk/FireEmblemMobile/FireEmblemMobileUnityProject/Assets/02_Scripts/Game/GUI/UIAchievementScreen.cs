using System.Collections;
using System.Collections.Generic;
using Game.AchievementSystem;
using Game.Menu;
using Game.Utility;
using UnityEngine;

namespace Game.GUI
{
    public class UIAchievementScreen : UIMenu
    {
        private List<LG_Achievement> achievements;
        [SerializeField]private GameObject achievementPrefab;
        [SerializeField] private Transform achievementContainer;
        private string currentFilter = "All";
        public override void Show()
        {
            StartCoroutine(ShowCoroutine());
            AchievementManager.instance.onAchievementUnlocked += UpdateUI;
            AchievementManager.instance.onStateChanged += UpdateUI;
            AddAchievements("All");
        }

        void UpdateUI()
        {
            AddAchievements(currentFilter);
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
                if(((Filter.Equals("All")) || (Filter.Equals("Achieved") && AM.States[i].Achieved)
                                           || (Filter.Equals("Unachieved") && !AM.States[i].Achieved))
                                           && !AM.AchievementList[i].Spoiler)
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
            currentFilter = filter;
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
            AchievementManager.instance.onAchievementUnlocked -= UpdateUI;
            AchievementManager.instance.onStateChanged -= UpdateUI;
            yield return null;
        }



        public override void Hide()
        {
            StartCoroutine(HideCoroutine());
        }
    }
}