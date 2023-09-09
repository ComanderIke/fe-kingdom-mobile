using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Dialog;
using Game.GameActors.Players;
using Game.GameResources;
using Game.Menu;
using Game.Systems;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LostGrace
{
    public class HubMenu : UIMenu
    {
        [SerializeField] private UIMenu characterSelectMenu;
        [SerializeField] private UIMenu upgradeMenu;
      
        [Header("SaveLoadStuff: ")]

        private string[] saveFiles;
        private string lastestSaveFile;
   
       [Header("Canvas Groups: ")]
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private CanvasGroup newGameButtonCanvasGroup;
        [SerializeField] private CanvasGroup tutorialButtonCanvasGroup;
        [SerializeField] private CanvasGroup upgradeButtonCanvasGroup;
        [SerializeField] private CanvasGroup backButtonCanvasGroup;
        [SerializeField] private CanvasGroup Fade;
        [Header("Buttons: ")]
        [SerializeField] private Button newCampaignButton;
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button upgradesButton;
        [SerializeField] private Button backButton;

        [Header("Experimental: ")] [SerializeField]
        private TextMeshProUGUI newGameButtonText;
        [SerializeField] private GoddessUI goddessUI;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private Conversation introConversation;
        [SerializeField] private EnemyArmyData tutorialBattleData;
        public override void Show()
        {
            //ContinueButton.SetActive(SaveData.currentSaveData!=null);
            //SaveButton.SetActive(SaveData.currentSaveData!=null);
            //LoadButton.SetActive(GetLoadFiles()!=0);
            newGameButtonText.text=SaveGameManager.HasOngoingCampaignSaveData()?"Continue Campaign":"New Campaign";
            StartCoroutine(ShowCoroutine());
            newCampaignButton.interactable = false;
            upgradesButton.interactable = false;
            backButton.interactable = false;
            tutorialButton.interactable = false;

        }

       
        IEnumerator ShowCoroutine()
        {
            base.Show();
            //tutorialButtonCanvasGroup.alpha = 1;
            yield return new WaitForSeconds(.5f);
            TweenUtility.FadeIn(titleCanvasGroup);
            TweenUtility.FadeIn(newGameButtonCanvasGroup);
            TweenUtility.FadeIn(upgradeButtonCanvasGroup);
            TweenUtility.FadeIn(backButtonCanvasGroup);
            TweenUtility.FadeIn(tutorialButtonCanvasGroup);
            if (GameConfig.Instance.tutorialEnabled)
                yield return TutorialCoroutine();
            else
            {
                //tutorialButton.interactable = true;
                newCampaignButton.interactable = true;
                upgradesButton.interactable = true;
                backButton.interactable = true;
            }
        }

        IEnumerator TutorialCoroutine()
        {
            yield return new WaitForSeconds(.6f);
            goddessUI.Show();
            yield return new WaitForSeconds(4.5f);
            dialogueManager.ShowDialog(introConversation);
            dialogueManager.dialogEnd += IntroFinished;
        }

        void IntroFinished()
        {
            dialogueManager.dialogEnd -= IntroFinished;
            goddessUI.Hide();
           // tutorialButton.interactable = true;
            newCampaignButton.interactable = false;
            upgradesButton.interactable = false;
            backButton.interactable = false;
        }
        IEnumerator HideCoroutine(UIMenu nextMenu)
        {
           
            TweenUtility.FadeOut(upgradeButtonCanvasGroup);
            TweenUtility.FadeOut(newGameButtonCanvasGroup);
            TweenUtility.FadeOut(tutorialButtonCanvasGroup);
           
            yield return new WaitForSeconds(.4f);
            TweenUtility.FadeOut(titleCanvasGroup);
            TweenUtility.FadeOut(backButtonCanvasGroup);

            TweenUtility.FadeIn(Fade).setOnComplete(()=>
            {
                base.Hide();
                nextMenu?.Show();
                TweenUtility.FadeOut(Fade);
            });
        }
        public override void Hide()
        {
            StartCoroutine(HideCoroutine(parent));
        }
        public override void BackClicked()
        {
            Hide();
            
        }
        public void ContinueClicked()
        {
            Hide();
            //LoadScene(Scenes.Campaign1);
            //LoadGame(lastestSaveFile);
        }
        public void NewGameClicked()
        {
            if (SaveGameManager.HasOngoingCampaignSaveData())
            {
                MenuActionController.StartGame();
            }
            else
            {
                StartCoroutine(HideCoroutine(characterSelectMenu));
            }

        }
        public void TutorialClicked()
        {
            SceneTransferData.Instance.Reset();
            SceneTransferData.Instance.EnemyArmyData = tutorialBattleData;
            SceneTransferData.Instance.TutorialBattle1 = true;
            SceneController.LoadSceneAsync(Scenes.Battle1, false);
        }
        
       
        public void UpgradeClicked()
        {
            StartCoroutine(HideCoroutine(upgradeMenu));
            //upgradeMenu.Show();
            //Hide();
        }
   
    }
}
